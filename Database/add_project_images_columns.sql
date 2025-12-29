IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Projects]') AND name = 'ProjectLogo')
BEGIN
    ALTER TABLE [dbo].[Projects]
    ADD [ProjectLogo] NVARCHAR(255) NULL;
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Projects]') AND name = 'ProjectCover')
BEGIN
    ALTER TABLE [dbo].[Projects]
    ADD [ProjectCover] NVARCHAR(255) NULL;
END
GO
