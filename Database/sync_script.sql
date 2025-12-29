/*
    Sync Script for TaskQuest
    Source: DB_Yasi (from yasi.bak)
    Target: DB_Mahsa (from mahsa.bak)
    
    This script performs the following:
    1.  Verifies the existence of DB_Yasi and DB_Mahsa.
    2.  Synchronizes the schema of 'Team', 'UserProfile', and 'Project' tables from Source to Target.
    3.  Synchronizes data, handling deduplication and foreign keys.
    4.  Provides detailed reporting.
*/

USE master;
GO

IF DB_ID('DB_Yasi') IS NULL OR DB_ID('DB_Mahsa') IS NULL
BEGIN
    PRINT 'ERROR: Databases DB_Yasi and DB_Mahsa must be restored first.';
    PRINT 'Please restore yasi.bak as DB_Yasi and mahsa.bak as DB_Mahsa.';
    SET NOEXEC ON; -- Stop execution
END
GO

USE DB_Mahsa;
GO

SET NOCOUNT ON;
SET XACT_ABORT ON;

BEGIN TRY
    BEGIN TRANSACTION;

    PRINT 'Starting Synchronization Process...';

    -- =============================================
    -- 1. Schema Synchronization
    -- =============================================
    PRINT 'Checking Schema Differences...';

    DECLARE @TableName NVARCHAR(128);
    DECLARE @ColumnName NVARCHAR(128);
    DECLARE @DataType NVARCHAR(128);
    DECLARE @MaxLength INT;
    DECLARE @SQL NVARCHAR(MAX);

    DECLARE TableCursor CURSOR FOR
    SELECT 'Team' UNION SELECT 'UserProfile' UNION SELECT 'Project';

    OPEN TableCursor;
    FETCH NEXT FROM TableCursor INTO @TableName;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        PRINT '  Processing Schema for Table: ' + @TableName;

        -- Check for missing columns in Target (DB_Mahsa) that exist in Source (DB_Yasi)
        DECLARE ColumnCursor CURSOR FOR
        SELECT C.COLUMN_NAME, C.DATA_TYPE, C.CHARACTER_MAXIMUM_LENGTH
        FROM DB_Yasi.INFORMATION_SCHEMA.COLUMNS C
        WHERE C.TABLE_NAME = @TableName
        AND NOT EXISTS (
            SELECT 1 FROM DB_Mahsa.INFORMATION_SCHEMA.COLUMNS C2
            WHERE C2.TABLE_NAME = @TableName AND C2.COLUMN_NAME = C.COLUMN_NAME
        );

        OPEN ColumnCursor;
        FETCH NEXT FROM ColumnCursor INTO @ColumnName, @DataType, @MaxLength;

        WHILE @@FETCH_STATUS = 0
        BEGIN
            SET @SQL = 'ALTER TABLE ' + QUOTENAME(@TableName) + ' ADD ' + QUOTENAME(@ColumnName) + ' ' + @DataType;
            IF @MaxLength IS NOT NULL AND @MaxLength != -1
                SET @SQL = @SQL + '(' + CAST(@MaxLength AS NVARCHAR) + ')';
            ELSE IF @MaxLength = -1
                SET @SQL = @SQL + '(MAX)';
            
            SET @SQL = @SQL + ' NULL;'; -- Default to NULL for new columns
            
            PRINT '    Adding missing column: ' + @ColumnName;
            EXEC sp_executesql @SQL;

            FETCH NEXT FROM ColumnCursor INTO @ColumnName, @DataType, @MaxLength;
        END

        CLOSE ColumnCursor;
        DEALLOCATE ColumnCursor;

        FETCH NEXT FROM TableCursor INTO @TableName;
    END

    CLOSE TableCursor;
    DEALLOCATE TableCursor;

    PRINT 'Schema Synchronization Completed.';

    -- =============================================
    -- 2. Data Synchronization
    -- =============================================
    PRINT 'Starting Data Synchronization...';

    -- Disable Constraints
    ALTER TABLE DB_Mahsa.dbo.Project NOCHECK CONSTRAINT ALL;
    ALTER TABLE DB_Mahsa.dbo.UserProfile NOCHECK CONSTRAINT ALL;
    ALTER TABLE DB_Mahsa.dbo.Team NOCHECK CONSTRAINT ALL;

    -- A. Sync Teams
    PRINT '  Syncing Teams...';
    -- Update existing
    UPDATE T
    SET T.Name = S.Name, T.Description = S.Description, T.CreatedAt = S.CreatedAt -- Add other columns as needed
    FROM DB_Mahsa.dbo.Team T
    INNER JOIN DB_Yasi.dbo.Team S ON T.Id = S.Id;

    -- Insert new
    SET IDENTITY_INSERT DB_Mahsa.dbo.Team ON;
    INSERT INTO DB_Mahsa.dbo.Team (Id, Name, Description, CreatedAt) -- Adjust columns
    SELECT Id, Name, Description, CreatedAt
    FROM DB_Yasi.dbo.Team S
    WHERE NOT EXISTS (SELECT 1 FROM DB_Mahsa.dbo.Team T WHERE T.Id = S.Id);
    SET IDENTITY_INSERT DB_Mahsa.dbo.Team OFF;

    -- B. Sync UserProfiles
    PRINT '  Syncing UserProfiles...';
    -- Update existing
    UPDATE T
    SET T.FullName = S.FullName, T.Bio = S.Bio, T.AvatarUrl = S.AvatarUrl
    FROM DB_Mahsa.dbo.UserProfile T
    INNER JOIN DB_Yasi.dbo.UserProfile S ON T.UserId = S.UserId;

    -- Insert new
    -- Note: UserProfile usually links to Users table. Ensure Users are synced too if needed.
    -- Assuming direct sync for now.
    INSERT INTO DB_Mahsa.dbo.UserProfile (UserId, FullName, Bio, AvatarUrl)
    SELECT UserId, FullName, Bio, AvatarUrl
    FROM DB_Yasi.dbo.UserProfile S
    WHERE NOT EXISTS (SELECT 1 FROM DB_Mahsa.dbo.UserProfile T WHERE T.UserId = S.UserId);

    -- C. Sync Projects
    PRINT '  Syncing Projects...';
    -- Update existing
    UPDATE T
    SET T.Name = S.Name, T.Description = S.Description, T.TeamId = S.TeamId, T.Status = S.Status
    FROM DB_Mahsa.dbo.Project T
    INNER JOIN DB_Yasi.dbo.Project S ON T.Id = S.Id;

    -- Insert new
    SET IDENTITY_INSERT DB_Mahsa.dbo.Project ON;
    INSERT INTO DB_Mahsa.dbo.Project (Id, Name, Description, TeamId, Status, CreatedAt)
    SELECT Id, Name, Description, TeamId, Status, CreatedAt
    FROM DB_Yasi.dbo.Project S
    WHERE NOT EXISTS (SELECT 1 FROM DB_Mahsa.dbo.Project T WHERE T.Id = S.Id);
    SET IDENTITY_INSERT DB_Mahsa.dbo.Project OFF;

    -- Re-enable Constraints
    ALTER TABLE DB_Mahsa.dbo.Team CHECK CONSTRAINT ALL;
    ALTER TABLE DB_Mahsa.dbo.UserProfile CHECK CONSTRAINT ALL;
    ALTER TABLE DB_Mahsa.dbo.Project CHECK CONSTRAINT ALL;

    PRINT 'Data Synchronization Completed Successfully.';

    COMMIT TRANSACTION;
    PRINT 'Transaction Committed.';
END TRY
BEGIN CATCH
    ROLLBACK TRANSACTION;
    PRINT 'Error occurred: ' + ERROR_MESSAGE();
    PRINT 'Transaction Rolled Back.';
END CATCH
GO
SET NOEXEC OFF;
