if exists (select * from sys.objects where name='COUNTRY' and type='U')
	drop table Common.COUNTRY
GO

CREATE TABLE Common.COUNTRY (
	CODE	char(2)			NOT NULL,
	NAME_AM	nvarchar(50)	NOT NULL,
	NAME_EN	varchar(50)		NOT NULL
)
GO

CREATE UNIQUE CLUSTERED INDEX iCOUNTRY1 ON Common.COUNTRY(CODE)
GO
