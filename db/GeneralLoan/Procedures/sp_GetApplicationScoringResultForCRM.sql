if exists (select * from sys.objects where name='sp_GetApplicationScoringResultForCRM' and type='P')
	drop procedure GL.sp_GetApplicationScoringResultForCRM
GO

create procedure GL.sp_GetApplicationScoringResultForCRM(@APPLICATION_ID	uniqueidentifier)
AS
	SELECT STATUS,REFUSAL_REASON,MANUAL_REASON,
		Common.f_GetScoringResultXMLForCRM(ID,LOAN_TYPE_ID,CURRENCY_CODE) AS SCORING_RESULT
	FROM Common.APPLICATION
	WHERE ID = @APPLICATION_ID
GO