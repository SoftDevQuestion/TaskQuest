$instances = @("(LocalDB)\MSSQLLocalDB", ".\SQLEXPRESS", "localhost", "(local)")
$targetFile = "P:\Dbs\ToDo.mdf"
$found = $false

foreach ($instance in $instances) {
    Write-Output "Checking instance: $instance"
    $connString = "Data Source=$instance;Initial Catalog=master;Integrated Security=True;Connect Timeout=5"
    try {
        $conn = New-Object System.Data.SqlClient.SqlConnection($connString)
        $conn.Open()
        
        $cmd = $conn.CreateCommand()
        $cmd.CommandText = "SELECT DB_NAME(database_id) as DbName, physical_name FROM sys.master_files WHERE physical_name = '$targetFile'"
        $reader = $cmd.ExecuteReader()
        
        if ($reader.Read()) {
            Write-Output "FOUND! Database '$($reader['DbName'])' on instance '$instance' is using file '$targetFile'"
            $found = $true
        }
        $conn.Close()
    } catch {
        Write-Output "Could not connect to $instance : $($_.Exception.Message)"
    }
    
    if ($found) { break }
}

if (-not $found) {
    Write-Output "File not found attached to any checked instance. It might be detached or on a different instance."
}
