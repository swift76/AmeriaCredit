if exists (select * from sys.objects where name='AGREEMENT_TEMPLATE' and type='U')
	drop table IL.AGREEMENT_TEMPLATE
GO

CREATE TABLE IL.AGREEMENT_TEMPLATE
(
	ID						int				identity(1,1)	NOT NULL,
	SHOP_CODE				char(4)			NOT NULL,
	PRODUCT_CATEGORY_CODE	char(2)			NOT NULL,
	TEMPLATE_CODE			char(4)			NOT NULL,
	TEMPLATE_NAME			nvarchar(50)	NOT NULL,
	INTEREST				money			NOT NULL,
	SERVICE_AMOUNT			money			NOT NULL,
	SERVICE_INTEREST		money			NOT NULL,
	TERM_FROM				int				NULL,
	TERM_TO					int				NULL
)
GO

CREATE UNIQUE CLUSTERED INDEX iAGREEMENT_TEMPLATE1 ON IL.AGREEMENT_TEMPLATE(ID)
GO

CREATE UNIQUE INDEX iAGREEMENT_TEMPLATE2 ON IL.AGREEMENT_TEMPLATE(SHOP_CODE,PRODUCT_CATEGORY_CODE)
GO
