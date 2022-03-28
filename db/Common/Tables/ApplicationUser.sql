if exists (select * from sys.objects where name='APPLICATION_USER' and type='U')
	drop table Common.APPLICATION_USER
GO

CREATE TABLE Common.APPLICATION_USER (
	ID						int				identity(1,1),
	LOGIN					varchar(50)		NOT NULL,
	FIRST_NAME				nvarchar(50)	NOT NULL,
	LAST_NAME				nvarchar(50)	NOT NULL,
	HASH					varchar(1000)	NOT NULL,
	EMAIL					varchar(70)		NOT NULL,
	CREATE_DATE				datetime		NOT NULL default getdate(),
	PASSWORD_EXPIRY_DATE	date			NULL,
	CLOSE_DATE				datetime		NULL,
	OBJECT_STATE_ID			tinyint			NOT NULL,
	USER_ROLE_ID			tinyint			NOT NULL
)
GO

CREATE UNIQUE CLUSTERED INDEX iAPPLICATION_USER1 ON Common.APPLICATION_USER(ID)
GO

CREATE UNIQUE INDEX iAPPLICATION_USER2 ON Common.APPLICATION_USER(USER_ROLE_ID,LOGIN)
GO
