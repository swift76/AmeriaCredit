if exists (select * from sys.objects where name='sp_GetApplicationScoringResultByType' and type='P')
	drop procedure GL.sp_GetApplicationScoringResultByType
GO

create procedure GL.sp_GetApplicationScoringResultByType(@APPLICATION_ID	uniqueidentifier)
AS
	SELECT r.AMOUNT, r.INTEREST, t.TERM_FROM, t.TERM_TO, t.TEMPLATE_CODE, t.TEMPLATE_NAME,
		isnull(r.PREPAYMENT_AMOUNT,0) as PREPAYMENT_AMOUNT, isnull(r.PREPAYMENT_INTEREST,0) as PREPAYMENT_INTEREST
	FROM Common.APPLICATION_SCORING_RESULT r
	JOIN Common.APPLICATION a
		ON a.ID=r.APPLICATION_ID
	JOIN GL.AGREEMENT_TEMPLATE_BY_TYPE t
		ON a.LOAN_TYPE_ID=t.LOAN_TYPE_ID and a.CURRENCY_CODE=t.CURRENCY_CODE
			and t.WAY_ID=
				case
					when a.SOURCE_ID=3 then 3
					when a.HAS_BANK_CARD=1 then 1
					else 2
				end
	WHERE r.APPLICATION_ID = @APPLICATION_ID
	ORDER BY r.AMOUNT desc
GO