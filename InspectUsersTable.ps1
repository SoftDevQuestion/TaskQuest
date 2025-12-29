$connString = "Data Source=.\MSRDINANI;Initial Catalog=ToDo;Integrated Security=True"
try {
    $conn = New-Object System.Data.SqlClient.SqlConnection($connString)
    $conn.Open()
    
    $cmd = $conn.CreateCommand()
    $cmd.CommandText = @"
        SELECT 
            t.name AS TableName,
            c.name AS ColumnName,
            ty.name AS DataType,
            c.max_length
        FROM sys.tables t
        INNER JOIN sys.columns c ON t.object_id = c.object_id
        INNER JOIN sys.types ty ON c.user_type_id = ty.user_type_id
        WHERE t.name = 'Users'
"@
    
    $adapter = New-Object System.Data.SqlClient.SqlDataAdapter($cmd)
    $dt = New-Object System.Data.DataTable
    $adapter.Fill($dt) | Out-Null
    
    if ($dt.Rows.Count -eq 0) {
        Write-Output "Table 'Users' NOT FOUND in database 'ToDo' on instance '.\MSRDINANI'"
        # List all tables to help debug
        $cmd.CommandText = "SELECT name FROM sys.tables"
        $reader = $cmd.ExecuteReader()
        Write-Output "Available tables:"
        while ($reader.Read()) { Write-Output "- $($reader['name'])" }
    } else {
        Write-Output "Table 'Users' found. Schema:"
        $dt | Format-Table -AutoSize
    }
    
    $conn.Close()
} catch {
    Write-Output "Error: $($_.Exception.Message)"
}
