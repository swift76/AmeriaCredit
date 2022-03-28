create or alter procedure Common.sp_SetTryCustomerUserRegistrationProcess(
	@PROCESS_ID uniqueidentifier
)
AS
	update Common.CUSTOMER_USER_REGISTRATION_PROCESS
	set TRY_COUNT = TRY_COUNT+1
	where PROCESS_ID = @PROCESS_ID
GO
