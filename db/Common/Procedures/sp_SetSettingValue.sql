if exists (select * from sys.objects where name='sp_SetSettingValue' and type='P')
	drop procedure Common.sp_SetSettingValue
GO

create procedure Common.sp_SetSettingValue(@CODE	varchar(30),
										   @VALUE	varchar(max))
AS
	update Common.SETTING
	set VALUE = @VALUE
	where CODE = @CODE
GO
