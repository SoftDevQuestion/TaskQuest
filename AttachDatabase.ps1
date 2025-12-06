$connectionString = "Data Source=(LocalDB)\MSSQLLocalDB;Integrated Security=True"
try {
    $conn = New-Object System.Data.SqlClient.SqlConnection($connectionString)
    $conn.Open()
    $cmd = $conn.CreateCommand()
    
    # Try to attach the database
    $dbName = "TodoAppDB_Attached"
    $mdfPath = "P:\Dbs\ToDo.mdf"
    $ldfPath = "P:\Dbs\ToDo_log.ldf"
    
    Write-Output "Attempting to attach database '$dbName' from '$mdfPath'..."
    
    $cmd.CommandText = "CREATE DATABASE [$dbName] ON (FILENAME = '$mdfPath'), (FILENAME = '$ldfPath') FOR ATTACH"
    $cmd.ExecuteNonQuery()
    
    Write-Output "Successfully attached database!"
    $conn.Close()
} catch {
    Write-Output "Error attaching database: $($_.Exception.Message)"
}
