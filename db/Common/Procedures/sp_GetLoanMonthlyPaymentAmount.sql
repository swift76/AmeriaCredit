create or alter procedure Common.sp_GetLoanMonthlyPaymentAmount(
	@AMOUNT				money,
	@TERM				tinyint,
	@INTEREST			money,
	@SERVICE_AMOUNT		money,
	@SERVICE_INTEREST	money
)
AS
	declare @RESULT money

	if @TERM=0
		set @RESULT=0
	else
	begin
		if @INTEREST=0
			set @RESULT=@AMOUNT/@TERM
		else
		begin
			declare @MONTHLY_INTEREST float = convert(float,@INTEREST) / 1200
			set @RESULT=@AMOUNT*@MONTHLY_INTEREST/(1-1/power(1+@MONTHLY_INTEREST,@TERM))
		end

		if isnull(@SERVICE_AMOUNT,0)>0
			set @AMOUNT=@SERVICE_AMOUNT
		else
			set @AMOUNT=@AMOUNT*isnull(@SERVICE_INTEREST,0)/100

		set @RESULT=@RESULT+@AMOUNT
	end

	select (@RESULT/100)*100 as MONTHLY_PAYMENT_AMOUNT
GO
