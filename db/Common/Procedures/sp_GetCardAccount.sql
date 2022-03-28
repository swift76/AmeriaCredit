if exists (select * from sys.objects where name='sp_GetCardAccount' and type='P')
	drop procedure Common.sp_GetCardAccount
GO

create procedure Common.sp_GetCardAccount(@CODE char(2))

AS
	select IS_CARD_ACCOUNT
	from Common.LOAN_TYPE
	where CODE = @CODE
GO
