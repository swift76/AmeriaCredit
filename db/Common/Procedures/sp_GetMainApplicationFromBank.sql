﻿CREATE OR ALTER PROCEDURE Common.sp_GetMainApplicationFromBank(@ID	uniqueidentifier)
AS
	select
		c.FINAL_AMOUNT,
		isnull(c.INTEREST,0) as INTEREST,
		c.PERIOD_TYPE_CODE,
		c.REPAY_DAY,
		isnull(c.FIRST_NAME_EN,'') as FIRST_NAME_EN,
		isnull(c.LAST_NAME_EN,'') as LAST_NAME_EN,
		coalesce(a.MOBILE_PHONE_1,c.MOBILE_PHONE_1,u.MOBILE_PHONE) as MOBILE_PHONE_1,
		isnull(c.MOBILE_PHONE_2,'') as MOBILE_PHONE_2,
		coalesce(c.FIXED_PHONE,u.FIXED_PHONE,'') as FIXED_PHONE,
		coalesce(c.EMAIL,u.EMAIL,'') as EMAIL,
		isnull(c.BIRTH_PLACE_CODE,'') as BIRTH_PLACE_CODE,
		isnull(c.CITIZENSHIP_CODE,'') as CITIZENSHIP_CODE,
		isnull(c.REGISTRATION_COUNTRY_CODE,'') as REGISTRATION_COUNTRY_CODE,
		isnull(c.REGISTRATION_STATE_CODE,'') as REGISTRATION_STATE_CODE,
		isnull(c.REGISTRATION_CITY_CODE,'') as REGISTRATION_COMMUNITY,
		isnull(Common.ahf_Unicode2ANSI(c.REGISTRATION_STREET),'') as REGISTRATION_STREET,
		isnull(Common.ahf_Unicode2ANSI(c.REGISTRATION_BUILDNUM),'') as REGISTRATION_BUILDNUM,
		isnull(Common.ahf_Unicode2ANSI(c.REGISTRATION_APARTMENT),'') as REGISTRATION_APARTMENT,
		isnull(c.CURRENT_COUNTRY_CODE,'') as CURRENT_COUNTRY_CODE,
		isnull(c.CURRENT_STATE_CODE,'') as CURRENT_STATE_CODE,
		isnull(c.CURRENT_CITY_CODE,'') as CURRENT_COMMUNITY,
		isnull(Common.ahf_Unicode2ANSI(c.CURRENT_STREET),'') as CURRENT_STREET,
		isnull(Common.ahf_Unicode2ANSI(c.CURRENT_BUILDNUM),'') as CURRENT_BUILDNUM,
		isnull(Common.ahf_Unicode2ANSI(c.CURRENT_APARTMENT),'') as CURRENT_APARTMENT,
		isnull(Common.ahf_Unicode2ANSI(c.COMPANY_NAME),'') as COMPANY_NAME,
		isnull(c.COMPANY_PHONE,'') as COMPANY_PHONE,
		isnull(Common.ahf_Unicode2ANSI(c.POSITION),'') as POSITION,
		isnull(c.MONTHLY_INCOME_CODE,'') as MONTHLY_INCOME_CODE,
		isnull(c.WORKING_EXPERIENCE_CODE,'') as WORKING_EXPERIENCE_CODE,
		isnull(c.FAMILY_STATUS_CODE,'') as FAMILY_STATUS_CODE,
		isnull(c.SHOP_CODE,'') as SHOP_CODE,
		coalesce(a.PRODUCT_CATEGORY_CODE,c.PRODUCT_CATEGORY_CODE,'') as PRODUCT_CATEGORY_CODE,
		isnull(c.PRODUCT_NUMBER,'') as PRODUCT_NUMBER,
		isnull(c.GOODS_RECEIVING_CODE,'') as GOODS_RECEIVING_CODE,
		isnull(c.GOODS_DELIVERY_ADDRESS,'') as GOODS_DELIVERY_ADDRESS,
		coalesce(a.LOAN_TEMPLATE_CODE,c.LOAN_TEMPLATE_CODE,'') as LOAN_TEMPLATE_CODE,
		isnull(c.OVERDRAFT_TEMPLATE_CODE,'') as OVERDRAFT_TEMPLATE_CODE,
		isnull(c.SHOP_SEQUENCE_NUMBER,'') as SHOP_SEQUENCE_NUMBER,
		isnull(ag.EXISTING_CARD_CODE,'') as EXISTING_CARD_CODE,
		isnull(ag.IS_NEW_CARD,0) as IS_NEW_CARD,
		isnull(ag.CREDIT_CARD_TYPE_CODE,'') as CREDIT_CARD_TYPE_CODE,
		isnull(ag.IS_CARD_DELIVERY,0) as IS_CARD_DELIVERY,
		isnull(Common.ahf_Unicode2ANSI(ag.CARD_DELIVERY_ADDRESS),'') as CARD_DELIVERY_ADDRESS,
		isnull(ag.BANK_BRANCH_CODE,'') as BANK_BRANCH_CODE,
		isnull(c.COMMUNICATION_TYPE_CODE,'') as COMMUNICATION_TYPE_CODE,
		isnull(ag.IS_ARBITRAGE_CHECKED,0) as IS_ARBITRAGE_CHECKED,
		isnull(c.IS_CURRENT_ADDRESS_SAME,0) as IS_CURRENT_ADDRESS_SAME,

		isnull(c.AUTO_BRAND_NAME,'') as AUTO_BRAND_NAME,
		isnull(c.AUTO_VIN_CODE,'') as AUTO_VIN_CODE,
		isnull(convert(varchar(4),c.AUTO_PRODUCTION_YEAR),'') as AUTO_PRODUCTION_YEAR,
		c.AUTO_OWNERSHIP_CERTIFCATE_DATE,
		isnull(c.AUTO_OWNERSHIP_CERTIFCATE_NUMBER,'') as AUTO_OWNERSHIP_CERTIFCATE_NUMBER,

		isnull(Common.ahf_Unicode2ANSI(c.ESTATE_ADDRESS),'') as ESTATE_ADDRESS,
		isnull(c.ESTATE_RESIDENTIAL_AREA,0) as ESTATE_RESIDENTIAL_AREA,
		isnull(c.ESTATE_LAND_AREA,0) as ESTATE_LAND_AREA,

		isnull(c.PLEDGE_PRICE,0) as PLEDGE_PRICE,
		isnull(c.INSURANCE_COMPANY_CODE,'') as INSURANCE_COMPANY_CODE,
		isnull(c.PLEDGE_TYPE_ACRA,'') as PLEDGE_TYPE_ACRA,
		isnull(c.PLEDGE_TYPE_LR,'') as PLEDGE_TYPE_LR,

		isnull(c.DEVELOPER_CODE,'') as DEVELOPER_CODE,
		isnull(c.DEVELOPER_DELTA,0) as DEVELOPER_DELTA,
		isnull(c.DEVELOPER_AMOUNT,0) as DEVELOPER_AMOUNT,
		isnull(c.DEVELOPER_INTEREST,0) as DEVELOPER_INTEREST,
		COMPLETION_DATE,

		isnull(c.PREPAID_AMOUNT,0) as PREPAID_AMOUNT,
		isnull(c.LTV,0) as LTV
	from Common.COMPLETED_APPLICATION c
	join Common.APPLICATION a
		on c.APPLICATION_ID=a.ID
	left join Common.CUSTOMER_USER u
		on a.CUSTOMER_USER_ID=u.APPLICATION_USER_ID
	left join GL.AGREED_APPLICATION ag
		on a.ID=ag.APPLICATION_ID
	where c.APPLICATION_ID = @ID
GO
