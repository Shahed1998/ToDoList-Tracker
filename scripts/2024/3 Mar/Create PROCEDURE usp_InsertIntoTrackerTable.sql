USE [ToDoListTracker]
GO
/****** Object:  StoredProcedure [dbo].[usp_InsertIntoTrackerTable]    Script Date: 3/18/2024 3:05:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[usp_InsertIntoTrackerTable] (
@completed DECIMAL(6,3),
@planned DECIMAL(6,3)
)
AS
BEGIN

	DECLARE @percentage DECIMAL(6,3)
	DECLARE @sql NVARCHAR(MAX)

	IF @planned <> 0
	BEGIN
		SET @percentage = (@completed/@planned * 100)
	END
	ELSE
	BEGIN
		SET @percentage = NULL
	END

	INSERT INTO dbo.Tracker
	(
		Completed,
		Planned,
		[Percentage],
		[Date]
	)
	VALUES
	(
		@completed,
		@planned,
		@percentage,
		GETDATE()
	)

END
