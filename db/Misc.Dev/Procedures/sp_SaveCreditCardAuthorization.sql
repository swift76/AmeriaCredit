if exists (select * from sys.objects where name='sp_SaveCreditCardAuthorization' and type='P')
	drop procedure GL.sp_SaveCreditCardAuthorization
GO

create procedure GL.sp_SaveCreditCardAuthorization(@APPLICATION_ID	uniqueidentifier,
												   @SMS_HASH 		varchar(1000))
AS
	declare @SMS_COUNT int
	select @SMS_COUNT = SMS_COUNT
	from GL.CREDIT_CARD_AUTHORIZATION
	where APPLICATION_ID = @APPLICATION_ID

	execute GL.sp_DeleteCreditCardAuthorization @APPLICATION_ID

	insert into GL.CREDIT_CARD_AUTHORIZATION (APPLICATION_ID, SMS_HASH)
		values (@APPLICATION_ID, 'zv9gqxIbT0IAL2RI/ndM2IzL3Yg=')

	if not @SMS_COUNT is null
		update GL.CREDIT_CARD_AUTHORIZATION
		set SMS_COUNT = @SMS_COUNT + 1
		where APPLICATION_ID = @APPLICATION_ID
GO
