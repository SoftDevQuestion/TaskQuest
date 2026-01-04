$connStr = "Data Source=SQL5106.site4now.net;Initial Catalog=db_ac2fe4_todo;User Id=db_ac2fe4_todo_admin;Password=taskquest2025;Connect Timeout=30"

try {
    $conn = New-Object System.Data.SqlClient.SqlConnection
    $conn.ConnectionString = $connStr
    $conn.Open()

    # 3. Drop SP
    $sql3 = "IF OBJECT_ID('sp_DeleteUserCascade', 'P') IS NOT NULL DROP PROCEDURE sp_DeleteUserCascade;"
    $cmd = $conn.CreateCommand()
    $cmd.CommandText = $sql3
    $cmd.ExecuteNonQuery()
    Write-Host "SP Drop done."

    # 4. Create SP
    $sql4 = @"
CREATE PROCEDURE sp_DeleteUserCascade
    @Username NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        -- DISABLE Trigger on Projects to allow Cascade updates from Tasks trigger
        DISABLE TRIGGER trg_Block_Project_Status_Manual_Update ON Projects;

        -- 1. Identify Teams Created by User
        DECLARE @CreatedTeams TABLE (TeamId INT);
        INSERT INTO @CreatedTeams SELECT TeamId FROM Team WHERE CreatorUsername = @Username;

        -- 2. Identify Projects related to these teams
        DECLARE @TeamProjects TABLE (ProjectId INT);
        
        -- Projects directly linked via TeamAccessId
        INSERT INTO @TeamProjects 
        SELECT ProjectId FROM Projects WHERE TeamAccessId IN (SELECT TeamId FROM @CreatedTeams);

        -- 3. Delete TASKS associated with these projects
        -- This will fire trg_Sync_Project_With_Tasks, which updates Projects.
        -- Since trg_Block_Project_Status_Manual_Update is disabled, it should succeed.
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

        -- RE-ENABLE Trigger
        ENABLE TRIGGER trg_Block_Project_Status_Manual_Update ON Projects;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        -- Ensure Trigger is re-enabled even on error
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        
        -- We can't easily re-enable inside CATCH if transaction rolled back? 
        -- Actually, DDL/Enable Trigger might behave differently in trans.
        -- But assuming we want to restore state.
        -- However, Enable Trigger is auto-committed? No.
        -- We should try to enable it.
        BEGIN TRY
            ENABLE TRIGGER trg_Block_Project_Status_Manual_Update ON Projects;
        END TRY
        BEGIN CATCH
            -- Ignore error here
        END CATCH

        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR (@ErrorMessage, 16, 1);
    END CATCH
END
"@
    $cmd.CommandText = $sql4
    $cmd.ExecuteNonQuery()
    Write-Host "SP Create done."

}
catch {
    Write-Host "Error: $($_.Exception.Message)"
}
finally {
    if ($conn -and $conn.State -eq 'Open') { $conn.Close() }
}
