if exists (select * from sys.objects where name='BANK_APPLICATION_OPERATION_LOG' and type='U')
	drop table IL.BANK_APPLICATION_OPERATION_LOG
GO

CREATE TABLE IL.BANK_APPLICATION_OPERATION_LOG (
	ID 					int IDENTITY(1,1)	NOT NULL,
	DATE				datetime			NOT NULL default getdate(),
	APPLICATION_USER_ID	int					NOT NULL,
	BANK_OBJECT_CODE	varchar(20)			NOT NULL,
	BANK_OPERATION_CODE	varchar(20)			NOT NULL,
	ENTITY_ID			int					NULL,
	OPERATION_DETAILS	nvarchar(max)		NOT NULL
)
GO

CREATE CLUSTERED INDEX iBANK_APPLICATION_OPERATION_LOG1 ON IL.BANK_APPLICATION_OPERATION_LOG(ID)
GO

CREATE INDEX iBANK_APPLICATION_OPERATION_LOG2 ON IL.BANK_APPLICATION_OPERATION_LOG(DATE,APPLICATION_USER_ID)
GO

CREATE INDEX iBANK_APPLICATION_OPERATION_LOG3 ON IL.BANK_APPLICATION_OPERATION_LOG(BANK_OBJECT_CODE,ENTITY_ID)
GO
