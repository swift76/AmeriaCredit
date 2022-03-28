if exists (select * from sys.objects where name='sp_GetUniversities' and type='P')
	drop procedure Common.sp_GetUniversities
GO

create procedure Common.sp_GetUniversities(@LANGUAGE_CODE	char(2))
AS
	select CODE,
		case @LANGUAGE_CODE
			when 'AM' then NAME_AM
			else NAME_EN
		end as NAME
	from Common.UNIVERSITY
GO
