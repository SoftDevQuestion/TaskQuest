IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Projects]') AND name = 'ProjectLogo')
BEGIN
    ALTER TABLE [dbo].[Projects]
    ADD [ProjectLogo] NVARCHAR(255) NULL;
    PRINT 'Column ProjectLogo added.';
END
ELSE
BEGIN
    PRINT 'Column ProjectLogo already exists.';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Projects]') AND name = 'ProjectCover')
BEGIN
    ALTER TABLE [dbo].[Projects]
    ADD [ProjectCover] NVARCHAR(255) NULL;
    PRINT 'Column ProjectCover added.';
END
ELSE
BEGIN
    PRINT 'Column ProjectCover already exists.';
END
