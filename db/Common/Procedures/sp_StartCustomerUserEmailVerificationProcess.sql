if exists (select * from sys.objects where name='sp_StartCustomerUserEmailVerificationProcess' and type='P')
	drop procedure Common.sp_StartCustomerUserEmailVerificationProcess
GO

create procedure Common.sp_StartCustomerUserEmailVerificationProcess (
	@PROCESS_ID			uniqueidentifier,
	@CUSTOMER_USER_ID	int,
	@EMAIL				varchar(70)
)
AS
	insert into Common.CUSTOMER_USER_EMAIL_VERIFICATION_PROCESS
		(PROCESS_ID, CUSTOMER_USER_ID, EMAIL)
	values
		(@PROCESS_ID, @CUSTOMER_USER_ID, @EMAIL)
GO
