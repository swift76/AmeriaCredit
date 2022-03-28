if exists (select * from sys.objects where name='sp_SaveApplicationScoringResult' and type='P')
	drop procedure Common.sp_SaveApplicationScoringResult
GO

create procedure Common.sp_SaveApplicationScoringResult(@APPLICATION_ID			uniqueidentifier,
														@SCORING_NUMBER			tinyint,
														@AMOUNT					money,
														@INTEREST				money,
														@PREPAYMENT_AMOUNT		money,
														@PREPAYMENT_INTEREST	money)
AS
	insert into Common.APPLICATION_SCORING_RESULT
		(APPLICATION_ID,SCORING_NUMBER,AMOUNT,INTEREST,PREPAYMENT_AMOUNT,PREPAYMENT_INTEREST)
	values
		(@APPLICATION_ID,@SCORING_NUMBER,@AMOUNT,@INTEREST,@PREPAYMENT_AMOUNT,@PREPAYMENT_INTEREST)
GO
