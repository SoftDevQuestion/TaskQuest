-- Fix_Delete_With_Trigger.sql
-- This script creates an INSTEAD OF DELETE trigger on the Users table.
-- This allows 'DELETE FROM Users WHERE ...' to work directly by handling
-- all foreign key constraints and cascade logic automatically.

IF OBJECT_ID('trg_Users_Cascade_Delete_Seamless', 'TR') IS NOT NULL
    DROP TRIGGER trg_Users_Cascade_Delete_Seamless;
GO

CREATE TRIGGER trg_Users_Cascade_Delete_Seamless
ON Users
INSTEAD OF DELETE
AS
BEGIN
    SET NOCOUNT ON;
    
    -- 1. Identify Users to Delete
    SELECT UserID, Username INTO #UsersToDelete FROM deleted;
    
    -- 2. Delete UserProfiles (Fixes FK_UserProfile_Users error)
    DELETE FROM UserProfile WHERE UserId IN (SELECT UserID FROM #UsersToDelete);
    
    -- 3. Identify Teams Created by these Users
    SELECT TeamId INTO #CreatedTeams FROM Team WHERE CreatorUsername IN (SELECT Username FROM #UsersToDelete);
    
    -- 4. Identify Projects in these Teams
    SELECT ProjectId INTO #TeamProjects FROM Projects WHERE TeamAccessId IN (SELECT TeamId FROM #CreatedTeams);
    
    -- 5. Delete Tasks (Cascading from Team Projects)
    DELETE FROM Tasks WHERE ProjectId IN (SELECT ProjectId FROM #TeamProjects);
    
    -- 6. Delete ProjectTeams & Projects (from Teams)
    DELETE FROM ProjectTeams WHERE ProjectID IN (SELECT ProjectId FROM #TeamProjects);
    DELETE FROM Projects WHERE ProjectId IN (SELECT ProjectId FROM #TeamProjects);
    
    -- 7. Delete TeamMembers & Teams (Created Teams)
    DELETE FROM TeamMembers WHERE TeamId IN (SELECT TeamId FROM #CreatedTeams);
    DELETE FROM ProjectTeams WHERE TeamID IN (SELECT TeamId FROM #CreatedTeams);
    DELETE FROM Team WHERE TeamId IN (SELECT TeamId FROM #CreatedTeams);
    
    -- 8. Cleanup User Memberships (The users themselves in other teams)
    DELETE FROM TeamMembers WHERE Username IN (SELECT Username FROM #UsersToDelete);
    
    -- 9. Cleanup Orphaned Projects (Created by Users directly)
    SELECT ProjectId INTO #UserProjects FROM Projects WHERE CreatorUserId IN (SELECT UserID FROM #UsersToDelete);
    
    DELETE FROM Tasks WHERE ProjectId IN (SELECT ProjectId FROM #UserProjects);
    DELETE FROM ProjectTeams WHERE ProjectID IN (SELECT ProjectId FROM #UserProjects);
    DELETE FROM Projects WHERE ProjectId IN (SELECT ProjectId FROM #UserProjects);
    
    -- 10. Finally Delete Users
    -- This performs the actual delete on the base table.
    -- Since this is an INSTEAD OF trigger, this DELETE will not recursively fire the trigger
    -- (unless RECURSIVE_TRIGGERS option is enabled, which is off by default).
    DELETE FROM Users WHERE UserID IN (SELECT UserID FROM #UsersToDelete);
    
    -- Cleanup Temp Tables
    DROP TABLE #UsersToDelete;
    DROP TABLE #CreatedTeams;
    DROP TABLE #TeamProjects;
    DROP TABLE #UserProjects;
    
    PRINT 'Cascade delete performed successfully via Trigger.';
END
GO
