if exists (select * from sys.objects where name='sp_GetProductCategories' and type='P')
	drop procedure IL.sp_GetProductCategories
GO

create procedure IL.sp_GetProductCategories(@LANGUAGE_CODE	char(2),
											@SHOP_USER_ID	int)
AS
	declare @SHOP_CODE char(4)

	select @SHOP_CODE=SHOP_CODE
	from IL.SHOP_USER
	where APPLICATION_USER_ID=@SHOP_USER_ID

	select @SHOP_CODE=case isnull(HEAD_CODE,'') when '' then CODE else HEAD_CODE end
	from IL.SHOP
	where CODE=@SHOP_CODE

	select c.CODE,
		case @LANGUAGE_CODE
			when 'AM' then c.NAME_AM
			else c.NAME_EN
		end as NAME
	from IL.PRODUCT_CATEGORY c
	join IL.SHOP_CATEGORY sc
		on c.CODE=sc.PRODUCT_CATEGORY_CODE
	where sc.SHOP_CODE=@SHOP_CODE
GO
