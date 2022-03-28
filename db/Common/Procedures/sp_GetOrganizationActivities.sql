if exists (select * from sys.objects where name='sp_GetOrganizationActivities' and type='P')
	drop procedure Common.sp_GetOrganizationActivities
GO

create procedure Common.sp_GetOrganizationActivities(@LANGUAGE_CODE	char(2))
AS
	select CODE,
		case @LANGUAGE_CODE
			when 'AM' then NAME_AM
			else NAME_EN
		end as NAME
	from Common.ORGANIZATION_ACTIVITY
GO
