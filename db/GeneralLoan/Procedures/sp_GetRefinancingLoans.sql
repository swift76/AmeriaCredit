if exists (select * from sys.objects where name='sp_GetRefinancingLoans' and type='P')
	drop procedure GL.sp_GetRefinancingLoans
GO

create procedure GL.sp_GetRefinancingLoans(@APPLICATION_ID	uniqueidentifier)
AS
	select APPLICATION_ID,
		   ROW_ID,
		   ORIGINAL_BANK_NAME,
		   LOAN_TYPE,
		   INITIAL_INTEREST,
		   INITIAL_AMOUNT,
		   CURRENT_BALANCE,
		   DRAWDOWN_DATE,
		   MATURITY_DATE,
		   LOAN_CODE
	from   GL.REFINANCING_LOAN
	where  APPLICATION_ID = @APPLICATION_ID
GO
