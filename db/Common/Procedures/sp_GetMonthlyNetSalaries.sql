if exists (select * from sys.objects where name='sp_GetMonthlyNetSalaries' and type='P')
	drop procedure Common.sp_GetMonthlyNetSalaries
GO

create procedure Common.sp_GetMonthlyNetSalaries(@LANGUAGE_CODE	char(2))
AS
	select CODE,
		case @LANGUAGE_CODE
			when 'AM' then NAME_AM
			else NAME_EN
		end as NAME
	from Common.MONTHLY_NET_SALARY
GO
