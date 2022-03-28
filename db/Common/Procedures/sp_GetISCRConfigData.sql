if exists (select * from sys.objects where name='sp_GetISCRConfigData' and type='P')
	drop procedure Common.sp_GetISCRConfigData
GO

create procedure Common.sp_GetISCRConfigData
AS
	select URL,USER_NAME,USER_PASSWORD
	from Common.SERVICE_CONFIGURATION
		where CODE='ISCR'
GO
