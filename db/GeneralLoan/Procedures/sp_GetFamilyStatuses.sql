if exists (select * from sys.objects where name='sp_GetFamilyStatuses' and type='P')
	drop procedure GL.sp_GetFamilyStatuses
GO

create procedure GL.sp_GetFamilyStatuses(@LANGUAGE_CODE	char(2))
AS
	select CODE,
		case @LANGUAGE_CODE
			when 'AM' then NAME_AM
			else NAME_EN
		end as NAME
	from GL.FAMILY_STATUS
GO
