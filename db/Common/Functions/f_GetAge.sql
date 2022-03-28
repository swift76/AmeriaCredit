CREATE OR ALTER FUNCTION Common.f_GetAge(@BIRTH_DATE	date)
RETURNS tinyint
AS
BEGIN
	declare @CurrentDate date = convert(date,getdate())
	declare @Age tinyint = datediff(year, @BIRTH_DATE, @CurrentDate)
	if dateadd(year, @Age, @BIRTH_DATE) > @CurrentDate
		set @Age=@Age-1
	RETURN @Age
END
GO
