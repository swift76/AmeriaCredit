if exists (select * from sys.objects where name='sp_CheckCustomerUserExistenceByMobilePhone' and type='P')
	drop procedure Common.sp_CheckCustomerUserExistenceByMobilePhone
GO

create procedure Common.sp_CheckCustomerUserExistenceByMobilePhone (@MOBILE_PHONE	char(11))
AS
	select APPLICATION_USER_ID
	from Common.CUSTOMER_USER as cu 
	join Common.APPLICATION_USER as au
		on cu.APPLICATION_USER_ID = au.ID
	where cu.MOBILE_PHONE = @MOBILE_PHONE
GO
