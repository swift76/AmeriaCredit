if exists (select * from sys.objects where name='sp_GetLoanParameters' and type='P')
	drop procedure Common.sp_GetLoanParameters
GO

create procedure Common.sp_GetLoanParameters(@LOAN_TYPE_CODE	char(2))
AS
	select REPAYMENT_DAY_FROM,REPAYMENT_DAY_TO,IS_OVERDRAFT,IS_REPAY_DAY_FIXED,IS_CARD_ACCOUNT
		,IS_REPAY_START_DAY,IS_REPAY_NEXT_MONTH,REPAY_TRANSITION_DAY
	from Common.LOAN_TYPE
	where CODE = @LOAN_TYPE_CODE
GO
