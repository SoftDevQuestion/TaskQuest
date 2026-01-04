-- Migration Script for Cascade Delete and Team Ownership
-- Generated: 2026-01-04

-- 1. Add CreatorUsername to Team Table
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Team' AND COLUMN_NAME = 'CreatorUsername')
BEGIN
    ALTER TABLE Team ADD CreatorUsername NVARCHAR(50) NULL;
    PRINT 'Added CreatorUsername column to Team table.';
END
GO

-- 2. Backfill Existing Teams (Default to 'admin')
UPDATE Team SET CreatorUsername = 'admin' WHERE CreatorUsername IS NULL;
PRINT 'Backfilled CreatorUsername with default value.';
GO

-- 3. Create/Update Stored Procedure sp_DeleteUserCascade
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

        -- 0. Get User ID
        DECLARE @UserId INT;
        SELECT @UserId = UserID FROM Users WHERE Username = @Username;

        -- 1. Delete UserProfile (Direct Child of Users)
        IF @UserId IS NOT NULL
        BEGIN
            DELETE FROM UserProfile WHERE UserId = @UserId;
        END

        -- 2. Identify Teams Created by User
        DECLARE @CreatedTeams TABLE (TeamId INT);
        INSERT INTO @CreatedTeams SELECT TeamId FROM Team WHERE CreatorUsername = @Username;

        -- 3. Identify Projects related to these teams
        DECLARE @TeamProjects TABLE (ProjectId INT);
        INSERT INTO @TeamProjects 
        SELECT ProjectId FROM Projects WHERE TeamAccessId IN (SELECT TeamId FROM @CreatedTeams);

        -- 4. Delete TASKS associated with these projects
        DELETE FROM Tasks WHERE ProjectId IN (SELECT ProjectId FROM @TeamProjects);

        -- 5. Delete ProjectTeams for these projects
        DELETE FROM ProjectTeams WHERE ProjectID IN (SELECT ProjectId FROM @TeamProjects);

        -- 6. Delete these Projects
        DELETE FROM Projects WHERE ProjectId IN (SELECT ProjectId FROM @TeamProjects);

        -- 7. Delete TeamMembers for Created Teams
        DELETE FROM TeamMembers WHERE TeamId IN (SELECT TeamId FROM @CreatedTeams);

        -- 8. Delete ProjectTeams for Created Teams
        DELETE FROM ProjectTeams WHERE TeamID IN (SELECT TeamId FROM @CreatedTeams);

        -- 9. Delete Created Teams
        DELETE FROM Team WHERE TeamId IN (SELECT TeamId FROM @CreatedTeams);

        -- 10. Clean up User membership in OTHER teams
        DELETE FROM TeamMembers WHERE Username = @Username;

        -- 11. Delete Projects created by user (orphaned)
        IF @UserId IS NOT NULL
        BEGIN
            DECLARE @UserProjects TABLE (ProjectId INT);
            INSERT INTO @UserProjects SELECT ProjectId FROM Projects WHERE CreatorUserId = @UserId;

            DELETE FROM Tasks WHERE ProjectId IN (SELECT ProjectId FROM @UserProjects);
            DELETE FROM ProjectTeams WHERE ProjectID IN (SELECT ProjectId FROM @UserProjects);
            DELETE FROM Projects WHERE ProjectId IN (SELECT ProjectId FROM @UserProjects);
        END

        -- 12. Delete User
        DELETE FROM Users WHERE Username = @Username;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR (@ErrorMessage, 16, 1);
    END CATCH
END
GO

PRINT 'Stored Procedure sp_DeleteUserCascade created/updated successfully.';
