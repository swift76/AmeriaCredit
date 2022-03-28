if exists (select * from sys.objects where name='sp_GetProductCategoriesForPartner' and type='P')
	drop procedure IL.sp_GetProductCategoriesForPartner
GO

create procedure IL.sp_GetProductCategoriesForPartner(
	@LANGUAGE_CODE			char(2),
	@PARTNER_COMPANY_CODE	varchar(8)
)
AS
	declare @SHOP_CODE char(4)

	select @SHOP_CODE=SHOP_CODE
	from Common.PARTNER_COMPANY
	where CODE=@PARTNER_COMPANY_CODE

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
