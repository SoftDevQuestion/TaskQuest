-- Fix_Delete_With_Trigger.sql
-- 1. Ensure ProjectTeams table exists (Missing table caused error 208)
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'ProjectTeams')
BEGIN
    CREATE TABLE ProjectTeams (
        ProjectTeamID INT IDENTITY(1,1) PRIMARY KEY,
        ProjectID INT NOT NULL,
        TeamID INT NOT NULL,
        FOREIGN KEY (ProjectID) REFERENCES Projects(ProjectID) ON DELETE CASCADE,
        FOREIGN KEY (TeamID) REFERENCES Team(TeamId) ON DELETE CASCADE
    );
    PRINT 'Created missing table: ProjectTeams';
END
GO

-- 2. Ensure TeamMembers table exists (Missing table caused error 208)
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'TeamMembers')
BEGIN
    CREATE TABLE TeamMembers (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        TeamId INT NOT NULL,
        Username NVARCHAR(50) NOT NULL,
        Role NVARCHAR(50) DEFAULT 'member',
        FOREIGN KEY (TeamId) REFERENCES Team(TeamId) ON DELETE CASCADE
    );
    PRINT 'Created missing table: TeamMembers';
END
GO

-- 3. Create the Trigger to handle Cascade Delete safely
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
    
    DELETE FROM Tasks WHERE ProjectId IN (SELECT ProjectId FROM #UserProjects);
    DELETE FROM ProjectTeams WHERE ProjectID IN (SELECT ProjectId FROM #UserProjects);
    DELETE FROM Projects WHERE ProjectId IN (SELECT ProjectId FROM #UserProjects);
    
    -- E. Finally Delete the Users
    DELETE FROM Users WHERE UserID IN (SELECT UserID FROM #UsersToDelete);
    
    -- Cleanup
    DROP TABLE #UsersToDelete;
    DROP TABLE #CreatedTeams;
    DROP TABLE #TeamProjects;
    DROP TABLE #UserProjects;
    
    PRINT 'User delete handled successfully via Trigger.';
END
GO
