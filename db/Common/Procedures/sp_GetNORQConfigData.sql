if exists (select * from sys.objects where name='sp_GetNORQConfigData' and type='P')
	drop procedure Common.sp_GetNORQConfigData
GO

create procedure Common.sp_GetNORQConfigData
AS
	select URL,USER_NAME,USER_PASSWORD
	from Common.SERVICE_CONFIGURATION
		where CODE='NORQ'
GO
