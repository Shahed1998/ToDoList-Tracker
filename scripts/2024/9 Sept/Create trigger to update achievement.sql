CREATE TRIGGER UpdateAchievement ON Tracker
AFTER INSERT
NOT FOR REPLICATION
AS
BEGIN
	IF OBJECT_ID(N'Achievement', 'U') IS NOT NULL

		DECLARE @result Decimal(5, 2)

		SET @result =
		(
			SELECT ISNULL(   NULLIF(
							 (
								 SELECT CASE
											WHEN SUM(t.Planned) = 0 THEN
												0.0
											ELSE
												CAST((SUM(t.Completed) / SUM(t.Planned)) * 100 AS DECIMAL(5, 2))
										END
								 FROM Tracker t
							 ), 0),
							 0
						 )
		)

		UPDATE Achievement SET Result=@result WHERE Id=1
END