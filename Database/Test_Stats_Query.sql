-- Test Query for Task Statistics
DECLARE @UserId INT = 1; -- Assuming UserID 1 exists
DECLARE @Username NVARCHAR(50) = 'admin'; -- Assuming admin exists

SELECT 
    COUNT(CASE WHEN t.StatusId = 3 THEN 1 END) as DoneCount,
    COUNT(CASE WHEN t.StatusId != 3 AND t.DueDate < CAST(GETDATE() AS DATE) THEN 1 END) as OverdueCount,
    COUNT(CASE WHEN t.StatusId != 3 AND (t.DueDate >= CAST(GETDATE() AS DATE) OR t.DueDate IS NULL) THEN 1 END) as ToDoCount
FROM Tasks t
JOIN Projects p ON t.ProjectID = p.ProjectID
LEFT JOIN ProjectTeams pt ON p.ProjectID = pt.ProjectID
LEFT JOIN Team tm_team ON pt.TeamID = tm_team.TeamId
LEFT JOIN TeamMembers tm ON tm_team.TeamId = tm.TeamId
-- Simulating the WHERE clause logic (simplified for test)
-- WHERE (p.CreatorUserId = @UserId OR tm.Username = @Username)
