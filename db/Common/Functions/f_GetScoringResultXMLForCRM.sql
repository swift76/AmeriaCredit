if exists (select * from sys.objects where name='f_GetScoringResultXMLForCRM' and type='FN')
	drop function Common.f_GetScoringResultXMLForCRM
GO

CREATE FUNCTION Common.f_GetScoringResultXMLForCRM(@APPLICATION_ID	uniqueidentifier,
												   @LOAN_TYPE_ID	char(2),
												   @CURRENCY_CODE	char(3))
RETURNS nvarchar(max)
AS
BEGIN
	declare @Result nvarchar(max)
	select @Result=
	(
		SELECT r.SCORING_NUMBER, r.AMOUNT, r.INTEREST, t.TERM_FROM, t.TERM_TO, t.TEMPLATE_CODE, ltrim(rtrim(t.TEMPLATE_NAME)) as TEMPLATE_NAME
		FROM Common.APPLICATION_SCORING_RESULT r
		JOIN GL.AGREEMENT_TEMPLATE_BY_TYPE t
			ON t.LOAN_TYPE_ID=@LOAN_TYPE_ID
				and t.CURRENCY_CODE=@CURRENCY_CODE
				and t.WAY_ID=3
		WHERE r.APPLICATION_ID=@APPLICATION_ID
		ORDER BY r.AMOUNT desc
		FOR XML RAW('ScoringResult'),ROOT('ScoringResults')
	)
	return @Result
END
GO
