if exists (select * from sys.objects where name='sp_GetOnboardingCustomer' and type='P')
	drop procedure Common.sp_GetOnboardingCustomer
GO

create procedure Common.sp_GetOnboardingCustomer(@ID uniqueidentifier)
AS
	select
		'SILVI' as first_name_eng,
		'HAMBARDZUMYAN' as last_name_eng,
		'55555555' as mobile_number,
		'silvihambardzumyan5555@gmail.com' as email,
		'1999-01-01' as birth_date,
		N'ՍԻԼՎԻ' as first_name_arm,
		N'ՀԱՄԲԱՐՁՈՒՄՅԱՆ' as last_name_arm,
		N'ՀԱՄԲԱՐՁՈՒՄԻ' as middle_name_arm,
		2 as document_type_id,
		'012345678' as document_number,
		'5101995555' as soccard_number,
		convert(date,'2019-12-20') as document_issue_date,
		'001' as document_issuer,
		1 as is_student
GO
