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
    -- Create empty table with structure from source (if possible) or define it manually
    -- Manual definition is safer to ensure we have what we need.
    CREATE TABLE dbo.Users (
        UserID INT IDENTITY(1,1) PRIMARY KEY,
        Username NVARCHAR(50),
        PasswordHash NVARCHAR(255),
        Email NVARCHAR(100),
        FullName NVARCHAR(100),
        AvatarPath NVARCHAR(255)
    );
    PRINT 'Users table created.';
END

-- Migrate Data (Safe Insert)
SET IDENTITY_INSERT dbo.Users ON;
BEGIN TRY
    INSERT INTO dbo.Users (UserID, Username, PasswordHash, Email, FullName, AvatarPath)
    SELECT 
        UserID, 
        Username, 
        PasswordHash, 
        Email, 
        FullName, 
        AvatarPath
    FROM Elahebak2.dbo.Users src
    WHERE NOT EXISTS (SELECT 1 FROM dbo.Users tgt WHERE tgt.UserID = src.UserID);
    PRINT 'Users data merged.';
END TRY
BEGIN CATCH
    PRINT 'Error migrating Users (Column mismatch?): ' + ERROR_MESSAGE();
END CATCH
SET IDENTITY_INSERT dbo.Users OFF;
GO

-- =============================================
-- 2. PROJECTS TABLE
-- =============================================
PRINT 'Migrating Projects...';

IF OBJECT_ID('dbo.Projects', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Projects (
        ProjectID INT IDENTITY(1,1) PRIMARY KEY,
        ProjectName NVARCHAR(100),
        Description NVARCHAR(MAX),
        CreatorUserId INT,
        TeamAccessId INT,
        CreatedAt DATETIME DEFAULT GETDATE()
    );
    PRINT 'Projects table created.';
END

SET IDENTITY_INSERT dbo.Projects ON;
BEGIN TRY
    INSERT INTO dbo.Projects (ProjectID, ProjectName, Description, CreatorUserId, TeamAccessId, CreatedAt)
    SELECT 
        ProjectID, 
        ProjectName, 
        Description, 
        CreatorUserId, 
        TeamAccessId, 
        CreatedAt
    FROM Elahebak2.dbo.Projects src
    WHERE NOT EXISTS (SELECT 1 FROM dbo.Projects tgt WHERE tgt.ProjectID = src.ProjectID);
    PRINT 'Projects data merged.';
END TRY
BEGIN CATCH
    PRINT 'Error migrating Projects: ' + ERROR_MESSAGE();
END CATCH
SET IDENTITY_INSERT dbo.Projects OFF;
GO

-- =============================================
-- 3. TASKS TABLE (Dynamic Migration)
-- =============================================
PRINT 'Migrating Tasks...';

-- A. Ensure Target Table Exists
IF OBJECT_ID('dbo.Tasks', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Tasks (
        TaskID INT IDENTITY(1,1) PRIMARY KEY,
        ProjectID INT,
        UserID INT NULL,
        Title NVARCHAR(200),
        Description NVARCHAR(MAX),
        StatusId INT,
        Priority INT DEFAULT 0,
        DueDate DATETIME,
        CreatedAt DATETIME DEFAULT GETDATE()
    );
    PRINT 'Tasks table created.';
END

-- B. Ensure Columns Exist in Target
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Tasks' AND COLUMN_NAME = 'UserID')
BEGIN
    ALTER TABLE dbo.Tasks ADD UserID INT NULL;
    PRINT 'Added UserID to Tasks.';
END
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Tasks' AND COLUMN_NAME = 'Priority')
BEGIN
    ALTER TABLE dbo.Tasks ADD Priority INT DEFAULT 0;
    PRINT 'Added Priority to Tasks.';
END

-- C. Dynamic Insert to handle missing columns in Source
DECLARE @Sql NVARCHAR(MAX);
DECLARE @HasUserID BIT = 0;
DECLARE @HasPriority BIT = 0;

-- Check Source Columns
IF EXISTS (SELECT * FROM Elahebak2.INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Tasks' AND COLUMN_NAME = 'UserID')
    SET @HasUserID = 1;

IF EXISTS (SELECT * FROM Elahebak2.INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Tasks' AND COLUMN_NAME = 'Priority')
    SET @HasPriority = 1;

-- Build SQL
SET @Sql = '
    SET IDENTITY_INSERT dbo.Tasks ON;
    
    INSERT INTO dbo.Tasks (TaskID, ProjectID, Title, Description, StatusId, DueDate, CreatedAt, UserID, Priority)
    SELECT 
        src.TaskID, 
        src.ProjectID, 
        src.Title, 
        src.Description, 
        src.StatusId, 
        src.DueDate, 
        src.CreatedAt,
        ' + CASE WHEN @HasUserID = 1 THEN 'src.UserID' ELSE 'NULL' END + ',
        ' + CASE WHEN @HasPriority = 1 THEN 'src.Priority' ELSE '0' END + '
    FROM Elahebak2.dbo.Tasks src
    WHERE NOT EXISTS (SELECT 1 FROM dbo.Tasks tgt WHERE tgt.TaskID = src.TaskID);
    
    SET IDENTITY_INSERT dbo.Tasks OFF;
';

PRINT 'Executing Dynamic Tasks Migration...';
EXEC sp_executesql @Sql;
PRINT 'Tasks data merged.';
GO

PRINT 'Migration script finished.';
