if exists (select * from sys.objects where name='sp_GetApplicationProducts' and type='P')
	drop procedure IL.sp_GetApplicationProducts
GO

create procedure IL.sp_GetApplicationProducts(@APPLICATION_ID	uniqueidentifier)
AS
	select DESCRIPTION, QUANTITY, PRICE, PRODUCT_CATEGORY_CODE
	from   APPLICATION_PRODUCT
	where  APPLICATION_ID = @APPLICATION_ID
GO
