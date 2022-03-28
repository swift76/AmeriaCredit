if exists (select * from sys.objects where name='sp_GetLoanLimits' and type='P')
	drop procedure Common.sp_GetLoanLimits
GO

create procedure Common.sp_GetLoanLimits(@LOAN_TYPE_CODE	char(2),
										 @CURRENCY			char(3))
AS
	select FROM_AMOUNT,TO_AMOUNT
	from Common.LOAN_LIMIT
	where LOAN_TYPE_CODE = @LOAN_TYPE_CODE and CURRENCY=@CURRENCY
GO
