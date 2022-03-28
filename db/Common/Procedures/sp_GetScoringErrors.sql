CREATE OR ALTER PROCEDURE Common.sp_GetScoringErrors(@FROM_DATE	date,
										@TO_DATE	date)
AS
	select
		convert(char(19),DATE,121) as DATE,
		APPLICATION_ID as ID,
		OPERATION,
		Common.ahf_Unicode2ANSI(ERROR_MESSAGE) as MESSAGE
	from Common.SCORING_ERROR
	where convert(date,DATE) between @FROM_DATE and @TO_DATE
	order by DATE desc
GO
