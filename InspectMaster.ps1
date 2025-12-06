$connectionString = "Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True"
try {
    $conn = New-Object System.Data.SqlClient.SqlConnection($connectionString)
    $conn.Open()
    $cmd = $conn.CreateCommand()
    $cmd.CommandText = "SELECT name, physical_name FROM sys.master_files"
    $reader = $cmd.ExecuteReader()
    while ($reader.Read()) {
        Write-Output "$($reader['name']) : $($reader['physical_name'])"
    }
    $conn.Close()
} catch {
    Write-Output "Error: $($_.Exception.Message)"
}
