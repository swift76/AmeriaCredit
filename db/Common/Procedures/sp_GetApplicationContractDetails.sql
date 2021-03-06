if exists (select * from sys.objects where name='sp_GetApplicationContractDetails' and type='P')
	drop procedure Common.sp_GetApplicationContractDetails
GO

create procedure Common.sp_GetApplicationContractDetails(@ID	uniqueidentifier)
AS
select a.CREATION_DATE,
	a.CLIENT_CODE,
	a.FIRST_NAME,
	a.LAST_NAME,
	a.PATRONYMIC_NAME,
	m.FIRST_NAME_EN,
	m.LAST_NAME_EN,
	a.DOCUMENT_NUMBER,
	a.DOCUMENT_GIVEN_BY,
	a.DOCUMENT_GIVEN_DATE,
	a.DOCUMENT_EXPIRY_DATE,
	a.SOCIAL_CARD_NUMBER,
	a.BIRTH_DATE,
	bc.NAME_AM as BIRTH_PLACE_NAME,
	nc.NAME_AM as CITIZENSHIP_COUNTRY_NAME,
	f.NAME_AM as FAMILY_STATUS,
	rc.NAME_AM as REGISTRATION_COUNTRY_NAME,
	rcty.NAME_AM as REGISTRATION_CITY_NAME,
	rst.NAME_AM as REGISTRATION_STATE_NAME,
	m.REGISTRATION_STREET,
	m.REGISTRATION_BUILDNUM,
	m.REGISTRATION_APARTMENT,
	cc.NAME_AM as CURRENT_COUNTRY_NAME,
	ccty.NAME_AM as CURRENT_CITY_NAME,
	cst.NAME_AM as CURRENT_STATE_NAME,
	m.CURRENT_STREET,
	m.CURRENT_BUILDNUM,
	m.CURRENT_APARTMENT,
	m.FIXED_PHONE,
	m.MOBILE_PHONE_1,
	m.MOBILE_PHONE_2,
	m.EMAIL,
	m.COMPANY_NAME,
	oa.NAME_AM as ORGANIZATION_ACTIVITY_NAME,
	m.COMPANY_PHONE,
	m.POSITION,
	we.NAME_AM as WORKING_EXPERIENCE_NAME,
	sal.NAME_AM as MONTHLY_INCOME_NAME,
	a.INITIAL_AMOUNT,
	m.REPAY_DAY,
	m.FINAL_AMOUNT,
	m.PERIOD_TYPE_CODE as PERIOD_TYPE_NAME,
	m.INTEREST,
	ll.NAME_AM as CURRENCY_NAME,
	coalesce(aa.IS_NEW_CARD, 0) as IS_NEW_CARD,
	case when coalesce(aa.IS_NEW_CARD, 0) = 0 then aa.EXISTING_CARD_CODE else null end as EXISTING_CARD_CODE,
	case when coalesce(aa.IS_NEW_CARD, 0) = 1 then ct.NAME_AM else null end as NEW_CARD_TYPE_NAME,
	case when coalesce(aa.IS_NEW_CARD, 0) = 1 and coalesce(aa.IS_CARD_DELIVERY, 0) = 0 then bb.NAME_AM else null end as CARD_DELIVERY_BANK_BRANCH_NAME,
	case when coalesce(aa.IS_NEW_CARD, 0) = 1 and coalesce(aa.IS_CARD_DELIVERY, 0) = 1 then aa.CARD_DELIVERY_ADDRESS else null end as CARD_DELIVERY_ADDRESS
from Common.COMPLETED_APPLICATION m
join Common.APPLICATION a
	on m.APPLICATION_ID = a.ID
join Common.COUNTRY rc
	on m.REGISTRATION_COUNTRY_CODE = rc.CODE
join Common.STATE rst
	on m.REGISTRATION_STATE_CODE = rst.CODE
join Common.CITY rcty
	on m.REGISTRATION_CITY_CODE = rcty.CODE
join Common.COUNTRY cc
	on m.CURRENT_COUNTRY_CODE = cc.CODE
join Common.STATE cst
	on m.CURRENT_STATE_CODE = cst.CODE
join Common.CITY ccty
	on m.CURRENT_CITY_CODE = ccty.CODE
join Common.COUNTRY nc
	on m.CITIZENSHIP_CODE = nc.CODE
join Common.COUNTRY bc
	on m.BIRTH_PLACE_CODE = bc.CODE
join GL.FAMILY_STATUS f
	on m.FAMILY_STATUS_CODE = f.CODE
join Common.ORGANIZATION_ACTIVITY oa
	on a.ORGANIZATION_ACTIVITY_CODE = oa.CODE
join Common.LOAN_LIMIT ll
	on a.LOAN_TYPE_ID = ll.LOAN_TYPE_CODE and a.CURRENCY_CODE = ll.CURRENCY
left join GL.WORKING_EXPERIENCE we
	on m.WORKING_EXPERIENCE_CODE = we.CODE
left join Common.MONTHLY_NET_SALARY sal
	on m.MONTHLY_INCOME_CODE = sal.CODE
left join GL.AGREED_APPLICATION aa
left join GL.BANK_BRANCH bb
	on aa.BANK_BRANCH_CODE = bb.CODE
	on a.ID = aa.APPLICATION_ID
left join GL.CREDIT_CARD_TYPE ct
	on a.LOAN_TYPE_ID = ct.LOAN_TYPE_ID and aa.CREDIT_CARD_TYPE_CODE = ct.CODE
where m.APPLICATION_ID = @ID
GO
