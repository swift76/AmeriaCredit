if exists (select * from sys.objects where name='OBJECT_STATE' and type='U')
	drop table Common.OBJECT_STATE
GO

CREATE TABLE Common.OBJECT_STATE (
	ID 			tinyint IDENTITY(1,1)	NOT NULL,
	DESCRIPTION	nvarchar(50)			NOT NULL
)
GO

CREATE UNIQUE CLUSTERED INDEX iOBJECT_STATE1 ON Common.OBJECT_STATE(ID)
GO
