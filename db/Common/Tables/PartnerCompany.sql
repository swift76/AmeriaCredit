if exists (select * from sys.objects where name='PARTNER_COMPANY' and type='U')
	drop table Common.PARTNER_COMPANY
GO

CREATE TABLE Common.PARTNER_COMPANY(
	CODE		varchar(8)		NOT NULL,
	NAME		nvarchar(40)	NULL,
	PARENT_CODE	varchar(8)		NULL,
	SHOP_CODE	char(4)			NULL
)
GO

CREATE UNIQUE CLUSTERED INDEX iPARTNER_COMPANY1 ON Common.PARTNER_COMPANY(CODE)
GO
