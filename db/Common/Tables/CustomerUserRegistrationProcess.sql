if exists (select * from sys.objects where name='CUSTOMER_USER_REGISTRATION_PROCESS' and type='U')
	drop table Common.CUSTOMER_USER_REGISTRATION_PROCESS
GO

CREATE TABLE Common.CUSTOMER_USER_REGISTRATION_PROCESS(
    PROCESS_ID			uniqueidentifier	NOT NULL PRIMARY KEY,
	VERIFICATION_CODE	varchar(6)         	NOT NULL,
	FIRST_NAME_EN		varchar(20)			NOT NULL,
	LAST_NAME_EN		varchar(20)			NOT NULL,
	MOBILE_PHONE		varchar(20)			NOT NULL,
	EMAIL				varchar(70)			NOT NULL,
	SOCIAL_CARD_NUMBER	char(10)			NOT NULL,
	HASH             	varchar(1000)		NOT NULL,
	ONBOARDING_ID		uniqueidentifier	NULL,
	IS_STUDENT			bit					NULL,
	TRY_COUNT			int					NOT NULL default 0,
	SMS_COUNT			int					NOT NULL default 1
)
GO
