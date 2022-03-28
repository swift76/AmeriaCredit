if exists (select * from sys.objects where name='sp_SetTryMobilePhoneAuthorization' and type='P')
	drop procedure IL.sp_SetTryMobilePhoneAuthorization
GO

create procedure IL.sp_SetTryMobilePhoneAuthorization(@APPLICATION_ID	uniqueidentifier,
													  @SMS_COUNT		int)
AS
	update IL.MOBILE_PHONE_AUTHORIZATION
	set    SMS_COUNT = @SMS_COUNT
	where  APPLICATION_ID = @APPLICATION_ID
GO
