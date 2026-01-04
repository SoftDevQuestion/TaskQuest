$remoteConnStr = "Data Source=SQL5106.site4now.net;Initial Catalog=db_ac2fe4_todo;User Id=db_ac2fe4_todo_admin;Password=taskquest2025;Connect Timeout=30"

try {
    Write-Host "Verifying Remote Database..."

    # 1. Check Column
    $colCheck = Invoke-Sqlcmd -ConnectionString $remoteConnStr -Query "SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Team' AND COLUMN_NAME = 'CreatorUsername'"
    if ($colCheck[0] -eq 1) {
        Write-Host " [PASS] Column 'CreatorUsername' exists in 'Team' table." -ForegroundColor Green
    } else {
        Write-Host " [FAIL] Column 'CreatorUsername' MISSING!" -ForegroundColor Red
    }

    # 2. Check SP
    $spCheck = Invoke-Sqlcmd -ConnectionString $remoteConnStr -Query "SELECT COUNT(*) FROM sys.objects WHERE type = 'P' AND name = 'sp_DeleteUserCascade'"
    if ($spCheck[0] -eq 1) {
        Write-Host " [PASS] Stored Procedure 'sp_DeleteUserCascade' exists." -ForegroundColor Green
    } else {
        Write-Host " [FAIL] Stored Procedure 'sp_DeleteUserCascade' MISSING!" -ForegroundColor Red
    }

    # 3. Check Data Sample (CreatorUsername not null)
    $nullCount = Invoke-Sqlcmd -ConnectionString $remoteConnStr -Query "SELECT COUNT(*) FROM Team WHERE CreatorUsername IS NULL"
    if ($nullCount[0] -eq 0) {
        Write-Host " [PASS] No NULL values in 'CreatorUsername'." -ForegroundColor Green
    } else {
        Write-Host " [FAIL] Found $($nullCount[0]) NULL values in 'CreatorUsername'." -ForegroundColor Red
    }

} catch {
    Write-Host "Error connecting to remote DB: $($_.Exception.Message)" -ForegroundColor Red
}
