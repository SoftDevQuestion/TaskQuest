$connStr = "Data Source=SQL5106.site4now.net;Initial Catalog=db_ac2fe4_todo;User Id=db_ac2fe4_todo_admin;Password=taskquest2025;Connect Timeout=30"
try {
    $conn = New-Object System.Data.SqlClient.SqlConnection
    $conn.ConnectionString = $connStr
    $conn.Open()
    $cmd = $conn.CreateCommand()
    $cmd.CommandText = "IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Tasks' AND COLUMN_NAME = 'UserID') BEGIN ALTER TABLE Tasks ADD UserID INT NULL; ALTER TABLE Tasks ADD CONSTRAINT FK_Tasks_Users FOREIGN KEY (UserID) REFERENCES Users(UserID); END"
    $cmd.ExecuteNonQuery()
    $conn.Close()
    Write-Host "Schema updated successfully: Added UserID to Tasks table."
} catch {
    Write-Host "Error: $($_.Exception.Message)"
}
