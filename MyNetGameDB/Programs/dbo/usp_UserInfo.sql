CREATE PROCEDURE [dbo].[usp_UserInfo]
	@userId VARCHAR(20)
AS
BEGIN
	IF EXISTS (SELECT userId FROM userTbl WHERE userId = @userId)
	begin
		SELECT * FROM userTbl WHERE userId = @userId 
	end
	else
	begin
		SELECT NULL
	end
END