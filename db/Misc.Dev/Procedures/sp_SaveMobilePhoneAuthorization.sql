if exists (select * from sys.objects where name='sp_SaveMobilePhoneAuthorization' and type='P')
	drop procedure IL.sp_SaveMobilePhoneAuthorization
GO

create procedure IL.sp_SaveMobilePhoneAuthorization(@APPLICATION_ID	uniqueidentifier,
												   @SMS_HASH 		varchar(1000))
AS
	declare @SMS_COUNT int
	select @SMS_COUNT = SMS_COUNT
	from IL.MOBILE_PHONE_AUTHORIZATION
	where APPLICATION_ID = @APPLICATION_ID

	execute IL.sp_DeleteMobilePhoneAuthorization @APPLICATION_ID

	insert into IL.MOBILE_PHONE_AUTHORIZATION (APPLICATION_ID, SMS_HASH)
		values (@APPLICATION_ID, 'zv9gqxIbT0IAL2RI/ndM2IzL3Yg=')

	if not @SMS_COUNT is null
		update IL.MOBILE_PHONE_AUTHORIZATION
		set SMS_COUNT = @SMS_COUNT + 1
		where APPLICATION_ID = @APPLICATION_ID
GO
