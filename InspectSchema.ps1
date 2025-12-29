$source = "P:\Dbs\ToDo.mdf"
$dest = "p:\TaskQuest\TaskQuest\TempDB_Inspect.mdf"
try {
    Copy-Item $source $dest -Force
    Write-Output "Copy successful"
} catch {
    Write-Output "Copy failed: $($_.Exception.Message)"
    exit
}

$connectionString = "Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=$dest;Integrated Security=True;Connect Timeout=30"
try {
    $conn = New-Object System.Data.SqlClient.SqlConnection($connectionString)
    $conn.Open()
    $cmd = $conn.CreateCommand()
    $cmd.CommandText = "SELECT t.TABLE_NAME, c.COLUMN_NAME, c.DATA_TYPE FROM INFORMATION_SCHEMA.TABLES t JOIN INFORMATION_SCHEMA.COLUMNS c ON t.TABLE_NAME = c.TABLE_NAME WHERE t.TABLE_TYPE = 'BASE TABLE' ORDER BY t.TABLE_NAME, c.ORDINAL_POSITION"
    $reader = $cmd.ExecuteReader()
    while ($reader.Read()) {
        Write-Output "$($reader['TABLE_NAME']) : $($reader['COLUMN_NAME']) - $($reader['DATA_TYPE'])"
    }
    $conn.Close()
} catch {
    Write-Output "Error: $($_.Exception.Message)"
}

Remove-Item $dest -Force -ErrorAction SilentlyContinue
