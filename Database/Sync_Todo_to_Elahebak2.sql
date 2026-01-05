-- Script to Sync Elahebak2 Structure and Data to Match TodoAppDB
USE Elahebak2;
GO

-- =============================================
-- 1. SYNC STRUCTURE (Schema)
-- =============================================
PRINT 'Syncing Schema...';

-- A. USERS Table Structure
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Users')
BEGIN
    CREATE TABLE Users (
        UserID INT IDENTITY(1,1) PRIMARY KEY,
        Username NVARCHAR(50),
        PasswordHash NVARCHAR(255),
        Email NVARCHAR(100),
        FullName NVARCHAR(100),
        AvatarPath NVARCHAR(255) NULL,
        CreatedAt DATETIME DEFAULT GETDATE()
    );
    PRINT 'Created Users table.';
END
ELSE
BEGIN
    IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Users' AND COLUMN_NAME = 'AvatarPath')
    BEGIN
        ALTER TABLE Users ADD AvatarPath NVARCHAR(255) NULL;
        PRINT 'Added AvatarPath to Users.';
    END
END
GO

-- B. TASKS Table Structure
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Tasks')
BEGIN
    CREATE TABLE Tasks (
        TaskID INT IDENTITY(1,1) PRIMARY KEY,
        ProjectID INT NULL,
        UserID INT NULL,
        Title NVARCHAR(200),
        Description NVARCHAR(MAX),
        StatusId INT DEFAULT 0,
        Priority INT DEFAULT 0,
        DueDate DATETIME2 NULL,
        CreatedAt DATETIME2 DEFAULT GETDATE()
    );
    PRINT 'Created Tasks table.';
END
ELSE
BEGIN
    IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Tasks' AND COLUMN_NAME = 'UserID')
    BEGIN
        ALTER TABLE Tasks ADD UserID INT NULL;
        PRINT 'Added UserID to Tasks.';
    END
    IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Tasks' AND COLUMN_NAME = 'Priority')
    BEGIN
        ALTER TABLE Tasks ADD Priority INT DEFAULT 0;
        PRINT 'Added Priority to Tasks.';
    END
END
GO

-- C. TEAM Table Structure
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Team')
BEGIN
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
    END
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

-- F. Projects Table Structure
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Projects')
BEGIN
    CREATE TABLE Projects (
        ProjectID INT IDENTITY(1,1) PRIMARY KEY,
        ProjectName NVARCHAR(100),
        Description NVARCHAR(MAX),
        CreatorUserId INT,
        TeamAccessId INT,
        CreatedAt DATETIME2 DEFAULT GETDATE(),
        UpdatedAt DATETIME DEFAULT GETDATE(),
        ProjectLogo NVARCHAR(255),
        ProjectCover NVARCHAR(255),
        DoneStatusCount INT DEFAULT 0,
        InProgressStatusCount INT DEFAULT 0,
        ToDoStatusCount INT DEFAULT 0,
        UndefinedStatusCount INT DEFAULT 0
    );
    PRINT 'Created Projects table.';
END
GO

-- =============================================
-- 2. SYNC DATA (Merge from TodoAppDB -> Elahebak2)
-- =============================================
PRINT 'Syncing Data from TodoAppDB to Elahebak2...';

-- A. Sync Users
SET IDENTITY_INSERT dbo.Users ON;
INSERT INTO dbo.Users (UserID, Username, PasswordHash, Email, FullName, AvatarPath)
SELECT UserID, Username, PasswordHash, Email, FullName, AvatarPath
FROM TodoAppDB.dbo.Users src
WHERE NOT EXISTS (SELECT 1 FROM dbo.Users tgt WHERE tgt.UserID = src.UserID);
SET IDENTITY_INSERT dbo.Users OFF;
PRINT 'Users synced.';
GO

-- B. Sync Teams
SET IDENTITY_INSERT dbo.Team ON;
INSERT INTO dbo.Team (TeamId, TeamName, CreatorUsername, UpdatedAt)
SELECT TeamId, TeamName, CreatorUsername, UpdatedAt
FROM TodoAppDB.dbo.Team src
WHERE NOT EXISTS (SELECT 1 FROM dbo.Team tgt WHERE tgt.TeamId = src.TeamId);
SET IDENTITY_INSERT dbo.Team OFF;
PRINT 'Teams synced.';
GO

-- C. Sync TeamMembers
-- Note: Source does NOT have JoinedAt, so we skip it (default GETDATE() will be used)
SET IDENTITY_INSERT dbo.TeamMembers ON;
INSERT INTO dbo.TeamMembers (ID, TeamId, Username, Role)
SELECT ID, TeamId, Username, Role
FROM TodoAppDB.dbo.TeamMembers src
WHERE NOT EXISTS (SELECT 1 FROM dbo.TeamMembers tgt WHERE tgt.ID = src.ID);
SET IDENTITY_INSERT dbo.TeamMembers OFF;
PRINT 'TeamMembers synced.';
GO

-- D. Sync Projects
SET IDENTITY_INSERT dbo.Projects ON;
INSERT INTO dbo.Projects (ProjectID, ProjectName, Description, CreatorUserId, TeamAccessId, CreatedAt)
SELECT ProjectID, ProjectName, Description, CreatorUserId, TeamAccessId, CreatedAt
FROM TodoAppDB.dbo.Projects src
WHERE NOT EXISTS (SELECT 1 FROM dbo.Projects tgt WHERE tgt.ProjectID = src.ProjectID);
SET IDENTITY_INSERT dbo.Projects OFF;
PRINT 'Projects synced.';
GO

-- E. Sync ProjectTeams
SET IDENTITY_INSERT dbo.ProjectTeams ON;
INSERT INTO dbo.ProjectTeams (ProjectTeamID, ProjectID, TeamID)
SELECT ProjectTeamID, ProjectID, TeamID
FROM TodoAppDB.dbo.ProjectTeams src
WHERE NOT EXISTS (SELECT 1 FROM dbo.ProjectTeams tgt WHERE tgt.ProjectTeamID = src.ProjectTeamID);
SET IDENTITY_INSERT dbo.ProjectTeams OFF;
PRINT 'ProjectTeams synced.';
GO

-- F. Sync Tasks
SET IDENTITY_INSERT dbo.Tasks ON;
-- Priority is not in source, so we omit it from INSERT (defaults to 0)
INSERT INTO dbo.Tasks (TaskID, ProjectID, UserID, Title, Description, StatusId, DueDate, CreatedAt)
SELECT TaskID, ProjectID, UserID, Title, Description, StatusId, DueDate, CreatedAt
FROM TodoAppDB.dbo.Tasks src
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
    UPDATE Tasks SET UserID = NULL WHERE UserID IS NOT NULL AND UserID NOT IN (SELECT UserID FROM Users);
    ALTER TABLE Tasks ADD CONSTRAINT FK_Tasks_Users FOREIGN KEY (UserID) REFERENCES Users(UserID);
    PRINT 'FK_Tasks_Users applied.';
END

-- ProjectID FK in Tasks
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Tasks_Projects')
BEGIN
    UPDATE Tasks SET ProjectID = NULL WHERE ProjectID IS NOT NULL AND ProjectID NOT IN (SELECT ProjectID FROM Projects);
    ALTER TABLE Tasks ADD CONSTRAINT FK_Tasks_Projects FOREIGN KEY (ProjectID) REFERENCES Projects(ProjectID) ON DELETE CASCADE;
    PRINT 'FK_Tasks_Projects applied.';
END

PRINT 'Sync Completed Successfully.';
