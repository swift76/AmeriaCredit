if exists (select * from sys.objects where name='AM0_APPLICATION_PRINT' and type='U')
	drop table dbo.AM0_APPLICATION_PRINT
GO

CREATE TABLE AM0_APPLICATION_PRINT (
	APPLICATION_ID				uniqueidentifier	NOT NULL,
	APPLICATION_PRINT_TYPE_ID	tinyint				NOT NULL,
	CONTENT						varbinary(max)		NULL
)
GO

CREATE UNIQUE CLUSTERED INDEX iAM0_APPLICATION_PRINT1 ON dbo.AM0_APPLICATION_PRINT(APPLICATION_ID, APPLICATION_PRINT_TYPE_ID)
GO
