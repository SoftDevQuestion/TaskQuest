-- 1. Fix Blocking Trigger (Allow Cascade Updates)
GO
ALTER TRIGGER trg_Block_Project_Status_Manual_Update
ON Projects
AFTER UPDATE
AS
BEGIN
    -- Allow updates initiated by the Tasks trigger (Cascade Update)
    IF TRIGGER_NESTLEVEL(OBJECT_ID('trg_Sync_Project_With_Tasks')) > 0
        RETURN;

    IF UPDATE(DoneStatusCount)
       OR UPDATE(InProgressStatusCount)
       OR UPDATE(ToDoStatusCount)
       OR UPDATE(UndefinedStatusCount)
    BEGIN
        RAISERROR (
            N'Updates to status counts are only allowed via Task changes.',
            16, 1
        );
        ROLLBACK TRANSACTION;
    END
END;
GO

-- 2. Ensure Correct Cascade Delete Procedure
IF OBJECT_ID('sp_DeleteUserCascade', 'P') IS NOT NULL
    DROP PROCEDURE sp_DeleteUserCascade;
GO

CREATE PROCEDURE sp_DeleteUserCascade
    @Username NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @UserId INT;
        SELECT @UserId = UserID FROM Users WHERE Username = @Username;

        -- 1. Delete UserProfile FIRST (Foreign Key Constraint)
        IF @UserId IS NOT NULL
            DELETE FROM UserProfile WHERE UserId = @UserId;

        -- 2. Identify Teams Created by User
        DECLARE @CreatedTeams TABLE (TeamId INT);
        INSERT INTO @CreatedTeams SELECT TeamId FROM Team WHERE CreatorUsername = @Username;

        -- 3. Identify Projects in those Teams
        DECLARE @TeamProjects TABLE (ProjectId INT);
        INSERT INTO @TeamProjects 
        SELECT ProjectId FROM Projects WHERE TeamAccessId IN (SELECT TeamId FROM @CreatedTeams);

        -- 4. Delete Tasks (Now allowed by trigger fix)
        DELETE FROM Tasks WHERE ProjectId IN (SELECT ProjectId FROM @TeamProjects);

        -- 5. Delete ProjectTeams & Projects
        DELETE FROM ProjectTeams WHERE ProjectID IN (SELECT ProjectId FROM @TeamProjects);
        DELETE FROM Projects WHERE ProjectId IN (SELECT ProjectId FROM @TeamProjects);

        -- 6. Delete TeamMembers & Teams
        DELETE FROM TeamMembers WHERE TeamId IN (SELECT TeamId FROM @CreatedTeams);
        DELETE FROM ProjectTeams WHERE TeamID IN (SELECT TeamId FROM @CreatedTeams);
        DELETE FROM Team WHERE TeamId IN (SELECT TeamId FROM @CreatedTeams);

        -- 7. Cleanup User Memberships
        DELETE FROM TeamMembers WHERE Username = @Username;

        -- 8. Cleanup Orphaned Projects
        IF @UserId IS NOT NULL
        BEGIN
            DECLARE @UserProjects TABLE (ProjectId INT);
            INSERT INTO @UserProjects SELECT ProjectId FROM Projects WHERE CreatorUserId = @UserId;

            DELETE FROM Tasks WHERE ProjectId IN (SELECT ProjectId FROM @UserProjects);
            DELETE FROM ProjectTeams WHERE ProjectID IN (SELECT ProjectId FROM @UserProjects);
            DELETE FROM Projects WHERE ProjectId IN (SELECT ProjectId FROM @UserProjects);
        END

        -- 9. Delete User
        DELETE FROM Users WHERE Username = @Username;

        COMMIT TRANSACTION;
        PRINT 'User deleted successfully with full cascade.';
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        DECLARE @Msg NVARCHAR(MAX) = ERROR_MESSAGE();
        RAISERROR(@Msg, 16, 1);
    END CATCH
END
GO
