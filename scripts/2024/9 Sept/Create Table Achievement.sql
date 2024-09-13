CREATE TABLE Achievement 
(
	Id int PRIMARY KEY Identity(1,1),
	Result Decimal(5,2) NOT NULL
)

IF OBJECT_ID(N'Achievement','U') IS NOT NULL
BEGIN
	INSERT INTO Achievement
	(Result) VALUES (0)
END