CREATE OR ALTER PROCEDURE Common.sp_GetPersonNewApplicationAmount(@LOAN_TYPE_ID			char(2),
													 @SOCIAL_CARD_NUMBER	char(10),
													 @DATE_FROM				date,
													 @DATE_TO				date)
AS
	select isnull(sum(isnull(c.FINAL_AMOUNT,a.INITIAL_AMOUNT)),0) as AMOUNT
	from Common.APPLICATION a with (NOLOCK)
	left join Common.COMPLETED_APPLICATION as c with (NOLOCK)
		on a.ID = c.APPLICATION_ID
	where a.LOAN_TYPE_ID = @LOAN_TYPE_ID
		and a.SOCIAL_CARD_NUMBER = @SOCIAL_CARD_NUMBER
		and convert(date,a.CREATION_DATE) between @DATE_FROM and @DATE_TO
		and a.STATUS in (5,10,15,20,21,22)
GO
