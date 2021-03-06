if exists (select * from sys.objects where name='MOBILE_PHONE_AUTHORIZATION' and type='U')
	drop table IL.MOBILE_PHONE_AUTHORIZATION
GO

CREATE TABLE IL.MOBILE_PHONE_AUTHORIZATION (
	APPLICATION_ID	uniqueidentifier	NOT NULL,
	SMS_HASH		varchar(1000)		NOT NULL,
	SMS_SENT_DATE	datetime			NOT NULL default getdate(),
	SMS_COUNT		int					NOT NULL default 1
)
GO

CREATE UNIQUE CLUSTERED INDEX iMOBILE_PHONE_AUTHORIZATION1 ON IL.MOBILE_PHONE_AUTHORIZATION (APPLICATION_ID)
GO
