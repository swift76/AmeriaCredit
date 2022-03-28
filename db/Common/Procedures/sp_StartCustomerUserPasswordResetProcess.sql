if exists (select * from sys.objects where name='sp_StartCustomerUserPasswordResetProcess' and type='P')
	drop procedure Common.sp_StartCustomerUserPasswordResetProcess
GO

create procedure Common.sp_StartCustomerUserPasswordResetProcess (
	@PROCESS_ID             uniqueidentifier,
	@PHONE					varchar(15),
	@HASH					varchar(1000)
)

AS
	insert into Common.CUSTOMER_USER_PASSWORD_RESET_PROCESS
		(PROCESS_ID, PHONE, HASH, EXPIRES_ON)
	values
		(@PROCESS_ID, @PHONE, @HASH, DATEADD(MINUTE, 30, GETDATE()))
GO
