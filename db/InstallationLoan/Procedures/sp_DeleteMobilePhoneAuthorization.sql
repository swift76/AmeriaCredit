if exists (select * from sys.objects where name='sp_DeleteMobilePhoneAuthorization' and type='P')
	drop procedure IL.sp_DeleteMobilePhoneAuthorization
GO

create procedure IL.sp_DeleteMobilePhoneAuthorization(@APPLICATION_ID	uniqueidentifier)
AS
	delete from IL.MOBILE_PHONE_AUTHORIZATION where APPLICATION_ID = @APPLICATION_ID
GO
