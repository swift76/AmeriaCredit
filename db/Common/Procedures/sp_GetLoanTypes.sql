if exists (select * from sys.objects where name='sp_GetLoanTypes' and type='P')
	drop procedure Common.sp_GetLoanTypes
GO

create procedure Common.sp_GetLoanTypes(
	@LANGUAGE_CODE	char(2),
	@IS_STUDENT		bit)
AS
	declare @CurrentDate date=getdate()
	select CODE,
		case @LANGUAGE_CODE
			when 'AM' then NAME_AM
			else NAME_EN
		end as NAME,
		IS_OVERDRAFT,
		IS_CARD_ACCOUNT,
		IS_STUDENT
	from Common.LOAN_TYPE
	where @CurrentDate between isnull(FROM_DATE,@CurrentDate) and isnull(TO_DATE,@CurrentDate)
		and CODE<>'00'
		and @IS_STUDENT in (1,IS_STUDENT)
GO
