if exists (select * from sys.objects where name='SCORING_RESULT' and type='U')
	drop table Common.SCORING_RESULT
GO

CREATE TABLE Common.SCORING_RESULT(
	QUERY_DATE			datetime			NOT NULL default getdate(),
	APPLICATION_ID		uniqueidentifier	NOT NULL,
	SCORING_AMOUNT		money				NOT NULL,
	SCORING_COEFFICIENT	money				NOT NULL
)
GO

CREATE UNIQUE CLUSTERED INDEX iSCORING_RESULT1 ON Common.SCORING_RESULT (APPLICATION_ID)
GO
