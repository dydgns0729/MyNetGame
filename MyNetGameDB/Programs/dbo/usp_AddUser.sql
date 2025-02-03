CREATE PROCEDURE [dbo].[usp_AddUser]
	@userId VARCHAR(20),
	@password VARCHAR(20),
	@mobile CHAR(11) = null,
	@level SMALLINT = 1,
	@health INT = 100,
	@gold INT = 1000
AS
BEGIN
	DECLARE @t_result INT
	SET @t_result = 0

	IF EXISTS (SELECT userId FROM userTbl WHERE userId = @userId)
	begin
		SET @t_result = 1
	end
	else
	begin
		INSERT INTO userTbl (userId, password, mobile, level, health, gold, mDate)
		VALUES (@userId, @password, @mobile, @level, @health, @gold, GETDATE())
	end

	SELECT @t_result result
END