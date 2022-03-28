if exists (select * from sys.objects where name='sp_GetCountries' and type='P')
	drop procedure Common.sp_GetCountries
GO

create procedure Common.sp_GetCountries(@LANGUAGE_CODE	char(2))
AS
	select CODE,
		case @LANGUAGE_CODE
			when 'AM' then NAME_AM
			else NAME_EN
		end as NAME
	from Common.COUNTRY
	where CODE = 'AM'

	union all

	select CODE,
		case @LANGUAGE_CODE
			when 'AM' then NAME_AM
			else NAME_EN
		end as NAME
	from Common.COUNTRY
	where not CODE in ('AM','NK')
GO