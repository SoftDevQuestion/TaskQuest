$connStr = "Data Source=SQL5106.site4now.net;Initial Catalog=db_ac2fe4_todo;User Id=db_ac2fe4_todo_admin;Password=taskquest2025;Connect Timeout=30"
try {
    $conn = New-Object System.Data.SqlClient.SqlConnection
    $conn.ConnectionString = $connStr
    $conn.Open()
    $cmd = $conn.CreateCommand()
    $cmd.CommandText = @"
        IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Tasks' AND COLUMN_NAME = 'UpdatedAt')
        BEGIN
            ALTER TABLE Tasks ADD UpdatedAt DATETIME NULL;
            PRINT 'Column UpdatedAt added to Tasks table.';
        END
        
        IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Projects' AND COLUMN_NAME = 'UpdatedAt')
        BEGIN
            ALTER TABLE Projects ADD UpdatedAt DATETIME NULL;
            PRINT 'Column UpdatedAt added to Projects table.';
        END
        
        -- Update existing rows to have a value if null
        UPDATE Tasks SET UpdatedAt = CreatedAt WHERE UpdatedAt IS NULL;
        UPDATE Tasks SET UpdatedAt = GETDATE() WHERE UpdatedAt IS NULL;
        
        UPDATE Projects SET UpdatedAt = CreatedAt WHERE UpdatedAt IS NULL;
        UPDATE Projects SET UpdatedAt = GETDATE() WHERE UpdatedAt IS NULL;
"@
    $cmd.ExecuteNonQuery()
    $conn.Close()
    Write-Host "Schema updated successfully: Checked UpdatedAt in Tasks and Projects."
} catch {
    Write-Host "Error: $($_.Exception.Message)"
}
