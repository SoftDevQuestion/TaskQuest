
USE ToDo;
GO

DECLARE @sql NVARCHAR(MAX) = N'';

-- 1. Drop Foreign Keys referencing mdm tables or within mdm tables
SELECT @sql += N'ALTER TABLE ' + QUOTENAME(OBJECT_SCHEMA_NAME(parent_object_id))
    + '.' + QUOTENAME(OBJECT_NAME(parent_object_id)) + 
    ' DROP CONSTRAINT ' + QUOTENAME(name) + ';' + CHAR(13)
FROM sys.foreign_keys
WHERE OBJECT_SCHEMA_NAME(referenced_object_id) = 'mdm';

EXEC sp_executesql @sql;
SET @sql = N'';

-- 2. Drop Tables in mdm schema
SELECT @sql += N'DROP TABLE ' + QUOTENAME(TABLE_SCHEMA) + '.' + QUOTENAME(TABLE_NAME) + ';' + CHAR(13)
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_SCHEMA = 'mdm' AND TABLE_TYPE = 'BASE TABLE';

EXEC sp_executesql @sql;

-- 3. Drop mdm schema if empty
IF NOT EXISTS (SELECT * FROM sys.objects WHERE schema_id = SCHEMA_ID('mdm'))
BEGIN
    DROP SCHEMA mdm;
END
GO
