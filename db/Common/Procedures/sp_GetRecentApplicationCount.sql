CREATE OR ALTER PROCEDURE Common.sp_GetRecentApplicationCount(@SOCIAL_CARD_NUMBER	char(10),
												   @DAY_COUNT			tinyint,
												   @APPLICATION_ID		uniqueidentifier,
												   @LOAN_TYPE_ID		char(2))
AS
	declare @Result int
	declare @ToDate date = convert(date,getdate())
	declare @FromDate date = dateadd(day, -@DAY_COUNT, @ToDate)

	if @LOAN_TYPE_ID='00'
		select @Result=count(ID)
		from Common.APPLICATION
		where SOCIAL_CARD_NUMBER=@SOCIAL_CARD_NUMBER
			and ID<>@APPLICATION_ID
			and LOAN_TYPE_ID='00'
			and convert(date,CREATION_DATE) between @FromDate and @ToDate
			and not isnull(REFUSAL_REASON,'') in (N'Տվյալների անհամապատասխանություն',N'Սխալ փաստաթղթի տվյալներ',N'Համակարգային սխալ')
			and STATUS<>0
	else
	begin
		declare @PledgeType char(1)

		select @PledgeType=PLEDGE_TYPE
		from Common.LOAN_TYPE
		where CODE=@LOAN_TYPE_ID

		select @Result=count(a.ID)
		from Common.APPLICATION a
		join Common.LOAN_TYPE t
			on a.LOAN_TYPE_ID=t.CODE
		where a.SOCIAL_CARD_NUMBER=@SOCIAL_CARD_NUMBER
			and a.ID<>@APPLICATION_ID
			and a.LOAN_TYPE_ID<>'00'
			and rtrim(t.PLEDGE_TYPE)=rtrim(@PledgeType)
			and convert(date,a.CREATION_DATE) between @FromDate and @ToDate
			and not isnull(a.REFUSAL_REASON,'') in (N'Տվյալների անհամապատասխանություն',N'Սխալ փաստաթղթի տվյալներ',N'Համակարգային սխալ')
			and a.STATUS<>0
	end

	select @Result
GO
