$connString = "Data Source=.\MSRDINANI;Initial Catalog=ToDo;Integrated Security=True"
try {
    $conn = New-Object System.Data.SqlClient.SqlConnection($connString)
    $conn.Open()
    Write-Output "Successfully connected to database 'ToDo' on '.\MSRDINANI'"
    
    $cmd = $conn.CreateCommand()
    $cmd.CommandText = "SELECT COUNT(*) FROM Users"
    $count = $cmd.ExecuteScalar()
    Write-Output "Current user count: $count"
    
    $conn.Close()
} catch {
    Write-Output "Connection failed: $($_.Exception.Message)"
}
