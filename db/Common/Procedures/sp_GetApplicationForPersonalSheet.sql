if exists (select * from sys.objects where name='sp_GetApplicationForPersonalSheet' and type='P')
	drop procedure Common.sp_GetApplicationForPersonalSheet
GO

create procedure Common.sp_GetApplicationForPersonalSheet(@ID	uniqueidentifier)
AS
	declare @CurrentDate date = getdate()
	declare @TEMPLATE_CODE char(4)
	SELECT @TEMPLATE_CODE=t.TEMPLATE_CODE
	FROM Common.APPLICATION a
	JOIN GL.AGREEMENT_TEMPLATE_BY_TYPE t
		ON a.LOAN_TYPE_ID=t.LOAN_TYPE_ID and a.CURRENCY_CODE=t.CURRENCY_CODE
			and t.WAY_ID=
				case
					when a.SOURCE_ID=3 then 3
					when a.HAS_BANK_CARD=1 then 1
					else 2
				end
	WHERE a.ID = @ID

	select  a.ID,
			a.CREATION_DATE as DATE,
		 	t.NAME_AM as LOAN_TYPE,
		 	CONVERT(varchar, c.FINAL_AMOUNT, 1) + ' ' + a.CURRENCY_CODE as LOAN_AMOUNT,
		 	ltrim(rtrim(c.PERIOD_TYPE_CODE)) as LOAN_DURATION,
		 	c.INTEREST as LOAN_INTEREST,
		 	rtrim(a.FIRST_NAME) + ' ' + rtrim(a.LAST_NAME) as CUSTOMER_NAME,
		 	case st.CODE
		 		when '001' then st.NAME_AM
		 		else cm.NAME_AM
		 	end +
	 		', ' + c.REGISTRATION_STREET +
	 		', ' + c.REGISTRATION_BUILDNUM +
	 		case when isnull(c.REGISTRATION_APARTMENT,'')='' then '' else ', ' + c.REGISTRATION_APARTMENT end
		 	as CUSTOMER_ADDRESS,
		 	c.MOBILE_PHONE_1 as CUSTOMER_PHONE,
		 	c.EMAIL as CUSTOMER_EMAIL,

			Common.f_GetServiceAmount(c.FINAL_AMOUNT,sc1.AMOUNT,sc1.INTEREST,sc1.MIN_AMOUNT,sc1.MAX_AMOUNT) as OTHER_PAYMENTS_LOAN_SERVICE_FEE,
			Common.f_GetServiceAmount(c.FINAL_AMOUNT,sc2.AMOUNT,sc2.INTEREST,sc2.MIN_AMOUNT,sc2.MAX_AMOUNT) as OTHER_PAYMENTS_CARD_SERVICE_FEE,
			Common.f_GetServiceAmount(c.FINAL_AMOUNT,sc3.AMOUNT,sc3.INTEREST,sc3.MIN_AMOUNT,sc3.MAX_AMOUNT) as OTHER_PAYMENTS_CASH_OUT_FEE,
			Common.f_GetServiceAmount(c.FINAL_AMOUNT,sc4.AMOUNT,sc4.INTEREST,sc4.MIN_AMOUNT,sc4.MAX_AMOUNT) as OTHER_PAYMENTS_PROVISION_FEE,
			Common.f_GetServiceAmount(c.FINAL_AMOUNT,sc9.AMOUNT,sc9.INTEREST,sc9.MIN_AMOUNT,sc9.MAX_AMOUNT) as OTHER_PAYMENTS_OTHER_FEE,

			@CurrentDate as SIGNATURE_DATE,
			@CurrentDate as SIGNATURE_DATE1,
			@CurrentDate as SIGNATURE_DATE2,

			t.IS_OVERDRAFT,
			@TEMPLATE_CODE as TEMPLATE_CODE,
			c.FINAL_AMOUNT,
		 	a.CURRENCY_CODE,
			c.REPAY_DAY,
			a.LOAN_TYPE_ID,
			isnull(ag.CREDIT_CARD_TYPE_CODE,'') as CREDIT_CARD_TYPE_CODE,
			case t.IS_CARD_ACCOUNT
				when 0 then N'ՍՊԱՌՈՂԱԿԱՆ ՎԱՐԿԻ'
				else N'ՎԱՐԿԱՅԻՆ ԳԾԻ (ՕՎԵՐԴՐԱՖՏԻ)'
			end + N' ԷԱԿԱՆ ՊԱՅՄԱՆՆԵՐԻ ԱՆՀԱՏԱԿԱՆ ԹԵՐԹԻԿ' as FORM_CAPTION
	from Common.APPLICATION as a
	join Common.COMPLETED_APPLICATION as c
		on a.ID = c.APPLICATION_ID
	join Common.LOAN_TYPE t
		on t.CODE = a.LOAN_TYPE_ID
	join Common.STATE st
		on st.CODE = c.REGISTRATION_STATE_CODE
	join Common.CITY cm
		on cm.CODE = c.REGISTRATION_CITY_CODE
	left join GL.AGREED_APPLICATION ag
		on a.ID=ag.APPLICATION_ID
	left join Common.LOAN_SERVICE_CONDITION sc1
		on sc1.SERVICE_TYPE_CODE='1' and sc1.LOAN_TYPE_CODE = a.LOAN_TYPE_ID and sc1.CURRENCY = a.CURRENCY_CODE
			and isnull(sc1.CREDIT_CARD_TYPE_CODE,'') in (isnull(ag.CREDIT_CARD_TYPE_CODE,''),'')
	left join Common.LOAN_SERVICE_CONDITION sc2
		on sc2.SERVICE_TYPE_CODE='2' and sc2.LOAN_TYPE_CODE = a.LOAN_TYPE_ID and sc2.CURRENCY = a.CURRENCY_CODE
			and isnull(sc2.CREDIT_CARD_TYPE_CODE,'') in (isnull(ag.CREDIT_CARD_TYPE_CODE,''),'')
	left join Common.LOAN_SERVICE_CONDITION sc3
		on sc3.SERVICE_TYPE_CODE='3' and sc3.LOAN_TYPE_CODE = a.LOAN_TYPE_ID and sc3.CURRENCY = a.CURRENCY_CODE
			and isnull(sc3.CREDIT_CARD_TYPE_CODE,'') in (isnull(ag.CREDIT_CARD_TYPE_CODE,''),'')
	left join Common.LOAN_SERVICE_CONDITION sc4
		on sc4.SERVICE_TYPE_CODE='4' and sc4.LOAN_TYPE_CODE = a.LOAN_TYPE_ID and sc4.CURRENCY = a.CURRENCY_CODE
			and isnull(sc4.CREDIT_CARD_TYPE_CODE,'') in (isnull(ag.CREDIT_CARD_TYPE_CODE,''),'')
	left join Common.LOAN_SERVICE_CONDITION sc9
		on sc9.SERVICE_TYPE_CODE='9' and sc9.LOAN_TYPE_CODE = a.LOAN_TYPE_ID and sc9.CURRENCY = a.CURRENCY_CODE
			and isnull(sc9.CREDIT_CARD_TYPE_CODE,'') in (isnull(ag.CREDIT_CARD_TYPE_CODE,''),'')
	where ID = @ID
GO
