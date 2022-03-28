if exists (select * from sys.objects where name='RESTRICTED_PASSWORD' and type='U')
	drop table Common.RESTRICTED_PASSWORD
GO

CREATE TABLE Common.RESTRICTED_PASSWORD(
	ID			int			NOT NULL identity(1,1),
	PASSWORD	varchar(50)	NOT NULL
)
GO

CREATE CLUSTERED INDEX iRESTRICTED_PASSWORD1 ON Common.RESTRICTED_PASSWORD(ID)
GO
CREATE UNIQUE INDEX iRESTRICTED_PASSWORD2 ON Common.RESTRICTED_PASSWORD(PASSWORD)
GO



create or alter procedure Common.sp_DeleteRestrictedPasswords
AS
	delete from Common.RESTRICTED_PASSWORD
GO



create or alter procedure Common.sp_SaveRestrictedPassword(@PASSWORD	varchar(50))
AS
	insert into Common.RESTRICTED_PASSWORD (PASSWORD)
		values (lower(@PASSWORD))
GO



create or alter procedure Common.sp_GetRestrictedPasswords
AS
	select PASSWORD from Common.RESTRICTED_PASSWORD
GO
