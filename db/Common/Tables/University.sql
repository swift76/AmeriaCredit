if exists (select * from sys.objects where name='UNIVERSITY' and type='U')
	drop table Common.UNIVERSITY
GO

CREATE TABLE Common.UNIVERSITY (
	CODE 	char(2)			NOT NULL,
	NAME_AM	nvarchar(50)	NOT NULL,
	NAME_EN	varchar(50)		NOT NULL
)
GO

CREATE UNIQUE CLUSTERED INDEX iUNIVERSITY1 ON Common.UNIVERSITY(CODE)
GO
