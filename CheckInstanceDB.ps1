$instance = ".\MSRDINANI"
$targetFile = "ToDo.mdf"

$connString = "Data Source=$instance;Initial Catalog=master;Integrated Security=True"
try {
    $conn = New-Object System.Data.SqlClient.SqlConnection($connString)
    $conn.Open()
    
    $cmd = $conn.CreateCommand()
    $cmd.CommandText = "SELECT DB_NAME(database_id) as DbName, physical_name FROM sys.master_files WHERE physical_name LIKE '%$targetFile%'"
    $reader = $cmd.ExecuteReader()
    
    if ($reader.Read()) {
        Write-Output "FOUND: Database '$($reader['DbName'])' at '$($reader['physical_name'])'"
    } else {
        Write-Output "NOT FOUND: No database matching '$targetFile' on instance '$instance'"
    }
    $conn.Close()
} catch {
    Write-Output "Error connecting to $instance : $($_.Exception.Message)"
}
