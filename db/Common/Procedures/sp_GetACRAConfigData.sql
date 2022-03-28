if exists (select * from sys.objects where name='sp_GetACRAConfigData' and type='P')
	drop procedure Common.sp_GetACRAConfigData
GO

create procedure Common.sp_GetACRAConfigData
AS
	select URL,USER_NAME,USER_PASSWORD
	from Common.SERVICE_CONFIGURATION
		where CODE='ACRA'
GO
