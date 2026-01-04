$connStr = "Data Source=SQL5106.site4now.net;Initial Catalog=db_ac2fe4_todo;User Id=db_ac2fe4_todo_admin;Password=taskquest2025;Connect Timeout=30"

function Execute-Query($conn, $query, $params) {
    $cmd = $conn.CreateCommand()
    $cmd.CommandText = $query
    if ($params) {
        foreach ($key in $params.Keys) {
            $cmd.Parameters.AddWithValue($key, $params[$key]) | Out-Null
        }
    }
    $da = New-Object System.Data.SqlClient.SqlDataAdapter($cmd)
    $dt = New-Object System.Data.DataTable
    $da.Fill($dt) | Out-Null
    return $dt
}

function Execute-NonQuery($conn, $query, $params) {
    $cmd = $conn.CreateCommand()
    $cmd.CommandText = $query
    if ($params) {
        foreach ($key in $params.Keys) {
            $cmd.Parameters.AddWithValue($key, $params[$key]) | Out-Null
        }
    }
    return $cmd.ExecuteScalar() # Returns ID or null
}

try {
    $conn = New-Object System.Data.SqlClient.SqlConnection
    $conn.ConnectionString = $connStr
    $conn.Open()

    $suffix = (Get-Date).Ticks
    $userA = "UserA_$suffix"
    $userB = "UserB_$suffix"

    Write-Host "Setting up test data..."

    # 1. Create Users
    $idA = Execute-NonQuery $conn "INSERT INTO Users (Username, PasswordHash, Email, FullName) VALUES (@U, 'hash', @U+'@t.com', 'User A'); SELECT SCOPE_IDENTITY();" @{U=$userA}
    $idB = Execute-NonQuery $conn "INSERT INTO Users (Username, PasswordHash, Email, FullName) VALUES (@U, 'hash', @U+'@t.com', 'User B'); SELECT SCOPE_IDENTITY();" @{U=$userB}

    # 2. Create Teams
    # TeamA created by UserA
    $teamA = Execute-NonQuery $conn "INSERT INTO Team (TeamName, CreatorUsername, UpdatedAt) VALUES ('Team A', @U, GETDATE()); SELECT SCOPE_IDENTITY();" @{U=$userA}
    # TeamB created by UserB
    $teamB = Execute-NonQuery $conn "INSERT INTO Team (TeamName, CreatorUsername, UpdatedAt) VALUES ('Team B', @U, GETDATE()); SELECT SCOPE_IDENTITY();" @{U=$userB}

    # 3. Add UserA as member of TeamB (to test exclusion)
    Execute-NonQuery $conn "INSERT INTO TeamMembers (TeamId, Username, Role) VALUES (@T, @U, 'member');" @{T=$teamB; U=$userA}

    # 4. Create Projects
    # ProjectA in TeamA
    $projA = Execute-NonQuery $conn "INSERT INTO Projects (ProjectName, TeamAccessId, CreatorUserId, CreatedAt) VALUES ('Project A', @T, @U, GETDATE()); SELECT SCOPE_IDENTITY();" @{T=$teamA; U=$idA}
    # Link ProjectA to TeamA in ProjectTeams
    Execute-NonQuery $conn "INSERT INTO ProjectTeams (ProjectID, TeamID) VALUES (@P, @T);" @{P=$projA; T=$teamA}

    # ProjectB in TeamB
    $projB = Execute-NonQuery $conn "INSERT INTO Projects (ProjectName, TeamAccessId, CreatorUserId, CreatedAt) VALUES ('Project B', @T, @U, GETDATE()); SELECT SCOPE_IDENTITY();" @{T=$teamB; U=$idB}
    Execute-NonQuery $conn "INSERT INTO ProjectTeams (ProjectID, TeamID) VALUES (@P, @T);" @{P=$projB; T=$teamB}


    # --- TEST 1: Teams Query for UserA ---
    Write-Host "`nTesting Teams Query for $userA..."
    # Copying query from Teams.aspx.cs (modified version)
    $teamsQuery = @"
        SELECT t.TeamId, t.TeamName 
        FROM Team t 
        WHERE t.CreatorUsername = @CurrentUser
"@
    $teams = Execute-Query $conn $teamsQuery @{CurrentUser=$userA}
    
    $foundTeamA = $false
    $foundTeamB = $false
    foreach ($row in $teams) {
        if ($row["TeamId"] -eq $teamA) { $foundTeamA = $true }
        if ($row["TeamId"] -eq $teamB) { $foundTeamB = $true }
    }

    if ($foundTeamA -and -not $foundTeamB) {
        Write-Host "PASSED: UserA sees TeamA but NOT TeamB (where they are just a member)." -ForegroundColor Green
    } else {
        Write-Host "FAILED: Teams visibility incorrect." -ForegroundColor Red
        Write-Host "  Found TeamA: $foundTeamA"
        Write-Host "  Found TeamB: $foundTeamB (Should be False)"
    }


    # --- TEST 2: Projects Query for UserA ---
    Write-Host "`nTesting Projects Query for $userA..."
    # Copying query from Projects.aspx.cs (modified version)
    $projectsQuery = @"
        SELECT DISTINCT p.ProjectID, p.ProjectName
        FROM Projects p
        LEFT JOIN ProjectTeams pt ON p.ProjectID = pt.ProjectID
        LEFT JOIN Team t ON pt.TeamID = t.TeamId
        WHERE (t.CreatorUsername = @CurrentUser) 
           OR (p.CreatorUserId = @CurrentUserId)
"@
    $projects = Execute-Query $conn $projectsQuery @{CurrentUser=$userA; CurrentUserId=$idA}

    $foundProjA = $false
    $foundProjB = $false
    foreach ($row in $projects) {
        if ($row["ProjectID"] -eq $projA) { $foundProjA = $true }
        if ($row["ProjectID"] -eq $projB) { $foundProjB = $true }
    }

    if ($foundProjA -and -not $foundProjB) {
        Write-Host "PASSED: UserA sees ProjectA but NOT ProjectB." -ForegroundColor Green
    } else {
        Write-Host "FAILED: Projects visibility incorrect." -ForegroundColor Red
        Write-Host "  Found ProjA: $foundProjA"
        Write-Host "  Found ProjB: $foundProjB (Should be False)"
    }

    # Clean up
    Write-Host "`nCleaning up..."
    # We can use our cascade delete SP!
    $cmd = $conn.CreateCommand()
    $cmd.CommandText = "sp_DeleteUserCascade"
    $cmd.CommandType = [System.Data.CommandType]::StoredProcedure
    $cmd.Parameters.AddWithValue("@Username", $userA) | Out-Null
    $cmd.ExecuteNonQuery()

    $cmd.Parameters["@Username"].Value = $userB
    $cmd.ExecuteNonQuery()
    Write-Host "Cleanup done."

}
catch {
    Write-Host "Error: $($_.Exception.Message)"
    Write-Host "Stack Trace: $($_.Exception.StackTrace)"
}
finally {
    if ($conn -and $conn.State -eq 'Open') { $conn.Close() }
}
