-- Add UpdatedAt columns and Triggers for tracking activity

USE [db_ac2fe4_todo]; -- Ensure correct DB context
GO

-- 1. Add UpdatedAt to Projects if missing
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Projects' AND COLUMN_NAME = 'UpdatedAt')
BEGIN
    ALTER TABLE Projects ADD UpdatedAt DATETIME DEFAULT GETDATE();
    PRINT 'Added UpdatedAt to Projects.';
END
GO

-- Backfill Projects.UpdatedAt with CreatedAt
UPDATE Projects SET UpdatedAt = CreatedAt WHERE UpdatedAt IS NULL;
GO

-- 2. Add UpdatedAt to Tasks if missing
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Tasks' AND COLUMN_NAME = 'UpdatedAt')
BEGIN
    ALTER TABLE Tasks ADD UpdatedAt DATETIME DEFAULT GETDATE();
    PRINT 'Added UpdatedAt to Tasks.';
END
GO

-- Backfill Tasks.UpdatedAt with CreatedAt
UPDATE Tasks SET UpdatedAt = CreatedAt WHERE UpdatedAt IS NULL;
GO

-- 3. Trigger to update Tasks.UpdatedAt
CREATE OR ALTER TRIGGER trg_Tasks_UpdatedAt
ON Tasks
AFTER UPDATE
AS
BEGIN
    UPDATE Tasks
    SET UpdatedAt = GETDATE()
    FROM Tasks t
    INNER JOIN inserted i ON t.TaskID = i.TaskID;
END
GO

-- 4. Update Project Trigger to also update Project.UpdatedAt
CREATE OR ALTER TRIGGER trg_Sync_Project_With_Tasks
ON Tasks
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @AffectedProjects TABLE (ProjectId INT);

    INSERT INTO @AffectedProjects (ProjectId)
    SELECT DISTINCT ProjectId FROM inserted
    UNION
    SELECT DISTINCT ProjectId FROM deleted;

    -- Update Counts AND UpdatedAt
    UPDATE p
    SET
        DoneStatusCount = (SELECT COUNT(*) FROM Tasks WHERE ProjectId = p.ProjectId AND StatusId = 3),
        InProgressStatusCount = (SELECT COUNT(*) FROM Tasks WHERE ProjectId = p.ProjectId AND StatusId = 2),
        ToDoStatusCount = (SELECT COUNT(*) FROM Tasks WHERE ProjectId = p.ProjectId AND StatusId = 1),
        UndefinedStatusCount = (SELECT COUNT(*) FROM Tasks WHERE ProjectId = p.ProjectId AND StatusId IS NULL),
        UpdatedAt = GETDATE() -- Update timestamp
    FROM Projects p
    WHERE p.ProjectId IN (SELECT ProjectId FROM @AffectedProjects);
END;
GO

PRINT 'Schema and Triggers updated successfully.';
