if exists (select * from sys.objects where name='sp_GetAddressCountries' and type='P')
	drop procedure Common.sp_GetAddressCountries
GO

create procedure Common.sp_GetAddressCountries(@LANGUAGE_CODE	char(2))
AS
	select CODE,
		case @LANGUAGE_CODE
			when 'AM' then NAME_AM
			else NAME_EN
		end as NAME
	from Common.COUNTRY
	where CODE = 'AM'
GO