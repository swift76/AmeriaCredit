if exists (select * from sys.objects where name='USER_ROLE' and type='U')
	drop table Common.USER_ROLE
GO

CREATE TABLE Common.USER_ROLE (
	ID 			tinyint IDENTITY(1,1)	NOT NULL,
	DESCRIPTION	nvarchar(50)			NOT NULL
)
GO

CREATE UNIQUE CLUSTERED INDEX iUSER_ROLE1 ON Common.USER_ROLE(ID)
GO
