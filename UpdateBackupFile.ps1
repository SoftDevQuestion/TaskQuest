$backupFile = "p:\TaskQuest\New folder\TaskQuest - web version\Database\ToDo_Backup.bak"
$tempDbName = "Temp_ToDo_Update_$(Get-Random)"
$localConnStr = "Data Source=(localdb)\MSSQLLocalDB;Integrated Security=True;Connect Timeout=30"
$remoteConnStr = "Data Source=SQL5106.site4now.net;Initial Catalog=db_ac2fe4_todo;User Id=db_ac2fe4_todo_admin;Password=taskquest2025;Connect Timeout=30"

# Temp File Paths
$tempMdf = "$env:TEMP\$tempDbName.mdf"
$tempLdf = "$env:TEMP\$tempDbName.ldf"

try {
    Write-Host "1. Restoring Backup to LocalDB ($tempDbName)..."
    # Check if DB exists and drop
    Invoke-Sqlcmd -ConnectionString $localConnStr -Query "IF EXISTS (SELECT name FROM sys.databases WHERE name = N'$tempDbName') DROP DATABASE [$tempDbName]"
    
    $restoreCmd = "RESTORE DATABASE [$tempDbName] FROM DISK = '$backupFile' WITH FILE = 1, MOVE 'ToDo' TO '$tempMdf', MOVE 'ToDo_log' TO '$tempLdf', REPLACE"
    Invoke-Sqlcmd -ConnectionString $localConnStr -Query $restoreCmd

    Write-Host "2. Applying Migrations to Local Temp DB..."
    # A. Add Column
    try {
        Invoke-Sqlcmd -ConnectionString "$localConnStr;Initial Catalog=$tempDbName" -Query "ALTER TABLE Team ADD CreatorUsername NVARCHAR(50) NULL"
        Write-Host "   - Added CreatorUsername column."
    } catch { 
        # Check if error is because column exists
        if ($_.Exception.Message -like "*Column names in each table must be unique*") {
            Write-Host "   - CreatorUsername column already exists."
        } else {
            Write-Host "   - Error adding column (might be okay if exists): $($_.Exception.Message)"
        }
    }

    # B. Backfill
    Invoke-Sqlcmd -ConnectionString "$localConnStr;Initial Catalog=$tempDbName" -Query "UPDATE Team SET CreatorUsername = 'admin' WHERE CreatorUsername IS NULL"
    Write-Host "   - Backfilled CreatorUsername."

    # C. Create SP
    $spQuery = @"
CREATE PROCEDURE sp_DeleteUserCascade
    @Username NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        -- 1. Identify Teams Created by User
        DECLARE @CreatedTeams TABLE (TeamId INT);
        INSERT INTO @CreatedTeams SELECT TeamId FROM Team WHERE CreatorUsername = @Username;

        -- 2. Identify Projects related to these teams
        DECLARE @TeamProjects TABLE (ProjectId INT);
        INSERT INTO @TeamProjects 
        SELECT ProjectId FROM Projects WHERE TeamAccessId IN (SELECT TeamId FROM @CreatedTeams);

        -- 3. Delete TASKS associated with these projects
        DELETE FROM Tasks WHERE ProjectId IN (SELECT ProjectId FROM @TeamProjects);

        -- 4. Delete ProjectTeams for these projects
        DELETE FROM ProjectTeams WHERE ProjectID IN (SELECT ProjectId FROM @TeamProjects);

        -- 5. Delete these Projects
        DELETE FROM Projects WHERE ProjectId IN (SELECT ProjectId FROM @TeamProjects);

        -- 6. Delete TeamMembers for Created Teams
        DELETE FROM TeamMembers WHERE TeamId IN (SELECT TeamId FROM @CreatedTeams);

        -- 7. Delete ProjectTeams for Created Teams
        DELETE FROM ProjectTeams WHERE TeamID IN (SELECT TeamId FROM @CreatedTeams);

        -- 8. Delete Created Teams
        DELETE FROM Team WHERE TeamId IN (SELECT TeamId FROM @CreatedTeams);

        -- 9. Clean up User membership in OTHER teams
        DELETE FROM TeamMembers WHERE Username = @Username;

        -- 10. Delete Projects created by user (orphaned)
        DECLARE @UserId INT;
        SELECT @UserId = UserID FROM Users WHERE Username = @Username;

        IF @UserId IS NOT NULL
        BEGIN
            DECLARE @UserProjects TABLE (ProjectId INT);
            INSERT INTO @UserProjects SELECT ProjectId FROM Projects WHERE CreatorUserId = @UserId;

            DELETE FROM Tasks WHERE ProjectId IN (SELECT ProjectId FROM @UserProjects);
            DELETE FROM ProjectTeams WHERE ProjectID IN (SELECT ProjectId FROM @UserProjects);
            DELETE FROM Projects WHERE ProjectId IN (SELECT ProjectId FROM @UserProjects);
        END

        -- 11. Delete User
        DELETE FROM Users WHERE Username = @Username;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR (@ErrorMessage, 16, 1);
    END CATCH
END
"@
    try {
        Invoke-Sqlcmd -ConnectionString "$localConnStr;Initial Catalog=$tempDbName" -Query "DROP PROCEDURE IF EXISTS sp_DeleteUserCascade"
        Invoke-Sqlcmd -ConnectionString "$localConnStr;Initial Catalog=$tempDbName" -Query $spQuery
        Write-Host "   - Created sp_DeleteUserCascade."
    } catch { Write-Host "   - Error creating SP: $_" }

    Write-Host "3. Overwriting Backup File..."
    # Ensure no active connections
    Invoke-Sqlcmd -ConnectionString $localConnStr -Query "ALTER DATABASE [$tempDbName] SET SINGLE_USER WITH ROLLBACK IMMEDIATE"
    
    $backupCmd = "BACKUP DATABASE [$tempDbName] TO DISK = '$backupFile' WITH FORMAT, INIT"
    Invoke-Sqlcmd -ConnectionString $localConnStr -Query $backupCmd
    Write-Host "   - Backup file updated successfully."

    # Verify Remote DB just in case
    Write-Host "4. Verifying Remote Database State..."
    $checkRemote = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Team' AND COLUMN_NAME = 'CreatorUsername'"
    $res = Invoke-Sqlcmd -ConnectionString $remoteConnStr -Query $checkRemote
    if ($res[0] -eq 1) { Write-Host "   - Remote DB confirmed to have CreatorUsername." -ForegroundColor Green }
    else { Write-Host "   - WARNING: Remote DB missing CreatorUsername!" -ForegroundColor Red }

} catch {
    Write-Host "ERROR: $($_.Exception.Message)" -ForegroundColor Red
} finally {
    # Cleanup
    Write-Host "Cleaning up..."
    try {
        Invoke-Sqlcmd -ConnectionString $localConnStr -Query "DROP DATABASE [$tempDbName]"
    } catch {}
    
    if (Test-Path $tempMdf) { Remove-Item $tempMdf -ErrorAction SilentlyContinue }
    if (Test-Path $tempLdf) { Remove-Item $tempLdf -ErrorAction SilentlyContinue }
}
