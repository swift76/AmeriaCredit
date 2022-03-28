if exists (select * from sys.objects where name='sp_GetCustomerMissingData' and type='P')
	drop procedure GL.sp_GetCustomerMissingData
GO

create procedure GL.sp_GetCustomerMissingData(@CUSTOMER_USER_ID	int)
AS
	select  LOGIN as MOBILE_PHONE_1,
			EMAIL
	from Common.APPLICATION_USER
	where ID = @CUSTOMER_USER_ID
GO
