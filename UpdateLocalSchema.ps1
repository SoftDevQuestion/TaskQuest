$connStr = "Data Source=DESKTOP-LNCBN7V\ELER;Initial Catalog=TodoAppDB;Integrated Security=True;"
try {
    $conn = New-Object System.Data.SqlClient.SqlConnection
    $conn.ConnectionString = $connStr
    $conn.Open()
    $cmd = $conn.CreateCommand()
    
    # 1. Check if UserID exists, if not add it
    $cmd.CommandText = "IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Tasks' AND COLUMN_NAME = 'UserID') BEGIN ALTER TABLE Tasks ADD UserID INT NULL; ALTER TABLE Tasks ADD CONSTRAINT FK_Tasks_Users FOREIGN KEY (UserID) REFERENCES Users(UserID); END"
    $cmd.ExecuteNonQuery()
    
    # 2. Check if AvatarPath exists in Users (just in case)
    $cmd.CommandText = "IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Users' AND COLUMN_NAME = 'AvatarPath') BEGIN ALTER TABLE Users ADD AvatarPath NVARCHAR(255) NULL; END"
    $cmd.ExecuteNonQuery()

    $conn.Close()
    Write-Host "Schema updated successfully for Local DB (ELER)."
} catch {
    Write-Host "Error: $($_.Exception.Message)"
}
