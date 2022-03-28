if exists (select * from sys.objects where name='DOCUMENT_TYPE' and type='U')
	drop table Common.DOCUMENT_TYPE
GO

CREATE TABLE Common.DOCUMENT_TYPE (
	CODE 	char(1)			NOT NULL,
	NAME_AM	nvarchar(50)	NOT NULL,
	NAME_EN	varchar(50)		NOT NULL
)
GO

CREATE UNIQUE CLUSTERED INDEX iDOCUMENT_TYPE1 ON Common.DOCUMENT_TYPE(CODE)
GO
