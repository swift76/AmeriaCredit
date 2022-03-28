if exists (select * from sys.objects where name='sp_CheckCustomerUserExistenceByEmail' and type='P')
	drop procedure Common.sp_CheckCustomerUserExistenceByEmail
GO

create procedure Common.sp_CheckCustomerUserExistenceByEmail (@EMAIL	varchar(70))
AS
	select APPLICATION_USER_ID
	from Common.CUSTOMER_USER as cu 
	join Common.APPLICATION_USER as au
		on cu.APPLICATION_USER_ID = au.ID
	where cu.EMAIL = @EMAIL
GO
