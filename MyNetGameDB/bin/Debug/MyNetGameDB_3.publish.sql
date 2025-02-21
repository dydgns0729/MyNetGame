﻿/*
MyNetGameDB의 배포 스크립트

이 코드는 도구를 사용하여 생성되었습니다.
파일 내용을 변경하면 잘못된 동작이 발생할 수 있으며, 코드를 다시 생성하면
변경 내용이 손실됩니다.
*/

GO
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER ON;

SET NUMERIC_ROUNDABORT OFF;


GO
:setvar DatabaseName "MyNetGameDB"
:setvar DefaultFilePrefix "MyNetGameDB"
:setvar DefaultDataPath "C:\Users\Admin\AppData\Local\Microsoft\Microsoft SQL Server Local DB\Instances\MSSQLLocalDB\"
:setvar DefaultLogPath "C:\Users\Admin\AppData\Local\Microsoft\Microsoft SQL Server Local DB\Instances\MSSQLLocalDB\"

GO
:on error exit
GO
/*
SQLCMD 모드가 지원되지 않으면 SQLCMD 모드를 검색하고 스크립트를 실행하지 않습니다.
SQLCMD 모드를 설정한 후에 이 스크립트를 다시 사용하려면 다음을 실행합니다.
SET NOEXEC OFF; 
*/
:setvar __IsSqlCmdEnabled "True"
GO
IF N'$(__IsSqlCmdEnabled)' NOT LIKE N'True'
    BEGIN
        PRINT N'이 스크립트를 실행하려면 SQLCMD 모드를 사용하도록 설정해야 합니다.';
        SET NOEXEC ON;
    END


GO
USE [$(DatabaseName)];


GO
PRINT N'프로시저 [dbo].[usp_LevelUp]을(를) 만드는 중...';


GO
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
	end
	else
	begin
		SELECT @t_result
	end
END
GO
PRINT N'프로시저 [dbo].[usp_Login]을(를) 만드는 중...';


GO
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
GO
PRINT N'프로시저 [dbo].[usp_UserInfo]을(를) 만드는 중...';


GO
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
GO
PRINT N'업데이트가 완료되었습니다.';


GO
