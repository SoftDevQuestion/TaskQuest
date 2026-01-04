$connStr = "Data Source=DESKTOP-LNCBN7V\ELER;Initial Catalog=TodoAppDB;Integrated Security=True;"
$conn = New-Object System.Data.SqlClient.SqlConnection($connStr)
$conn.Open()
$cmd = $conn.CreateCommand()
$cmd.CommandText = "SELECT Top 10 Title, StatusId, DueDate FROM Tasks"
$adapter = New-Object System.Data.SqlClient.SqlDataAdapter($cmd)
$dt = New-Object System.Data.DataTable
$adapter.Fill($dt)
$conn.Close()

foreach ($row in $dt.Rows) {
    Write-Host "Title: $($row['Title']), Status: $($row['StatusId']), Due: $($row['DueDate'])"
}
