create or alter procedure Common.sp_UpdateCustomerUserRegistrationProcess(
	@PROCESS_ID			uniqueidentifier,
	@VERIFICATION_CODE	varchar(6))
AS
	if exists (select *
			   from Common.CUSTOMER_USER_REGISTRATION_PROCESS
			   where PROCESS_ID = @PROCESS_ID)
		update Common.CUSTOMER_USER_REGISTRATION_PROCESS
		set	VERIFICATION_CODE = @VERIFICATION_CODE,
			SMS_COUNT = SMS_COUNT+1
		where PROCESS_ID = @PROCESS_ID
GO
