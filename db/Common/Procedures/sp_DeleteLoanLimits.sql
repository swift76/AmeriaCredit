if exists (select * from sys.objects where name='sp_DeleteLoanLimits' and type='P')
	drop procedure Common.sp_DeleteLoanLimits
GO

create procedure Common.sp_DeleteLoanLimits(@LOAN_TYPE_CODE	char(2))
AS
	delete from Common.LOAN_LIMIT
	where LOAN_TYPE_CODE=@LOAN_TYPE_CODE
GO
