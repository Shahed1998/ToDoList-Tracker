/****** Object:  Trigger [dbo].[UpdateAchievement]    Script Date: 11/5/2024 10:12:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER TRIGGER [dbo].[UpdateAchievement] ON [dbo].[Tracker]
AFTER INSERT, Delete, Update
NOT FOR REPLICATION
AS
BEGIN
	SET NOCOUNT ON;

	IF OBJECT_ID(N'Achievement', 'U') IS NOT NULL

		DECLARE @result Decimal(5, 2)
		DECLARE @userId NVARCHAR(450)

		-- inserted and updated table stores copies of the affected rows in the trigger table
		IF EXISTS (SELECT TOP 1 UserId FROM inserted)
		BEGIN
			SELECT TOP 1 @userId = UserId FROM inserted
		END
		
		IF EXISTS (SELECT TOP 1 UserId FROM deleted)
		BEGIN
			SELECT TOP 1 @userId = UserId FROM deleted
		END

		SET @result =
		(
			SELECT ISNULL(NULLIF(
								CASE 
									WHEN SUM(t.Planned) = 0 THEN 0.0
									ELSE CAST((SUM(t.Completed) / SUM(t.Planned)) * 100 AS DECIMAL(5, 2))
								END, 0.0), 0.0)
						FROM Tracker t
						WHERE t.UserId = @userId 
		)

		IF EXISTS (SELECT TOP 1 Id FROM Achievement WHERE UserId=@userId)
		BEGIN
			UPDATE Achievement SET Result=@result WHERE UserId=@userId
		END
		ELSE
		BEGIN
			INSERT INTO Achievement (Result, UserId)
			VALUES (@result, @userId)
		END
END