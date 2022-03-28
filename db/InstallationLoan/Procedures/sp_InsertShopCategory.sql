if exists (select * from sys.objects where name='sp_InsertShopCategory' and type='P')
	drop procedure IL.sp_InsertShopCategory
GO

create procedure IL.sp_InsertShopCategory(@SHOP_CODE				char(4),
										  @PRODUCT_CATEGORY_CODE	char(2))
AS
	insert into IL.SHOP_CATEGORY (SHOP_CODE,PRODUCT_CATEGORY_CODE)
	values (@SHOP_CODE,@PRODUCT_CATEGORY_CODE)
GO
