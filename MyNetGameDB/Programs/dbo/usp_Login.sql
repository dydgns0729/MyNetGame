CREATE PROCEDURE [dbo].[usp_Login]
	@userId VARCHAR(20),
	@password VARCHAR(20)
AS
BEGIN
	DECLARE @t_result INT

	IF EXISTS (SELECT userId FROM userTbl WHERE userId = @userId AND password = @password)
	begin
		SET @t_result = 0
	end
	else
	begin
		SET @t_result = 1
	end

	SELECT @t_result result
END