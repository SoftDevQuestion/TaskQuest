$connStr = "Data Source=SQL5106.site4now.net;Initial Catalog=db_ac2fe4_todo;User Id=db_ac2fe4_todo_admin;Password=taskquest2025;Connect Timeout=30"

try {
    $conn = New-Object System.Data.SqlClient.SqlConnection
    $conn.ConnectionString = $connStr
    $conn.Open()
    
    $cmd = $conn.CreateCommand()
    $cmd.CommandText = "SELECT name, OBJECT_DEFINITION(object_id) FROM sys.triggers WHERE parent_id = OBJECT_ID('Projects')"
    $reader = $cmd.ExecuteReader()
    
    while ($reader.Read()) {
        Write-Host "Trigger: $($reader[0])"
        Write-Host "Definition:"
        Write-Host $reader[1]
        Write-Host "----------------"
    }
}
catch {
    Write-Host "Error: $($_.Exception.Message)"
}
finally {
    if ($conn -and $conn.State -eq 'Open') { $conn.Close() }
}
