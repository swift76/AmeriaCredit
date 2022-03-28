if exists (select * from sys.objects where name='sp_SaveInitialApplicationFromCRM' and type='P')
	drop procedure Common.sp_SaveInitialApplicationFromCRM
GO

create procedure Common.sp_SaveInitialApplicationFromCRM(@ID							uniqueidentifier,
														 @LOAN_TYPE_ID					char(2),
														 @FIRST_NAME					nvarchar(20),
														 @LAST_NAME						nvarchar(20),
														 @PATRONYMIC_NAME				nvarchar(20),
														 @BIRTH_DATE					date,
														 @SOCIAL_CARD_NUMBER			char(10),
														 @DOCUMENT_TYPE_CODE			char(1),
														 @DOCUMENT_NUMBER				char(9),
														 @DOCUMENT_GIVEN_BY				char(3),
														 @DOCUMENT_GIVEN_DATE			date,
														 @DOCUMENT_EXPIRY_DATE			date,
														 @INITIAL_AMOUNT				money,
														 @CURRENCY_CODE					char(3),
														 @ORGANIZATION_ACTIVITY_CODE	char(2))
AS
	insert into Common.APPLICATION
		(CREATION_DATE, ID, SOURCE_ID, LOAN_TYPE_ID, STATUS, INITIAL_AMOUNT, CURRENCY_CODE, CUSTOMER_USER_ID, SHOP_USER_ID, TO_BE_SYNCHRONIZED
			,FIRST_NAME, LAST_NAME, PATRONYMIC_NAME, BIRTH_DATE, SOCIAL_CARD_NUMBER, ORGANIZATION_ACTIVITY_CODE, MOBILE_PHONE_1
			,DOCUMENT_TYPE_CODE, DOCUMENT_NUMBER,DOCUMENT_GIVEN_BY, DOCUMENT_GIVEN_DATE, DOCUMENT_EXPIRY_DATE, PRODUCT_CATEGORY_CODE, LOAN_TEMPLATE_CODE
			,IS_REFINANCING)
	values
		(getdate(), @ID, '4', @LOAN_TYPE_ID, 1, @INITIAL_AMOUNT, @CURRENCY_CODE, null, null, 1
			,@FIRST_NAME, @LAST_NAME, @PATRONYMIC_NAME, @BIRTH_DATE, @SOCIAL_CARD_NUMBER, @ORGANIZATION_ACTIVITY_CODE, null
			,@DOCUMENT_TYPE_CODE, @DOCUMENT_NUMBER, @DOCUMENT_GIVEN_BY, @DOCUMENT_GIVEN_DATE, @DOCUMENT_EXPIRY_DATE, null, null
			,0)
GO
