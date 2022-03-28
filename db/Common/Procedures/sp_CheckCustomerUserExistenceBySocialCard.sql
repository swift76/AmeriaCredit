if exists (select * from sys.objects where name='sp_CheckCustomerUserExistenceBySocialCard' and type='P')
	drop procedure Common.sp_CheckCustomerUserExistenceBySocialCard
GO

create procedure Common.sp_CheckCustomerUserExistenceBySocialCard (@SOCIAL_CARD_NUMBER	char(10))
AS
	select APPLICATION_USER_ID
	from Common.CUSTOMER_USER as cu
	join Common.APPLICATION_USER as au
		on cu.APPLICATION_USER_ID = au.ID
	where cu.SOCIAL_CARD_NUMBER = @SOCIAL_CARD_NUMBER
GO
