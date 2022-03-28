if exists (select * from sys.objects where name='sp_InsertAgreementTemplate' and type='P')
	drop procedure IL.sp_InsertAgreementTemplate
GO

create procedure IL.sp_InsertAgreementTemplate(@SHOP_CODE				char(4),
											   @PRODUCT_CATEGORY_CODE	char(2),
											   @TEMPLATE_CODE			char(4),
											   @TEMPLATE_NAME			nvarchar(50),
											   @INTEREST				money,
											   @TERM_FROM				int,
											   @TERM_TO					int,
											   @SERVICE_AMOUNT			money,
											   @SERVICE_INTEREST		money)
AS
	insert into IL.AGREEMENT_TEMPLATE (SHOP_CODE,PRODUCT_CATEGORY_CODE,TEMPLATE_CODE,TEMPLATE_NAME,INTEREST,TERM_FROM,TERM_TO,SERVICE_AMOUNT,SERVICE_INTEREST)
	values (@SHOP_CODE,@PRODUCT_CATEGORY_CODE,@TEMPLATE_CODE,@TEMPLATE_NAME,@INTEREST,@TERM_FROM,@TERM_TO,@SERVICE_AMOUNT,@SERVICE_INTEREST)
GO
