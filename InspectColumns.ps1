$connStr = "Data Source=SQL5106.site4now.net;Initial Catalog=db_ac2fe4_todo;User Id=db_ac2fe4_todo_admin;Password=taskquest2025;Connect Timeout=30"

try {
    $conn = New-Object System.Data.SqlClient.SqlConnection
    $conn.ConnectionString = $connStr
    $conn.Open()
    $cmd = $conn.CreateCommand()

    $tables = @('Projects', 'projects', 'Tasks', 'tasks', 'Team', 'Users', 'users')
    
    foreach ($t in $tables) {
        $cmd.CommandText = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '$t'"
        $reader = $cmd.ExecuteReader()
        
        Write-Host "Table: $t"
        while ($reader.Read()) {
            Write-Host " - $($reader[0])"
        }
        $reader.Close()
        Write-Host "----------------"
    }
}
catch {
    Write-Host "Error: $($_.Exception.Message)"
}
finally {
    if ($conn -and $conn.State -eq 'Open') { $conn.Close() }
}
