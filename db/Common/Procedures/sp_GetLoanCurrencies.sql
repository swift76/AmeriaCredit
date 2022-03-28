if exists (select * from sys.objects where name='sp_GetLoanCurrencies' and type='P')
	drop procedure Common.sp_GetLoanCurrencies
GO

create procedure Common.sp_GetLoanCurrencies(@LOAN_TYPE_CODE	char(2),
											 @LANGUAGE_CODE		char(2))
AS
	select CURRENCY as CODE,
		case @LANGUAGE_CODE
			when 'AM' then NAME_AM
			else NAME_EN
		end as NAME
	from Common.LOAN_LIMIT
	where LOAN_TYPE_CODE = @LOAN_TYPE_CODE
GO
