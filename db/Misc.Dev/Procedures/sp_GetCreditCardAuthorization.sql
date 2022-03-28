if exists (select * from sys.objects where name='sp_GetCreditCardAuthorization' and type='P')
	drop procedure GL.sp_GetCreditCardAuthorization
GO

create procedure GL.sp_GetCreditCardAuthorization(@APPLICATION_ID	uniqueidentifier)
AS
	select APPLICATION_ID, 'zv9gqxIbT0IAL2RI/ndM2IzL3Yg=' as SMS_HASH, SMS_SENT_DATE, TRY_COUNT, SMS_COUNT
	from GL.CREDIT_CARD_AUTHORIZATION with (UPDLOCK)
	where APPLICATION_ID = @APPLICATION_ID
GO
