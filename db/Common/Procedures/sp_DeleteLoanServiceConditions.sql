if exists (select * from sys.objects where name='sp_DeleteLoanServiceConditions' and type='P')
	drop procedure Common.sp_DeleteLoanServiceConditions
GO

create procedure Common.sp_DeleteLoanServiceConditions
AS
	truncate table Common.LOAN_SERVICE_CONDITION
GO
