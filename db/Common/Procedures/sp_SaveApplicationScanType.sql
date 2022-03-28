if exists (select * from sys.objects where name='sp_SaveApplicationScanType' and type='P')
	drop procedure Common.sp_SaveApplicationScanType
GO

create procedure Common.sp_SaveApplicationScanType(@CODE	char(1),
												   @NAME_AM	nvarchar(50),
												   @NAME_EN	varchar(50))
AS
	insert into Common.APPLICATION_SCAN_TYPE (CODE,NAME_AM,NAME_EN)
		values (@CODE,Common.ahf_ANSI2Unicode(@NAME_AM),@NAME_EN)
GO
