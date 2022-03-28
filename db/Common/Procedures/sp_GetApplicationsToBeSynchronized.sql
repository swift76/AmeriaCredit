CREATE OR ALTER PROCEDURE Common.sp_GetApplicationsToBeSynchronized
AS
	select ID,STATUS,isnull(ISN,-1) as ISN,SOURCE_ID,Common.ahf_Unicode2ANSI(isnull(REFUSAL_REASON,'')) as REFUSAL_REASON
	from Common.APPLICATION with (NOLOCK)
	where TO_BE_SYNCHRONIZED=1
		and STATUS>0
		and datediff(day,convert(date,CREATION_DATE),convert(date,getdate()))<=7
	order by CREATION_DATE
GO
