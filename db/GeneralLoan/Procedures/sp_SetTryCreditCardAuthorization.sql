if exists (select * from sys.objects where name='sp_SetTryCreditCardAuthorization' and type='P')
	drop procedure GL.sp_SetTryCreditCardAuthorization
GO

create procedure GL.sp_SetTryCreditCardAuthorization(@APPLICATION_ID uniqueidentifier,
													 @TRY_COUNT		 int)
AS
	update GL.CREDIT_CARD_AUTHORIZATION
	set    TRY_COUNT = @TRY_COUNT
	where  APPLICATION_ID = @APPLICATION_ID
GO
