-- Script to Sync Elahebak2 Structure and Data to Match Todo
-- Reference (Source): Todo
-- Target: Elahebak2
-- This script will make Elahebak2 look like Todo (Structure + Data Merge)

USE Elahebak2;
GO

-- =============================================
-- 1. SYNC STRUCTURE (Schema)
-- =============================================
PRINT 'Syncing Schema...';

-- A. USERS Table Structure
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Users' AND COLUMN_NAME = 'AvatarPath')
BEGIN
    ALTER TABLE Users ADD AvatarPath NVARCHAR(255) NULL;
    PRINT 'Added AvatarPath to Users.';
END
GO

-- B. TASKS Table Structure
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Tasks' AND COLUMN_NAME = 'UserID')
BEGIN
    ALTER TABLE Tasks ADD UserID INT NULL;
    -- Note: We add FK later to avoid errors if Users don't exist yet
    PRINT 'Added UserID to Tasks.';
END

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Tasks' AND COLUMN_NAME = 'Priority')
BEGIN
    ALTER TABLE Tasks ADD Priority INT DEFAULT 0;
    PRINT 'Added Priority to Tasks.';
END
GO

-- C. TEAM Table Structure (If exists in Todo but missing/different in Elahe)
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Team')
BEGIN
    -- Basic Team Structure assumed from Todo
    CREATE TABLE Team (
        TeamId INT IDENTITY(1,1) PRIMARY KEY,
        TeamName NVARCHAR(100),
        CreatorUsername NVARCHAR(50),
        UpdatedAt DATETIME DEFAULT GETDATE()
    );
    PRINT 'Created Team table.';
END
ELSE
BEGIN
    IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Team' AND COLUMN_NAME = 'CreatorUsername')
    BEGIN
        ALTER TABLE Team ADD CreatorUsername NVARCHAR(50) NULL;
        PRINT 'Added CreatorUsername to Team.';
    end
END
GO

-- D. ProjectTeams Table
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'ProjectTeams')
BEGIN
    CREATE TABLE ProjectTeams (
        ProjectTeamID INT IDENTITY(1,1) PRIMARY KEY,
        ProjectID INT NOT NULL,
        TeamID INT NOT NULL
    );
    PRINT 'Created ProjectTeams table.';
END
GO

-- E. TeamMembers Table
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'TeamMembers')
BEGIN
    CREATE TABLE TeamMembers (
        ID INT IDENTITY(1,1) PRIMARY KEY,
        TeamId INT NOT NULL,
        Username NVARCHAR(50) NOT NULL,
        Role NVARCHAR(50) DEFAULT 'Member',
        JoinedAt DATETIME DEFAULT GETDATE()
    );
    PRINT 'Created TeamMembers table.';
END
GO

-- =============================================
-- 2. SYNC DATA (Merge from Todo -> Elahebak2)
-- =============================================
PRINT 'Syncing Data from Todo to Elahebak2...';

-- A. Sync Users
-- We insert users from Todo that don't exist in Elahebak2
SET IDENTITY_INSERT dbo.Users ON;
INSERT INTO dbo.Users (UserID, Username, PasswordHash, Email, FullName, AvatarPath)
SELECT UserID, Username, PasswordHash, Email, FullName, AvatarPath
FROM Todo.dbo.Users src
WHERE NOT EXISTS (SELECT 1 FROM dbo.Users tgt WHERE tgt.UserID = src.UserID);
SET IDENTITY_INSERT dbo.Users OFF;
PRINT 'Users synced.';
GO

-- B. Sync Teams
SET IDENTITY_INSERT dbo.Team ON;
INSERT INTO dbo.Team (TeamId, TeamName, CreatorUsername, UpdatedAt)
SELECT TeamId, TeamName, CreatorUsername, UpdatedAt
FROM Todo.dbo.Team src
WHERE NOT EXISTS (SELECT 1 FROM dbo.Team tgt WHERE tgt.TeamId = src.TeamId);
SET IDENTITY_INSERT dbo.Team OFF;
PRINT 'Teams synced.';
GO

-- C. Sync TeamMembers
-- Assuming ID is PK
SET IDENTITY_INSERT dbo.TeamMembers ON;
INSERT INTO dbo.TeamMembers (ID, TeamId, Username, Role, JoinedAt)
SELECT ID, TeamId, Username, Role, JoinedAt
FROM Todo.dbo.TeamMembers src
WHERE NOT EXISTS (SELECT 1 FROM dbo.TeamMembers tgt WHERE tgt.ID = src.ID);
SET IDENTITY_INSERT dbo.TeamMembers OFF;
PRINT 'TeamMembers synced.';
GO

-- D. Sync Projects
SET IDENTITY_INSERT dbo.Projects ON;
INSERT INTO dbo.Projects (ProjectID, ProjectName, Description, CreatorUserId, TeamAccessId, CreatedAt)
SELECT ProjectID, ProjectName, Description, CreatorUserId, TeamAccessId, CreatedAt
FROM Todo.dbo.Projects src
WHERE NOT EXISTS (SELECT 1 FROM dbo.Projects tgt WHERE tgt.ProjectID = src.ProjectID);
SET IDENTITY_INSERT dbo.Projects OFF;
PRINT 'Projects synced.';
GO

-- E. Sync ProjectTeams
SET IDENTITY_INSERT dbo.ProjectTeams ON;
INSERT INTO dbo.ProjectTeams (ProjectTeamID, ProjectID, TeamID)
SELECT ProjectTeamID, ProjectID, TeamID
FROM Todo.dbo.ProjectTeams src
WHERE NOT EXISTS (SELECT 1 FROM dbo.ProjectTeams tgt WHERE tgt.ProjectTeamID = src.ProjectTeamID);
SET IDENTITY_INSERT dbo.ProjectTeams OFF;
PRINT 'ProjectTeams synced.';
GO

-- F. Sync Tasks
SET IDENTITY_INSERT dbo.Tasks ON;
INSERT INTO dbo.Tasks (TaskID, ProjectID, UserID, Title, Description, StatusId, Priority, DueDate, CreatedAt)
SELECT TaskID, ProjectID, UserID, Title, Description, StatusId, Priority, DueDate, CreatedAt
FROM Todo.dbo.Tasks src
WHERE NOT EXISTS (SELECT 1 FROM dbo.Tasks tgt WHERE tgt.TaskID = src.TaskID);
SET IDENTITY_INSERT dbo.Tasks OFF;
PRINT 'Tasks synced.';
GO

-- =============================================
-- 3. APPLY CONSTRAINTS (Foreign Keys)
-- =============================================
PRINT 'Applying Constraints...';

-- UserID FK in Tasks
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Tasks_Users')
BEGIN
    -- Check if constraint can be applied (all UserIDs must exist)
    -- If invalid data exists, update it to NULL first
    UPDATE Tasks SET UserID = NULL WHERE UserID IS NOT NULL AND UserID NOT IN (SELECT UserID FROM Users);

    ALTER TABLE Tasks ADD CONSTRAINT FK_Tasks_Users FOREIGN KEY (UserID) REFERENCES Users(UserID);
    PRINT 'FK_Tasks_Users applied.';
END

-- ProjectID FK in Tasks
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Tasks_Projects')
BEGIN
    UPDATE Tasks SET ProjectID = NULL WHERE ProjectID IS NOT NULL AND ProjectID NOT IN (SELECT ProjectID FROM Projects);
    -- If ProjectID is NOT NULL column, we might have issues. Assuming nullable or valid data.
    
    ALTER TABLE Tasks ADD CONSTRAINT FK_Tasks_Projects FOREIGN KEY (ProjectID) REFERENCES Projects(ProjectID) ON DELETE CASCADE;
    PRINT 'FK_Tasks_Projects applied.';
END

PRINT 'Sync Completed Successfully.';
