if exists (select * from sys.objects where name='sp_SaveLoanServiceCondition' and type='P')
	drop procedure Common.sp_SaveLoanServiceCondition
GO

create procedure Common.sp_SaveLoanServiceCondition(@LOAN_TYPE_CODE			char(2),
													@SERVICE_TYPE_CODE		char(1),
													@CURRENCY				char(3),
													@CREDIT_CARD_TYPE_CODE	char(3),
													@AMOUNT					money,
													@INTEREST				money,
													@MIN_AMOUNT				money,
													@MAX_AMOUNT				money)
AS
	insert into Common.LOAN_SERVICE_CONDITION (LOAN_TYPE_CODE,SERVICE_TYPE_CODE,CURRENCY,CREDIT_CARD_TYPE_CODE,AMOUNT,INTEREST,MIN_AMOUNT,MAX_AMOUNT)
		values (@LOAN_TYPE_CODE,@SERVICE_TYPE_CODE,@CURRENCY,@CREDIT_CARD_TYPE_CODE,@AMOUNT,@INTEREST,@MIN_AMOUNT,@MAX_AMOUNT)
GO
