if exists (select * from sys.objects where name='LOAN_STATUS' and type='U')
	drop table Common.LOAN_STATUS
GO

CREATE TABLE Common.LOAN_STATUS (
	APPLICATION_ID 			uniqueidentifier	NOT NULL,
	STATE					tinyint				NOT NULL,
	LOAN_AGREEMENT_CODE		nvarchar(14)		NULL,
	PLEDGE_AGREEMENT_CODE	nvarchar(14)		NULL,
	ERROR_MESSAGE			nvarchar(1000)		NULL
)
GO

CREATE UNIQUE CLUSTERED INDEX iLOAN_STATUS1 ON Common.LOAN_STATUS(APPLICATION_ID)
GO
