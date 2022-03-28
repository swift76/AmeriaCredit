if exists (select * from sys.objects where name='sp_GetOnboardingCustomer' and type='P')
	drop procedure Common.sp_GetOnboardingCustomer
GO

create procedure Common.sp_GetOnboardingCustomer(@ID uniqueidentifier)
AS
	declare @CurrentDate datetime = getdate()
	select
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
		isnull(is_student,0) as is_student
	from Common.ONBOARDING_CUSTOMER with (nolock)
	where ID=@ID and CREATION_DATE>=dateadd(day,-2,@CurrentDate)
GO
