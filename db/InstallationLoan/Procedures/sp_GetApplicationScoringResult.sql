if exists (select * from sys.objects where name='sp_GetApplicationScoringResult' and type='P')
	drop procedure IL.sp_GetApplicationScoringResult
GO

create procedure IL.sp_GetApplicationScoringResult(@APPLICATION_ID	uniqueidentifier)
AS
	SELECT AMOUNT,
		   INTEREST,
		   isnull(PREPAYMENT_AMOUNT, 0) as PREPAYMENT_AMOUNT,
		   isnull(PREPAYMENT_INTEREST, 0) as PREPAYMENT_INTEREST
	FROM Common.APPLICATION_SCORING_RESULT
	WHERE APPLICATION_ID = @APPLICATION_ID
GO
