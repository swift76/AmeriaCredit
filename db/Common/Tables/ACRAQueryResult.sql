if exists (select * from sys.objects where name='ACRA_QUERY_RESULT' and type='U')
	drop table Common.ACRA_QUERY_RESULT
GO

CREATE TABLE Common.ACRA_QUERY_RESULT(
	QUERY_DATE			datetime			NOT NULL default getdate(),
	APPLICATION_ID		uniqueidentifier	NOT NULL,
	FICO_SCORE			char(3)				NOT NULL,
	RESPONSE_XML		nvarchar(max)		NULL
)
GO

CREATE UNIQUE CLUSTERED INDEX iACRA_QUERY_RESULT1 ON Common.ACRA_QUERY_RESULT (APPLICATION_ID)
GO
