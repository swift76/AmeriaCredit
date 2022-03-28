create or alter procedure Common.sp_GetILApplicationScoringResult(@ID	varchar(50))
AS
	select SCORING_AMOUNT,SCORING_COEFFICIENT
	from Common.SCORING_RESULT
	where APPLICATION_ID=@ID
GO
