if exists (select * from sys.objects where name='sp_GetApplicationCountSetting' and type='P')
	drop procedure GL.sp_GetApplicationCountSetting
GO

create procedure GL.sp_GetApplicationCountSetting(@SOCIAL_CARD_NUMBER	char(10),
												  @APPLICATION_ID		uniqueidentifier)
AS
	declare @APPLICATION_COUNT int,@REPEAT_COUNT int,@REPEAT_DAY_COUNT tinyint
	select @REPEAT_COUNT=REPEAT_COUNT,@REPEAT_DAY_COUNT=REPEAT_DAY_COUNT from GL.GENERAL_LOAN_SETTING
	declare @ToDate date = convert(date,getdate())
	declare @FromDate date = dateadd(day, -@REPEAT_DAY_COUNT, @ToDate)

	select @APPLICATION_COUNT=count(a.ID)
	from Common.APPLICATION a
	join Common.LOAN_TYPE t
		on a.LOAN_TYPE_ID=t.CODE
	where a.SOCIAL_CARD_NUMBER=@SOCIAL_CARD_NUMBER
		and (@APPLICATION_ID is null or a.ID<>@APPLICATION_ID)
		and a.LOAN_TYPE_ID<>'00'
		and rtrim(t.PLEDGE_TYPE)=''
		and convert(date,a.CREATION_DATE) between @FromDate and @ToDate
		and not isnull(a.REFUSAL_REASON,'') in (N'Տվյալների անհամապատասխանություն',N'Սխալ փաստաթղթի տվյալներ',N'Համակարգային սխալ')
		and a.STATUS<>0

	select isnull(@APPLICATION_COUNT,0) as APPLICATION_COUNT,
		   isnull(@REPEAT_COUNT,0) as REPEAT_COUNT,
		   isnull(@REPEAT_DAY_COUNT,0) as REPEAT_DAY_COUNT
GO
