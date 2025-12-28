CREATE TRIGGER trg_Block_Project_Status_Manual_Update
ON Projects
AFTER UPDATE
AS
BEGIN
    IF UPDATE(DoneStatusCount)
       OR UPDATE(InProgressStatusCount)
       OR UPDATE(ToDoStatusCount)
       OR UPDATE(UndefinedStatusCount)
    BEGIN
        RAISERROR (
            N'این ستون‌ها فقط از طریق تغییر Task به‌روزرسانی می‌شوند.',
            16, 1
        );
        ROLLBACK TRANSACTION;
    END
END;
GO
