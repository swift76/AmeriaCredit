if exists (select * from sys.objects where name='LOAN_LIMIT' and type='U')
	drop table Common.LOAN_LIMIT
GO

CREATE TABLE Common.LOAN_LIMIT (
	LOAN_TYPE_CODE	char(2)			NOT NULL,
	CURRENCY		char(3)			NOT NULL,
	NAME_AM			nvarchar(50)	NOT NULL,
	NAME_EN			varchar(50)		NOT NULL,
	FROM_AMOUNT		money			NULL,
	TO_AMOUNT		money			NULL
)
GO

CREATE UNIQUE CLUSTERED INDEX iLOAN_LIMIT1 ON Common.LOAN_LIMIT(LOAN_TYPE_CODE,CURRENCY)
GO
