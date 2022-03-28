if exists (select * from sys.objects where name='sp_GetGSCRConfigData' and type='P')
	drop procedure Common.sp_GetGSCRConfigData
GO

create procedure Common.sp_GetGSCRConfigData
AS
	select URL,USER_NAME,USER_PASSWORD
	from Common.SERVICE_CONFIGURATION
		where CODE='GSCR'
GO
