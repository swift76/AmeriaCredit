if exists (select * from sys.objects where name='sp_SaveState' and type='P')
	drop procedure Common.sp_SaveState
GO

create procedure Common.sp_SaveState(@CODE		char(3),
									 @NAME_AM	nvarchar(50),
									 @NAME_EN	varchar(50))
AS
	insert into Common.STATE (CODE,NAME_AM,NAME_EN)
		values (@CODE,Common.ahf_ANSI2Unicode(@NAME_AM),@NAME_EN)
GO
