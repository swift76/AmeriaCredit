if exists (select * from sys.objects where name='sp_GetInstallationTemplatesForPartner' and type='P')
	drop procedure IL.sp_GetInstallationTemplatesForPartner
GO

create procedure IL.sp_GetInstallationTemplatesForPartner(
	@PARTNER_COMPANY_CODE	varchar(8),
    @PRODUCT_CATEGORY_CODE	char(2)
)
AS
	declare @SHOP_CODE char(4),@HeadCode char(4)

	select @SHOP_CODE=SHOP_CODE
	from Common.PARTNER_COMPANY
	where CODE=@PARTNER_COMPANY_CODE

	select @HeadCode=HEAD_CODE
	from IL.SHOP
	where CODE=@SHOP_CODE

	SELECT TERM_FROM,
		   TERM_TO,
		   TEMPLATE_CODE,
		   TEMPLATE_NAME,
		   SERVICE_AMOUNT,
		   SERVICE_INTEREST
	FROM IL.AGREEMENT_TEMPLATE
	WHERE SHOP_CODE in (@HeadCode, @SHOP_CODE, '') and
		  PRODUCT_CATEGORY_CODE in (@PRODUCT_CATEGORY_CODE, '')
GO
