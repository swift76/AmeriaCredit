﻿create or alter procedure Common.sp_GetCustomerUserRegistrationProcess(
	@PROCESS_ID	uniqueidentifier
)
AS
	select PROCESS_ID,
		VERIFICATION_CODE,
		FIRST_NAME_EN,
		LAST_NAME_EN,
		MOBILE_PHONE,
		EMAIL,
		SOCIAL_CARD_NUMBER,
		HASH,
		ONBOARDING_ID,
		IS_STUDENT,
		TRY_COUNT,
		SMS_COUNT
	from Common.CUSTOMER_USER_REGISTRATION_PROCESS
	where PROCESS_ID = @PROCESS_ID
GO
