if exists (select * from sys.objects where name='sp_DeleteCreditCardAuthorization' and type='P')
	drop procedure GL.sp_DeleteCreditCardAuthorization
GO

create procedure GL.sp_DeleteCreditCardAuthorization(@APPLICATION_ID	uniqueidentifier)
AS
	delete from GL.CREDIT_CARD_AUTHORIZATION where APPLICATION_ID = @APPLICATION_ID
GO
