if exists (select * from sys.objects where name='sp_SaveApplicationProduct' and type='P')
	drop procedure IL.sp_SaveApplicationProduct
GO

create procedure IL.sp_SaveApplicationProduct(@APPLICATION_ID			uniqueidentifier,
											  @DESCRIPTION				nvarchar(150),
											  @QUANTITY					int,
											  @PRICE					money,
											  @PRODUCT_CATEGORY_CODE	char(2))
AS
	insert into IL.APPLICATION_PRODUCT
		(APPLICATION_ID, DESCRIPTION, QUANTITY, PRICE, PRODUCT_CATEGORY_CODE)
	values
		(@APPLICATION_ID, @DESCRIPTION, @QUANTITY, @PRICE, @PRODUCT_CATEGORY_CODE)
GO
