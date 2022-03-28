if exists (select * from sys.objects where name='sp_SaveInitialApplicationFromBank' and type='P')
	drop procedure Common.sp_SaveInitialApplicationFromBank
GO

create procedure Common.sp_SaveInitialApplicationFromBank(
	@LOAN_TYPE_ID				char(2),
	@FIRST_NAME					nvarchar(20),
	@LAST_NAME					nvarchar(20),
	@PATRONYMIC_NAME			nvarchar(20),
	@BIRTH_DATE					date,
	@SOCIAL_CARD_NUMBER			char(10),
	@DOCUMENT_TYPE_CODE			char(1),
	@DOCUMENT_NUMBER			char(9),
	@DOCUMENT_GIVEN_BY			char(3),
	@DOCUMENT_GIVEN_DATE		date,
	@DOCUMENT_EXPIRY_DATE		date,
	@INITIAL_AMOUNT				money,
	@CURRENCY_CODE				char(3),
	@ORGANIZATION_ACTIVITY_CODE	char(2)	= null,
	@CUSTOMER_USER_ID			int,
	@CLIENT_CODE				char(8),
	@ISN						int,
	@IMPORT_ID					int,
	@IS_REFINANCING				bit,
	@UNIVERSITY_CODE			char(2) = NULL,
	@UNIVERSITY_FACULTY			nvarchar(50) = NULL,
	@UNIVERSITY_YEAR			nvarchar(50) = NULL
)
AS
	declare @ID uniqueidentifier = newid()

	insert into Common.APPLICATION
		(CREATION_DATE, ID, SOURCE_ID, LOAN_TYPE_ID, INITIAL_AMOUNT, CURRENCY_CODE, CUSTOMER_USER_ID, ISN, CLIENT_CODE, STATUS, IMPORT_ID, IS_REFINANCING
			,FIRST_NAME,LAST_NAME,PATRONYMIC_NAME,BIRTH_DATE,SOCIAL_CARD_NUMBER,ORGANIZATION_ACTIVITY_CODE
			,DOCUMENT_TYPE_CODE,DOCUMENT_NUMBER,DOCUMENT_GIVEN_BY,DOCUMENT_GIVEN_DATE,DOCUMENT_EXPIRY_DATE
			,UNIVERSITY_CODE,UNIVERSITY_FACULTY,UNIVERSITY_YEAR)
	values
		(getdate(), @ID, 3, @LOAN_TYPE_ID, @INITIAL_AMOUNT, @CURRENCY_CODE, @CUSTOMER_USER_ID, @ISN, @CLIENT_CODE, 1, @IMPORT_ID, @IS_REFINANCING
			,Common.ahf_ANSI2Unicode(@FIRST_NAME),Common.ahf_ANSI2Unicode(@LAST_NAME),Common.ahf_ANSI2Unicode(@PATRONYMIC_NAME),@BIRTH_DATE,@SOCIAL_CARD_NUMBER,@ORGANIZATION_ACTIVITY_CODE
			,@DOCUMENT_TYPE_CODE,@DOCUMENT_NUMBER,@DOCUMENT_GIVEN_BY,@DOCUMENT_GIVEN_DATE,@DOCUMENT_EXPIRY_DATE
			,@UNIVERSITY_CODE,@UNIVERSITY_FACULTY,@UNIVERSITY_YEAR)

	select @ID
GO
