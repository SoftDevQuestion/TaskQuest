$remoteConnStr = "Data Source=SQL5106.site4now.net;Initial Catalog=db_ac2fe4_todo;User Id=db_ac2fe4_todo_admin;Password=taskquest2025;Connect Timeout=30"

$spQuery = @"
ALTER PROCEDURE sp_DeleteUserCascade
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

        -- 10. Delete Projects created by user (orphaned) AND UserProfile
        DECLARE @UserId INT;
        SELECT @UserId = UserID FROM Users WHERE Username = @Username;

        IF @UserId IS NOT NULL
        BEGIN
            DECLARE @UserProjects TABLE (ProjectId INT);
            INSERT INTO @UserProjects SELECT ProjectId FROM Projects WHERE CreatorUserId = @UserId;

            DELETE FROM Tasks WHERE ProjectId IN (SELECT ProjectId FROM @UserProjects);
            DELETE FROM ProjectTeams WHERE ProjectID IN (SELECT ProjectId FROM @UserProjects);
            DELETE FROM Projects WHERE ProjectId IN (SELECT ProjectId FROM @UserProjects);

            -- NEW: Delete UserProfile
            DELETE FROM UserProfile WHERE UserId = @UserId;
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
    Write-Host "Updating sp_DeleteUserCascade on Remote DB..."
    Invoke-Sqlcmd -ConnectionString $remoteConnStr -Query $spQuery
    Write-Host "Successfully updated stored procedure." -ForegroundColor Green
} catch {
    Write-Host "Error updating SP: $($_.Exception.Message)" -ForegroundColor Red
}
