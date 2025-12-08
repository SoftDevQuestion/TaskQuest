-- Migration Script: Apply changes from TodoAppDB to ToDo database
-- Generated based on schema comparison

USE ToDo;
GO

-- 1. Create Role table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Role')
BEGIN
    CREATE TABLE Role (
        RoleId INT IDENTITY(1,1) PRIMARY KEY,
        RoleName NVARCHAR(50) NOT NULL
    );
    
    SET IDENTITY_INSERT Role ON;
    INSERT INTO Role (RoleId, RoleName) VALUES (1, 'member');
    INSERT INTO Role (RoleId, RoleName) VALUES (2, 'admin');
    SET IDENTITY_INSERT Role OFF;

    PRINT 'Created table Role and inserted default data';
END
GO

-- 2. Create TaskStatus table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'TaskStatus')
BEGIN
    CREATE TABLE TaskStatus (
        StatusId INT IDENTITY(1,1) PRIMARY KEY,
        StatusName NVARCHAR(50) NOT NULL
    );

    SET IDENTITY_INSERT TaskStatus ON;
    INSERT INTO TaskStatus (StatusId, StatusName) VALUES (1, 'todo');
    INSERT INTO TaskStatus (StatusId, StatusName) VALUES (2, 'inprogress');
    INSERT INTO TaskStatus (StatusId, StatusName) VALUES (3, 'done');
    SET IDENTITY_INSERT TaskStatus OFF;

    PRINT 'Created table TaskStatus and inserted default data';
END
GO

-- 3. Create Team table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Team')
BEGIN
    CREATE TABLE Team (
        TeamId INT IDENTITY(1,1) PRIMARY KEY,
        TeamName NVARCHAR(100) NOT NULL,
        Description NVARCHAR(200) NULL,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE()
    );
    PRINT 'Created table Team';
END
GO

-- 4. Update Users table (Add RoleId and TeamId)
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'Users') AND name = 'RoleId')
BEGIN
    ALTER TABLE Users ADD RoleId INT NOT NULL DEFAULT 1; -- Default to 'member' (RoleId=1)
    PRINT 'Added RoleId to Users with default value 1';
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'Users') AND name = 'TeamId')
BEGIN
    ALTER TABLE Users ADD TeamId INT NULL;
    PRINT 'Added TeamId to Users';
END
GO

-- 5. Create Projects table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Projects')
BEGIN
    CREATE TABLE Projects (
        ProjectId INT IDENTITY(1,1) PRIMARY KEY,
        CreatorUserId INT NOT NULL,
        ProjectName NVARCHAR(200) NULL,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
        TeamAccessId INT NULL,
        DoneStatusCount INT NOT NULL DEFAULT 0,
        InProgressStatusCount INT NOT NULL DEFAULT 0,
        ToDoStatusCount INT NOT NULL DEFAULT 0,
        UndefinedStatusCount INT NOT NULL DEFAULT 0,
        FOREIGN KEY (CreatorUserId) REFERENCES Users(UserId),
        FOREIGN KEY (TeamAccessId) REFERENCES Team(TeamId)
    );
    PRINT 'Created table Projects';
END
GO

-- 6. Create Tasks table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Tasks')
BEGIN
    CREATE TABLE Tasks (
        TaskId INT IDENTITY(1,1) PRIMARY KEY,
        ProjectId INT NOT NULL,
        StatusId INT NOT NULL,
        Title NVARCHAR(200) NOT NULL,
        Description NVARCHAR(MAX) NULL,
        DueDate DATETIME2 NULL,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
        FOREIGN KEY (ProjectId) REFERENCES Projects(ProjectId),
        FOREIGN KEY (StatusId) REFERENCES TaskStatus(StatusId)
    );
    PRINT 'Created table Tasks';
END
GO

-- 7. Create UserProfile table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'UserProfile')
BEGIN
    CREATE TABLE UserProfile (
        UserProfileId INT IDENTITY(1,1) PRIMARY KEY,
        UserId INT NOT NULL,
        RoleId INT NOT NULL,
        Bio NVARCHAR(500) NULL,
        ProfileImageUrl NVARCHAR(300) NULL,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
        FOREIGN KEY (UserId) REFERENCES Users(UserId),
        FOREIGN KEY (RoleId) REFERENCES Role(RoleId)
    );
    PRINT 'Created table UserProfile';
END
GO

-- Add Foreign Key for Users.RoleId if it exists and Role table exists
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Role') AND EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'Users') AND name = 'RoleId')
BEGIN
    IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE parent_object_id = OBJECT_ID(N'Users') AND name = 'FK_Users_Role')
    BEGIN
        ALTER TABLE Users ADD CONSTRAINT FK_Users_Role FOREIGN KEY (RoleId) REFERENCES Role(RoleId);
        PRINT 'Added FK_Users_Role constraint';
    END
END
GO

-- Add Foreign Key for Users.TeamId if it exists and Team table exists
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Team') AND EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'Users') AND name = 'TeamId')
BEGIN
    IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE parent_object_id = OBJECT_ID(N'Users') AND name = 'FK_Users_Team')
    BEGIN
        ALTER TABLE Users ADD CONSTRAINT FK_Users_Team FOREIGN KEY (TeamId) REFERENCES Team(TeamId);
        PRINT 'Added FK_Users_Team constraint';
    END
END
GO

PRINT 'Migration completed successfully.';
