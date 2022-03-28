if exists (select * from sys.objects where name='sp_SaveRefinancingLoan' and type='P')
	drop procedure GL.sp_SaveRefinancingLoan
GO

create procedure GL.sp_SaveRefinancingLoan(@APPLICATION_ID		uniqueidentifier,
										   @ROW_ID 				int,
										   @ORIGINAL_BANK_NAME	nvarchar(40),
										   @LOAN_TYPE			nvarchar(40),
										   @INITIAL_INTEREST	money,
										   @CURRENCY			char(3),
										   @INITIAL_AMOUNT		money,
										   @CURRENT_BALANCE		money,
										   @DRAWDOWN_DATE		datetime,
										   @MATURITY_DATE		datetime)
AS
	insert into GL.REFINANCING_LOAN
		(APPLICATION_ID,ROW_ID,ORIGINAL_BANK_NAME,LOAN_TYPE,INITIAL_INTEREST,CURRENCY,INITIAL_AMOUNT,CURRENT_BALANCE,DRAWDOWN_DATE,MATURITY_DATE)
	values
		(@APPLICATION_ID,@ROW_ID,Common.ahf_ANSI2Unicode(@ORIGINAL_BANK_NAME),Common.ahf_ANSI2Unicode(@LOAN_TYPE),@INITIAL_INTEREST,@CURRENCY,@INITIAL_AMOUNT,@CURRENT_BALANCE,@DRAWDOWN_DATE,@MATURITY_DATE)
GO
