if exists (select * from sys.objects where name='sp_GetMobilePhoneAuthorization' and type='P')
	drop procedure IL.sp_GetMobilePhoneAuthorization
GO

create procedure IL.sp_GetMobilePhoneAuthorization(@APPLICATION_ID	uniqueidentifier)
AS
	select APPLICATION_ID, SMS_HASH, SMS_SENT_DATE, SMS_COUNT
	from IL.MOBILE_PHONE_AUTHORIZATION with (UPDLOCK)
	where APPLICATION_ID = @APPLICATION_ID
GO
