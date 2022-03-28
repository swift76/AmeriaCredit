if exists (select * from sys.objects where name='sp_GetCachedACRAClientResponse' and type='P')
	drop procedure Common.sp_GetCachedACRAClientResponse
GO

create procedure Common.sp_GetCachedACRAClientResponse(@SOCIAL_CARD_NUMBER	char(10))
AS
	declare @DayCount int
	select @DayCount=convert(int,VALUE) from Common.SETTING where CODE='ACRA_CACHE_DAY'

	declare @DateTo date=getdate()
	declare @DateFrom date=dateadd(month,-@DayCount,@DateTo)

	select top 1 RESPONSE_XML
	from Common.ACRA_CLIENT_QUERY_RESULT
	where SOCIAL_CARD_NUMBER=@SOCIAL_CARD_NUMBER
		and convert(date,QUERY_DATE) between @DateFrom and @DateTo
	order by QUERY_DATE desc
GO
