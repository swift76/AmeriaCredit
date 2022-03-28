if exists (select * from sys.objects where name='sp_GetApplicationTemplateResult' and type='P')
	drop procedure IL.sp_GetApplicationTemplateResult
GO

create procedure IL.sp_GetApplicationTemplateResult(@SHOP_CODE				char(4),
												    @PRODUCT_CATEGORY_CODE	char(2))
AS
	declare @HeadCode char(4)
	select @HeadCode=HEAD_CODE from IL.SHOP where CODE=@SHOP_CODE

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
