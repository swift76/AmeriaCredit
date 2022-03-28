if exists (select * from sys.objects where name='sp_SaveOrganizationActivity' and type='P')
	drop procedure Common.sp_SaveOrganizationActivity
GO

create procedure Common.sp_SaveOrganizationActivity(@CODE		char(2),
										   			@NAME_AM	nvarchar(50),
										   			@NAME_EN	varchar(50))
AS
	insert into Common.ORGANIZATION_ACTIVITY (CODE,NAME_AM,NAME_EN)
		values (@CODE,Common.ahf_ANSI2Unicode(@NAME_AM),@NAME_EN)
GO
