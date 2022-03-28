create or alter procedure Common.sp_HasBranchRefusedGLApplication(@SOCIAL_CARD_NUMBER	char(10),
	@MONTH_COUNT		tinyint)
AS
	declare @Result bit
	declare @ToDate date = convert(date,getdate())
	declare @FromDate date = dateadd(month, -@MONTH_COUNT, @ToDate)

	if exists (
		select top 1 ID
		from Common.APPLICATION
		where SOCIAL_CARD_NUMBER=@SOCIAL_CARD_NUMBER
			and LOAN_TYPE_ID<>'00'
			and convert(date,CREATION_DATE) between @FromDate and @ToDate
			and STATUS in (9,14)
		)
		set @Result=1
	else
		set @Result=0

	select @Result
GO
