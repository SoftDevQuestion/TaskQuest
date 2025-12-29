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

    UPDATE p
    SET
        DoneStatusCount = (
            SELECT COUNT(*) FROM Tasks
            WHERE ProjectId = p.ProjectId AND StatusId = 3
        ),
        InProgressStatusCount = (
            SELECT COUNT(*) FROM Tasks
            WHERE ProjectId = p.ProjectId AND StatusId = 2
        ),
        ToDoStatusCount = (
            SELECT COUNT(*) FROM Tasks
            WHERE ProjectId = p.ProjectId AND StatusId = 1
        ),
        UndefinedStatusCount = (
            SELECT COUNT(*) FROM Tasks
            WHERE ProjectId = p.ProjectId AND StatusId IS NULL
        )
    FROM Projects p
    WHERE p.ProjectId IN (SELECT ProjectId FROM @AffectedProjects);
END;
GO
