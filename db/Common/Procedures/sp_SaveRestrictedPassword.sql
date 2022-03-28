create or alter procedure Common.sp_SaveRestrictedPassword(@PASSWORD	varchar(50))
AS
	insert into Common.RESTRICTED_PASSWORD (PASSWORD)
		values (lower(@PASSWORD))
GO
