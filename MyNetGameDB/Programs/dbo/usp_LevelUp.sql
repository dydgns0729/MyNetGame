CREATE PROCEDURE [dbo].[usp_LevelUp]
	@userId VARCHAR(20)
AS
BEGIN
	--레벨업 성공시 레벨업한 최종 레벨값 반환, 실패시 0반환
	DECLARE @t_result INT
	SET @t_result = 0
	IF EXISTS (SELECT userId FROM userTbl WHERE userId = @userId)
	begin
		UPDATE userTbl SET level = level+1 WHERE userId = @userId
		SELECT level FROM userTbl WHERE userId = @userId
	end
	else
	begin
		SELECT @t_result
	end
END