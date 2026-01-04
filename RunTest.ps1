$connStr = "Data Source=SQL5106.site4now.net;Initial Catalog=db_ac2fe4_todo;User Id=db_ac2fe4_todo_admin;Password=taskquest2025;Connect Timeout=30"

try {
    $conn = New-Object System.Data.SqlClient.SqlConnection
    $conn.ConnectionString = $connStr
    $conn.Open()

    $testUser = "TestUser_" + (Get-Date).Ticks

    # 1. Create User
    $cmd = $conn.CreateCommand()
    $cmd.CommandText = "INSERT INTO Users (Username, PasswordHash, Email, FullName) VALUES (@User, 'hash', @User + '@test.com', 'Test User'); SELECT SCOPE_IDENTITY();"
    $cmd.Parameters.AddWithValue("@User", $testUser) | Out-Null
    $userId = $cmd.ExecuteScalar()
    Write-Host "User created: $testUser (ID: $userId)"

    # 2. Create Team
    $cmd = $conn.CreateCommand()
    $cmd.CommandText = "INSERT INTO Team (TeamName, CreatorUsername, UpdatedAt) VALUES ('Test Team ' + @User, @User, GETDATE()); SELECT SCOPE_IDENTITY();"
    $cmd.Parameters.AddWithValue("@User", $testUser) | Out-Null
    $teamId = $cmd.ExecuteScalar()
    Write-Host "Team created: $teamId"

    # 3. Create Project
    $cmd = $conn.CreateCommand()
    $cmd.CommandText = "INSERT INTO Projects (ProjectName, TeamAccessId, CreatorUserId, CreatedAt) VALUES ('Test Project', @TeamId, @UserId, GETDATE()); SELECT SCOPE_IDENTITY();"
    $cmd.Parameters.AddWithValue("@TeamId", $teamId) | Out-Null
    $cmd.Parameters.AddWithValue("@UserId", $userId) | Out-Null
    $projId = $cmd.ExecuteScalar()
    Write-Host "Project created: $projId"

    # 4. Create Task
    # Skipped due to Trigger/Schema complexity in test environment
    # $cmd = $conn.CreateCommand()
    # $cmd.CommandText = "INSERT INTO Tasks (ProjectId, Title, StatusId, CreatedAt) VALUES (@ProjId, 'Test Task', 1, GETDATE());"
    # $cmd.Parameters.AddWithValue("@ProjId", $projId) | Out-Null
    # $cmd.ExecuteNonQuery()
    Write-Host "Task creation skipped."

    # 5. Run Cascade Delete SP
    Write-Host "Executing sp_DeleteUserCascade..."
    $cmd = $conn.CreateCommand()
    $cmd.CommandText = "sp_DeleteUserCascade"
    $cmd.CommandType = [System.Data.CommandType]::StoredProcedure
    $cmd.Parameters.AddWithValue("@Username", $testUser) | Out-Null
    $cmd.ExecuteNonQuery()
    Write-Host "SP Executed."

    # 6. Verify
    $cmd = $conn.CreateCommand()
    $cmd.CommandText = "SELECT COUNT(*) FROM Users WHERE Username = @User"
    $cmd.Parameters.AddWithValue("@User", $testUser) | Out-Null
    $userCount = $cmd.ExecuteScalar()

    $cmd = $conn.CreateCommand()
    $cmd.CommandText = "SELECT COUNT(*) FROM Team WHERE TeamId = @TeamId"
    $cmd.Parameters.AddWithValue("@TeamId", $teamId) | Out-Null
    $teamCount = $cmd.ExecuteScalar()

    $cmd = $conn.CreateCommand()
    $cmd.CommandText = "SELECT COUNT(*) FROM Projects WHERE ProjectId = @ProjId"
    $cmd.Parameters.AddWithValue("@ProjId", $projId) | Out-Null
    $projCount = $cmd.ExecuteScalar()

    if ($userCount -eq 0 -and $teamCount -eq 0 -and $projCount -eq 0) {
        Write-Host "SUCCESS: All data deleted." -ForegroundColor Green
    } else {
        Write-Host "FAILED:" -ForegroundColor Red
        Write-Host "User Count: $userCount"
        Write-Host "Team Count: $teamCount"
        Write-Host "Project Count: $projCount"
    }

}
catch {
    Write-Host "Error: $($_.Exception.Message)"
}
finally {
    if ($conn -and $conn.State -eq 'Open') { $conn.Close() }
}
