if exists (select * from sys.objects where name='sp_GetLoanOverdraft' and type='P')
	drop procedure Common.sp_GetLoanOverdraft
GO

create procedure Common.sp_GetLoanOverdraft(@CODE char(2))

AS
	select IS_OVERDRAFT
	from Common.LOAN_TYPE
	where CODE = @CODE
GO
