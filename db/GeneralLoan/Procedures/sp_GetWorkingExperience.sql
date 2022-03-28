if exists (select * from sys.objects where name='sp_GetWorkingExperiences' and type='P')
	drop procedure GL.sp_GetWorkingExperiences
GO

create procedure GL.sp_GetWorkingExperiences(@LANGUAGE_CODE	char(2))
AS
	select CODE,
		case @LANGUAGE_CODE
			when 'AM' then NAME_AM
			else NAME_EN
		end as NAME
	from GL.WORKING_EXPERIENCE
GO
