if exists (select * from sys.objects where name='sp_SaveOnboardingCustomer' and type='P')
	drop procedure Common.sp_SaveOnboardingCustomer
GO

create procedure Common.sp_SaveOnboardingCustomer(
	@ID						uniqueidentifier,
	@first_name_eng			varchar(50),
	@last_name_eng			varchar(50),
	@mobile_number			varchar(20),
	@email					varchar(50),
	@birth_date				date,
	@first_name_arm			nvarchar(50),
	@last_name_arm			nvarchar(50),
	@middle_name_arm		nvarchar(50),
	@document_type_id		tinyint,
	@document_number		varchar(50),
	@soccard_number			varchar(50),
	@document_issue_date	date,
	@document_issuer		char(3),
	@is_student 			bit
)
AS
	insert into Common.ONBOARDING_CUSTOMER (ID,
		first_name_eng,
		last_name_eng,
		mobile_number,
		email,
		birth_date,
		first_name_arm,
		last_name_arm,
		middle_name_arm,
		document_type_id,
		document_number,
		soccard_number,
		document_issue_date,
		document_issuer,
		is_student)
	values (@ID,
		@first_name_eng,
		@last_name_eng,
		@mobile_number,
		@email,
		@birth_date,
		@first_name_arm,
		@last_name_arm,
		@middle_name_arm,
		@document_type_id,
		@document_number,
		@soccard_number,
		@document_issue_date,
		@document_issuer,
		@is_student)
GO
