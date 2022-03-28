if exists (select * from sys.objects where name='f_GetApprovedAmount' and type='FN')
	drop function Common.f_GetApprovedAmount
GO

create function Common.f_GetApprovedAmount(@APPLICATION_ID	uniqueidentifier,
										   @LOAN_TYPE_ID	char(2),
										   @CURRENCY_CODE	char(3))
RETURNS varchar(1000)
AS
BEGIN
	declare @Result varchar(1000)

	if @LOAN_TYPE_ID='00'
		select @Result = convert(varchar(15), FORMAT(AMOUNT, '###,###,###')) + ' ' + @CURRENCY_CODE
		from Common.APPLICATION_SCORING_RESULT
		where APPLICATION_ID = @APPLICATION_ID
	else
		select @Result = convert(varchar(15), FORMAT(max(AMOUNT), '###,###,###')) + ' ' + @CURRENCY_CODE
		from Common.APPLICATION_SCORING_RESULT
		where APPLICATION_ID = @APPLICATION_ID

	return @Result
END
GO
