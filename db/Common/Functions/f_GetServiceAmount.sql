if exists (select * from sys.objects where name='f_GetServiceAmount' and type='FN')
	drop function Common.f_GetServiceAmount
GO

create function Common.f_GetServiceAmount(@APPLICATION_AMOUNT	money,
										  @SERVICE_AMOUNT		money,
										  @SERVICE_INTEREST		money,
										  @MIN_AMOUNT			money,
										  @MAX_AMOUNT			money)
RETURNS money
AS
BEGIN
	declare @Result money

	if isnull(@SERVICE_AMOUNT,0)>0
		set @Result=@SERVICE_AMOUNT
	else
		set @Result=@APPLICATION_AMOUNT*isnull(@SERVICE_INTEREST,0)/100

	if @Result<@MIN_AMOUNT
		set @Result=@MIN_AMOUNT
	if @Result>@MAX_AMOUNT
		set @Result=@MAX_AMOUNT

	return @Result
END
GO
