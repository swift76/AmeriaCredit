create or alter procedure GL.sp_GetAgreedApplication(@ID	uniqueidentifier)
AS
	select  aa.EXISTING_CARD_CODE,
			aa.IS_NEW_CARD,
			aa.CREDIT_CARD_TYPE_CODE,
			aa.IS_CARD_DELIVERY,
			aa.CARD_DELIVERY_ADDRESS,
			aa.BANK_BRANCH_CODE,
			aa.IS_ARBITRAGE_CHECKED,
			a.STATUS as STATUS_ID,
			a.LOAN_TYPE_ID,
			convert(bit,case a.LOAN_TYPE_ID
				when 13 then 0
				else 1
			end) as AGREED_WITH_TERMS,
			aa.ACTUAL_INTEREST
	from GL.AGREED_APPLICATION aa
	join Common.APPLICATION a
		on aa.APPLICATION_ID = a.ID
	where aa.APPLICATION_ID = @ID
GO
