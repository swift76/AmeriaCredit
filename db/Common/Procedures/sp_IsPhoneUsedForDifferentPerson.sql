CREATE OR ALTER PROCEDURE Common.sp_IsPhoneUsedForDifferentPerson(@SOCIAL_CARD_NUMBER	char(10),
													   @MONTH_COUNT			tinyint,
													   @MOBILE_PHONE		varchar(20),
													   @APPLICATION_ID		uniqueidentifier)
AS
	declare @Result bit
	declare @ToDate date = convert(date,getdate())
	declare @FromDate date = dateadd(month, -@MONTH_COUNT, @ToDate)

	select @Result=convert(bit,case count(ID) when 0 then 0 else 1 end)
	from Common.APPLICATION
	where SOCIAL_CARD_NUMBER<>@SOCIAL_CARD_NUMBER
		and MOBILE_PHONE_1=@MOBILE_PHONE and isnull(MOBILE_PHONE_1,'')<>''
		and convert(date,CREATION_DATE) between @FromDate and @ToDate
		and ID<>@APPLICATION_ID
		and STATUS in (5,7,8,10,11,12,13,15,20,21,22,55)

	select @Result
GO
