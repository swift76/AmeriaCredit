if exists (select * from sys.objects where name='SETTING' and type='U')
	drop table Common.SETTING
GO

CREATE TABLE Common.SETTING (
	CODE		varchar(30)		NOT NULL,
	VALUE		varchar(max)	NOT NULL,
	DESCRIPTION	nvarchar(100)	NOT NULL
)
GO

CREATE UNIQUE CLUSTERED INDEX iSETTING1 ON Common.SETTING(CODE)
GO
