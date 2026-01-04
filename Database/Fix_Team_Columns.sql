-- =============================================
-- ADD MISSING COLUMNS TO TEAM TABLE
-- Run this script in your Local SSMS (SQL Server Management Studio)
-- =============================================

USE [ToDo]; -- Local Database Name
GO

-- 1. Add LogoPath Column
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Team' AND COLUMN_NAME = 'LogoPath')
BEGIN
    ALTER TABLE Team ADD LogoPath NVARCHAR(255) NULL;
    PRINT 'Added LogoPath column to Team table.';
END
GO

-- 2. Add UpdatedAt Column
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Team' AND COLUMN_NAME = 'UpdatedAt')
BEGIN
    ALTER TABLE Team ADD UpdatedAt DATETIME DEFAULT GETDATE();
    PRINT 'Added UpdatedAt column to Team table.';
END
GO

-- 3. Backfill UpdatedAt for existing rows (if any are NULL)
UPDATE Team SET UpdatedAt = CreatedAt WHERE UpdatedAt IS NULL;
PRINT 'Backfilled UpdatedAt column.';
GO

PRINT 'Team table schema updated successfully.';
