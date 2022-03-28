if exists (select * from sys.objects where name='APPLICATION_STATUS' and type='U')
	drop table Common.APPLICATION_STATUS
GO

CREATE TABLE Common.APPLICATION_STATUS (
	CODE	 	char(2)			NOT NULL,
	UI_NAME_AM	nvarchar(50)	NULL,
	UI_NAME_EN	varchar(50)		NULL,
	NAME_AM		nvarchar(50)	NOT NULL,
	NAME_EN		varchar(50)		NOT NULL
)
GO

CREATE UNIQUE CLUSTERED INDEX iAPPLICATION_STATUS1 ON Common.APPLICATION_STATUS(CODE)
GO