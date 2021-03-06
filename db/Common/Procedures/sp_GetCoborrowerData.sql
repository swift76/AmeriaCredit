if exists (select * from sys.objects where name='sp_GetCoborrowerData' and type='P')
	drop procedure Common.sp_GetCoborrowerData
GO

create procedure Common.sp_GetCoborrowerData(@ID uniqueidentifier)
AS
	select a.COBORROWER_SOCIAL_CARD_NUMBER as SOCIAL_CARD_NUMBER,
		a.COBORROWER_FIRST_NAME as FIRST_NAME,
		a.COBORROWER_LAST_NAME as LAST_NAME,
		n.PATRONYMIC_NAME,
		a.COBORROWER_BIRTH_DATE as BIRTH_DATE,
		a.COBORROWER_FIRST_NAME_EN as FIRST_NAME_EN,
		a.COBORROWER_LAST_NAME_EN as LAST_NAME_EN,
		a.COBORROWER_MOBILE_PHONE as MOBILE_PHONE,
		a.COBORROWER_EMAIL as EMAIL,
		a.COBORROWER_BIRTH_PLACE_CODE as BIRTH_PLACE_CODE,
		a.COBORROWER_CITIZENSHIP_CODE as CITIZENSHIP_CODE,
		n.GENDER,
		n.DISTRICT,
		n.COMMUNITY,
		n.STREET,
		n.BUILDING,
		n.APARTMENT,
		n.FEE as CURRENT_SALARY,
		n.PASSPORT_NUMBER,
		n.PASSPORT_DATE,
		n.PASSPORT_EXPIRY_DATE,
		n.PASSPORT_BY,
		n.BIOMETRIC_PASSPORT_NUMBER,
		n.BIOMETRIC_PASSPORT_ISSUE_DATE,
		n.BIOMETRIC_PASSPORT_EXPIRY_DATE,
		n.BIOMETRIC_PASSPORT_ISSUED_BY,
		n.ID_CARD_NUMBER,
		n.ID_CARD_ISSUE_DATE,
		n.ID_CARD_EXPIRY_DATE,
		n.ID_CARD_ISSUED_BY,
		n.SOCIAL_PAYMENT
	from Common.APPLICATION a with (NOLOCK)
	join Common.NORQ_COBORROWER_QUERY_RESULT n with (NOLOCK)
		on n.APPLICATION_ID = a.ID
	where a.ID=@ID
GO
