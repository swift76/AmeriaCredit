alter table Common.CUSTOMER_USER_REGISTRATION_PROCESS
alter column
	VERIFICATION_CODE	varchar(6)         	NOT NULL
GO



alter table Common.CUSTOMER_USER_REGISTRATION_PROCESS
add
	TRY_COUNT			int					NOT NULL default 0,
	SMS_COUNT			int					NOT NULL default 1
GO



alter table Common.CUSTOMER_USER_PASSWORD_RESET_PROCESS
add
	TRY_COUNT			int					NOT NULL default 0
GO



insert into Common.SETTING (CODE, VALUE, DESCRIPTION)
values ('AUTHORIZATION_CODE_TRY_COUNT', '5', N'Նույնականացման կոդի վավերացման անհաջող փորձերի քանակ')
GO



create or alter procedure Common.sp_StartCustomerUserRegistrationProcess (
	@PROCESS_ID			uniqueidentifier,
	@VERIFICATION_CODE	varchar(6),
	@FIRST_NAME_EN		varchar(50),
	@LAST_NAME_EN		varchar(50),
	@SOCIAL_CARD_NUMBER	char(10),
	@MOBILE_PHONE		char(11),
	@EMAIL				varchar(50),
	@HASH				varchar(1000),
	@ONBOARDING_ID		uniqueidentifier = null,
	@IS_STUDENT			bit = null
)
AS
	insert into Common.CUSTOMER_USER_REGISTRATION_PROCESS
		(PROCESS_ID, VERIFICATION_CODE, FIRST_NAME_EN, LAST_NAME_EN, MOBILE_PHONE, EMAIL, SOCIAL_CARD_NUMBER, HASH, ONBOARDING_ID, IS_STUDENT)
	values
		(@PROCESS_ID, @VERIFICATION_CODE, @FIRST_NAME_EN, @LAST_NAME_EN, @MOBILE_PHONE, @EMAIL, @SOCIAL_CARD_NUMBER, @HASH, @ONBOARDING_ID, @IS_STUDENT)
GO



create or alter procedure Common.sp_GetCustomerUserRegistrationProcess(
	@PROCESS_ID	uniqueidentifier
)
AS
	select PROCESS_ID,
		VERIFICATION_CODE,
		FIRST_NAME_EN,
		LAST_NAME_EN,
		MOBILE_PHONE,
		EMAIL,
		SOCIAL_CARD_NUMBER,
		HASH,
		ONBOARDING_ID,
		IS_STUDENT,
		TRY_COUNT,
		SMS_COUNT
	from Common.CUSTOMER_USER_REGISTRATION_PROCESS
	where PROCESS_ID = @PROCESS_ID
GO



create or alter procedure Common.sp_UpdateCustomerUserRegistrationProcess(
	@PROCESS_ID			uniqueidentifier,
	@VERIFICATION_CODE	varchar(6))
AS
	if exists (select *
			   from Common.CUSTOMER_USER_REGISTRATION_PROCESS
			   where PROCESS_ID = @PROCESS_ID)
		update Common.CUSTOMER_USER_REGISTRATION_PROCESS
		set	VERIFICATION_CODE = @VERIFICATION_CODE,
			SMS_COUNT = SMS_COUNT+1
		where PROCESS_ID = @PROCESS_ID
GO



create or alter procedure Common.sp_SetTryCustomerUserRegistrationProcess(
	@PROCESS_ID uniqueidentifier
)
AS
	update Common.CUSTOMER_USER_REGISTRATION_PROCESS
	set TRY_COUNT = TRY_COUNT+1
	where PROCESS_ID = @PROCESS_ID
GO



create or alter procedure Common.sp_UpdateCustomerUserPassword(
	@PROCESS_ID             uniqueidentifier,
	@PHONE					varchar(15),
	@VERIFICATION_CODE_HASH varchar(1000),
	@PASSWORD_HASH 			varchar(1000)
)
AS
	IF EXISTS (
		SELECT *
		FROM Common.CUSTOMER_USER_PASSWORD_RESET_PROCESS
		WHERE PROCESS_ID = @PROCESS_ID
			AND HASH = @VERIFICATION_CODE_HASH
			AND PHONE = @PHONE
			AND EXPIRES_ON > GETDATE())
		UPDATE Common.APPLICATION_USER
		SET HASH = @PASSWORD_HASH
		WHERE LOGIN = @PHONE
	ELSE
	begin
		declare @TRY_COUNT int,@EXPIRES_ON DATETIME,@LIMIT_COUNT int

		SELECT @TRY_COUNT=TRY_COUNT,
			@EXPIRES_ON=EXPIRES_ON
		FROM Common.CUSTOMER_USER_PASSWORD_RESET_PROCESS
		WHERE PROCESS_ID = @PROCESS_ID
			AND PHONE = @PHONE

		select @LIMIT_COUNT=convert(int,VALUE)
		from Common.SETTING
		where CODE='AUTHORIZATION_CODE_TRY_COUNT'

		if @EXPIRES_ON>GETDATE() and @TRY_COUNT<@LIMIT_COUNT
			update Common.CUSTOMER_USER_PASSWORD_RESET_PROCESS
			set TRY_COUNT=TRY_COUNT+1
			WHERE PROCESS_ID = @PROCESS_ID
				AND PHONE = @PHONE;

		THROW 51000, N'Օգտագործողի նշանաբանը փոխելու ընթացքում սխալ է առաջացել', 1
	end
GO
