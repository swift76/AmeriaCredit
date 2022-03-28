if exists (select * from sys.objects where name='sp_GetCachedNORQResponse' and type='P')
	drop procedure Common.sp_GetCachedNORQResponse
GO

create procedure Common.sp_GetCachedNORQResponse(@SOCIAL_CARD_NUMBER	char(10))
AS
	declare @DayCount int
	select @DayCount=convert(int,VALUE) from Common.SETTING where CODE='NORQ_CACHE_DAY'

	declare @DateTo date=getdate()
	declare @DateFrom date=dateadd(month,-@DayCount,@DateTo)

	declare @RESPONSE_XML nvarchar(max)

	select top 1 @RESPONSE_XML=RESPONSE_XML
	from Common.NORQ_QUERY_RESULT
	where SOCIAL_CARD_NUMBER=@SOCIAL_CARD_NUMBER
		and convert(date,QUERY_DATE) between @DateFrom and @DateTo
	order by QUERY_DATE desc

	if @RESPONSE_XML is null
		select top 1 @RESPONSE_XML=RESPONSE_XML
		from Common.NORQ_CLIENT_QUERY_RESULT
		where SOCIAL_CARD_NUMBER=@SOCIAL_CARD_NUMBER
			and convert(date,QUERY_DATE) between @DateFrom and @DateTo
		order by QUERY_DATE desc

	if @RESPONSE_XML is null
		select top 1 @RESPONSE_XML=RESPONSE_XML
		from Common.NORQ_COBORROWER_QUERY_RESULT
		where SOCIAL_CARD_NUMBER=@SOCIAL_CARD_NUMBER
			and convert(date,QUERY_DATE) between @DateFrom and @DateTo
		order by QUERY_DATE desc

	if not @RESPONSE_XML is NULL
		select @RESPONSE_XML as RESPONSE_XML
GO
