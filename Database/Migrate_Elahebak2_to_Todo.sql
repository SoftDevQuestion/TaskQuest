-- Script to Migrate Users, Projects, and Tasks from Elahebak2 to Todo
-- Run this script in SSMS. Ensure both databases are attached.

USE Todo;
GO

-- =============================================
-- 1. USERS TABLE
-- =============================================
PRINT 'Migrating Users...';

IF OBJECT_ID('dbo.Users', 'U') IS NULL
BEGIN
    -- Case 1: Table does not exist. Create it from source structure and data.
    SELECT * INTO dbo.Users FROM Elahebak2.dbo.Users;
    
    -- Restore Primary Key (Assuming UserID is PK)
    ALTER TABLE dbo.Users ADD CONSTRAINT PK_Users PRIMARY KEY (UserID);
    PRINT 'Users table created and data imported.';
END
ELSE
BEGIN
    -- Case 2: Table exists. Merge data.
    -- We use IDENTITY_INSERT to preserve IDs from source.
    SET IDENTITY_INSERT dbo.Users ON;
    
    INSERT INTO dbo.Users (UserID, Username, PasswordHash, Email, FullName, AvatarPath)
    SELECT UserID, Username, PasswordHash, Email, FullName, AvatarPath
    FROM Elahebak2.dbo.Users src
    WHERE NOT EXISTS (SELECT 1 FROM dbo.Users tgt WHERE tgt.UserID = src.UserID);
    
    SET IDENTITY_INSERT dbo.Users OFF;
    PRINT 'Users data merged (new records added).';
END
GO

-- =============================================
-- 2. PROJECTS TABLE
-- =============================================
PRINT 'Migrating Projects...';

IF OBJECT_ID('dbo.Projects', 'U') IS NULL
BEGIN
    SELECT * INTO dbo.Projects FROM Elahebak2.dbo.Projects;
    
    ALTER TABLE dbo.Projects ADD CONSTRAINT PK_Projects PRIMARY KEY (ProjectID);
    PRINT 'Projects table created and data imported.';
END
ELSE
BEGIN
    SET IDENTITY_INSERT dbo.Projects ON;
    
    -- Note: Column list must match exactly. If schemas differ, this might fail.
    -- Using common columns inferred from code.
    INSERT INTO dbo.Projects (ProjectID, ProjectName, Description, CreatorUserId, TeamAccessId, CreatedAt)
    SELECT ProjectID, ProjectName, Description, CreatorUserId, TeamAccessId, CreatedAt
    FROM Elahebak2.dbo.Projects src
    WHERE NOT EXISTS (SELECT 1 FROM dbo.Projects tgt WHERE tgt.ProjectID = src.ProjectID);
    
    SET IDENTITY_INSERT dbo.Projects OFF;
    PRINT 'Projects data merged.';
END
GO

-- =============================================
-- 3. TASKS TABLE
-- =============================================
PRINT 'Migrating Tasks...';

IF OBJECT_ID('dbo.Tasks', 'U') IS NULL
BEGIN
    SELECT * INTO dbo.Tasks FROM Elahebak2.dbo.Tasks;
    
    ALTER TABLE dbo.Tasks ADD CONSTRAINT PK_Tasks PRIMARY KEY (TaskID);
    PRINT 'Tasks table created and data imported.';
END
ELSE
BEGIN
    SET IDENTITY_INSERT dbo.Tasks ON;
    
    INSERT INTO dbo.Tasks (TaskID, ProjectID, UserID, Title, Description, StatusId, Priority, DueDate, CreatedAt)
    SELECT TaskID, ProjectID, UserID, Title, Description, StatusId, Priority, DueDate, CreatedAt
    FROM Elahebak2.dbo.Tasks src
    WHERE NOT EXISTS (SELECT 1 FROM dbo.Tasks tgt WHERE tgt.TaskID = src.TaskID);
    
    SET IDENTITY_INSERT dbo.Tasks OFF;
    PRINT 'Tasks data merged.';
END
GO

-- =============================================
-- 4. RESTORE FOREIGN KEYS (Optional/Recommended)
-- =============================================
-- Only run this if tables were newly created and you want to enforce integrity
-- IF OBJECT_ID('FK_Tasks_Projects', 'F') IS NULL
-- BEGIN
--     ALTER TABLE dbo.Tasks WITH CHECK ADD CONSTRAINT FK_Tasks_Projects FOREIGN KEY(ProjectID)
--     REFERENCES dbo.Projects (ProjectID);
-- END

PRINT 'Migration completed.';
