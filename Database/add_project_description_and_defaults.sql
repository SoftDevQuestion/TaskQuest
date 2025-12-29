IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Projects]') AND name = 'Description')
BEGIN
    ALTER TABLE [dbo].[Projects]
    ADD [Description] NVARCHAR(MAX) NULL DEFAULT 'a brief of how i wanna change the world!';
END
GO

-- Insert default projects if they don't exist
-- Assuming UserID 1 exists. If not, this might fail or need a check. 
-- Getting the first UserID just in case.
DECLARE @CreatorUserId INT;
SELECT TOP 1 @CreatorUserId = UserID FROM [dbo].[Users];

IF @CreatorUserId IS NOT NULL
BEGIN
    IF NOT EXISTS (SELECT 1 FROM [dbo].[Projects] WHERE ProjectName = 'Physics' AND CreatorUserId = @CreatorUserId)
    BEGIN
        INSERT INTO [dbo].[Projects] (CreatorUserId, ProjectName, Description, CreatedAt, ProjectCover)
        VALUES (@CreatorUserId, 'Physics', 'Small and concise headline for physics project', GETDATE(), 'assets/images/project1.jpeg');
    END

    IF NOT EXISTS (SELECT 1 FROM [dbo].[Projects] WHERE ProjectName = 'UI/UX Design System' AND CreatorUserId = @CreatorUserId)
    BEGIN
        INSERT INTO [dbo].[Projects] (CreatorUserId, ProjectName, Description, CreatedAt, ProjectCover)
        VALUES (@CreatorUserId, 'UI/UX Design System', 'Longer description that should not break the layout', GETDATE(), 'assets/images/project2.jpeg');
    END
END
GO
