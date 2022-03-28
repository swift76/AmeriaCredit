create or alter procedure Common.sp_GetGLApplicationScoringResults(@ID	varchar(50))
AS
	select SCORING_OPTION,SCORING_AMOUNT,SCORING_COEFFICIENT,SCORING_INTEREST
	from Common.SCORING_RESULTS
	where APPLICATION_ID=@ID
GO
