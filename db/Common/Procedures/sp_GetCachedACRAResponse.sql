if exists (select * from sys.objects where name='sp_GetCachedACRAResponse' and type='P')
	drop procedure Common.sp_GetCachedACRAResponse
GO

create procedure Common.sp_GetCachedACRAResponse(@SOCIAL_CARD_NUMBER	char(10))
AS
	declare @DayCount int
	select @DayCount=convert(int,VALUE) from Common.SETTING where CODE='ACRA_CACHE_DAY'

	declare @DateTo date=getdate()
	declare @DateFrom date=dateadd(month,-@DayCount,@DateTo)

	declare @RESPONSE_XML nvarchar(max)

	select top 1 @RESPONSE_XML=n.RESPONSE_XML
	from Common.NORQ_QUERY_RESULT a
	join Common.ACRA_QUERY_RESULT n
		on a.APPLICATION_ID=n.APPLICATION_ID
	where a.SOCIAL_CARD_NUMBER=@SOCIAL_CARD_NUMBER
		and convert(date,n.QUERY_DATE) between @DateFrom and @DateTo
	order by n.QUERY_DATE desc

	if @RESPONSE_XML is null
		select top 1 @RESPONSE_XML=n.RESPONSE_XML
		from Common.NORQ_COBORROWER_QUERY_RESULT a
		join Common.ACRA_COBORROWER_QUERY_RESULT n
			on a.APPLICATION_ID=n.APPLICATION_ID
		where a.SOCIAL_CARD_NUMBER=@SOCIAL_CARD_NUMBER
			and convert(date,n.QUERY_DATE) between @DateFrom and @DateTo
		order by n.QUERY_DATE desc

	if not @RESPONSE_XML is NULL
		select @RESPONSE_XML as RESPONSE_XML
GO
