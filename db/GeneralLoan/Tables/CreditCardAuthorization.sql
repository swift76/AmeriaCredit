if exists (select * from sys.objects where name='CREDIT_CARD_AUTHORIZATION' and type='U')
	drop table GL.CREDIT_CARD_AUTHORIZATION
GO

CREATE TABLE GL.CREDIT_CARD_AUTHORIZATION (
	APPLICATION_ID	uniqueidentifier	NOT NULL,
	SMS_HASH		varchar(1000)		NOT NULL,
	SMS_SENT_DATE	datetime			NOT NULL default getdate(),
	TRY_COUNT		int					NOT NULL default 0,
	SMS_COUNT		int					NOT NULL default 1
)
GO

CREATE UNIQUE CLUSTERED INDEX iCREDIT_CARD_AUTHORIZATION1 ON GL.CREDIT_CARD_AUTHORIZATION (APPLICATION_ID)
GO
