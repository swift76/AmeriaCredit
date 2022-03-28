if exists (select * from sys.objects where name='sp_GetServiceConfiguration' and type='P')
	drop procedure Common.sp_GetServiceConfiguration
GO

create procedure Common.sp_GetServiceConfiguration(@CODE char(4))
AS
	select URL,USER_NAME,USER_PASSWORD
	from Common.SERVICE_CONFIGURATION
	where CODE=@CODE
GO
