CREATE TABLE [dbo].[userTbl]
(
	userId VARCHAR(20) NOT NULL PRIMARY KEY,		-- 유저 아이디 (이메일)
	password VARCHAR(20) NOT NULL,					-- 비밀번호
	mobile CHAR(11),							-- 유저 전화번호
	level SMALLINT,								-- 유저 레벨
	health INT NOT NULL,						-- 유저 체력
	gold INT NOT NULL,							-- 유저 골드
	mDate date									-- 유저 가입일
)

