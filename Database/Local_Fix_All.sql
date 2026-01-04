-- =============================================
-- MASTER FIX SCRIPT FOR LOCAL DATABASE
-- Run this script in your Local SSMS (SQL Server Management Studio)
-- This applies all necessary schema changes, triggers, and fixes
-- to enable Cascade Delete and fix User deletion errors.
-- =============================================

USE [ToDo]; -- Or whatever your local database name is (e.g., db_ac2fe4_todo)
GO

-- 1. Add CreatorUsername Column to Team Table (if missing)
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Team' AND COLUMN_NAME = 'CreatorUsername')
BEGIN
    ALTER TABLE Team ADD CreatorUsername NVARCHAR(50) NULL;
    PRINT 'Added CreatorUsername column to Team table.';
END
GO

-- 2. Backfill Existing Teams (Default to 'admin' or system)
UPDATE Team SET CreatorUsername = 'admin' WHERE CreatorUsername IS NULL;
PRINT 'Backfilled CreatorUsername with default value.';
GO

-- 3. Fix Trigger Deadlock (Allow Cascade Updates)
-- This ensures that deleting tasks doesn't block project updates
IF OBJECT_ID('trg_Block_Project_Status_Manual_Update', 'TR') IS NOT NULL
BEGIN
    EXEC('
    ALTER TRIGGER trg_Block_Project_Status_Manual_Update
    ON Projects
    AFTER UPDATE
    AS
    BEGIN
        -- Allow updates initiated by other triggers (Cascade Update)
        IF TRIGGER_NESTLEVEL(OBJECT_ID(''trg_Sync_Project_With_Tasks'')) > 0
            RETURN;

        IF UPDATE(DoneStatusCount)
           OR UPDATE(InProgressStatusCount)
           OR UPDATE(ToDoStatusCount)
           OR UPDATE(UndefinedStatusCount)
        BEGIN
            RAISERROR (
                N''Updates to status counts are only allowed via Task changes.'',
                16, 1
            );
            ROLLBACK TRANSACTION;
        END
    END;
    ');
    PRINT 'Fixed trg_Block_Project_Status_Manual_Update trigger.';
END
GO

-- 4. Create Seamless Cascade Delete Trigger for Users
-- This allows "DELETE FROM Users WHERE ..." to work without FK errors
IF OBJECT_ID('trg_Users_Cascade_Delete_Seamless', 'TR') IS NOT NULL
    DROP TRIGGER trg_Users_Cascade_Delete_Seamless;
GO

CREATE TRIGGER trg_Users_Cascade_Delete_Seamless
ON Users
INSTEAD OF DELETE
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Capture deleted users
    SELECT UserID, Username INTO #UsersToDelete FROM deleted;
    
    -- A. Delete UserProfile (Direct dependency)
    DELETE FROM UserProfile WHERE UserId IN (SELECT UserID FROM #UsersToDelete);
    
    -- B. Handle Team Ownership
    -- 1. Find Teams created by these users
    SELECT TeamId INTO #CreatedTeams FROM Team WHERE CreatorUsername IN (SELECT Username FROM #UsersToDelete);
    
    -- 2. Find Projects in these Teams
    SELECT ProjectId INTO #TeamProjects FROM Projects WHERE TeamAccessId IN (SELECT TeamId FROM #CreatedTeams);
    
    -- 3. Delete Tasks in these Projects
    DELETE FROM Tasks WHERE ProjectId IN (SELECT ProjectId FROM #TeamProjects);
    
    -- 4. Delete ProjectTeams entries & Projects
    DELETE FROM ProjectTeams WHERE ProjectID IN (SELECT ProjectId FROM #TeamProjects);
    DELETE FROM Projects WHERE ProjectId IN (SELECT ProjectId FROM #TeamProjects);
    
    -- 5. Delete TeamMembers & ProjectTeams for the Teams
    DELETE FROM TeamMembers WHERE TeamId IN (SELECT TeamId FROM #CreatedTeams);
    DELETE FROM ProjectTeams WHERE TeamID IN (SELECT TeamId FROM #CreatedTeams);
    DELETE FROM Team WHERE TeamId IN (SELECT TeamId FROM #CreatedTeams);
    
    -- C. Cleanup User Memberships in other teams
    DELETE FROM TeamMembers WHERE Username IN (SELECT Username FROM #UsersToDelete);
    
    -- D. Cleanup Orphaned Projects (Created by User directly)
    SELECT ProjectId INTO #UserProjects FROM Projects WHERE CreatorUserId IN (SELECT UserID FROM #UsersToDelete);
    
    -- E. Delete Tasks & Projects for Orphaned Projects
    DELETE FROM Tasks WHERE ProjectId IN (SELECT ProjectId FROM #UserProjects);
    DELETE FROM ProjectTeams WHERE ProjectID IN (SELECT ProjectId FROM #UserProjects);
    DELETE FROM Projects WHERE ProjectId IN (SELECT ProjectId FROM #UserProjects);
    
    -- F. Finally Delete the Users
    DELETE FROM Users WHERE UserID IN (SELECT UserID FROM #UsersToDelete);
    
    -- Cleanup Temp Tables
    DROP TABLE #UsersToDelete;
    DROP TABLE #CreatedTeams;
    DROP TABLE #TeamProjects;
    DROP TABLE #UserProjects;
    
    PRINT 'Cascade delete performed successfully via Trigger.';
END
GO

PRINT 'ALL LOCAL FIXES APPLIED SUCCESSFULLY.';
