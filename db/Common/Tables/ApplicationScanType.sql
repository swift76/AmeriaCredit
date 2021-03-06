if exists (select * from sys.objects where name='APPLICATION_SCAN_TYPE' and type='U')
	drop table Common.APPLICATION_SCAN_TYPE
GO

CREATE TABLE Common.APPLICATION_SCAN_TYPE (
	CODE 	char(1)			NOT NULL,
	NAME_AM	nvarchar(50)	NOT NULL,
	NAME_EN varchar(50)		NOT NULL
)
GO

CREATE UNIQUE CLUSTERED INDEX iAPPLICATION_SCAN_TYPE1 ON Common.APPLICATION_SCAN_TYPE(CODE)
GO
