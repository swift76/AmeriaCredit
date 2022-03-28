if exists (select * from sys.objects where name='sp_LogSMSCreditCardStep' and type='P')
	drop procedure GL.sp_LogSMSCreditCardStep
GO

create procedure GL.sp_LogSMSCreditCardStep (@APPLICATION_ID uniqueidentifier,
											 @CARD_NUMBER	 char(16) = null,
											 @MOBILE_PHONE	 varchar(20) = null)

AS
	BEGIN TRANSACTION

	BEGIN TRY
		declare @CUSTOMER_USER_ID int, @LOAN_TYPE_ID char(2), @OPERATION_DETAILS varchar(max)

		select @CUSTOMER_USER_ID = CUSTOMER_USER_ID, @LOAN_TYPE_ID = LOAN_TYPE_ID
		from Common.APPLICATION with (updlock) where ID = @APPLICATION_ID

		set @OPERATION_DETAILS = case
									when @CARD_NUMBER is not null then 'Credit card with number ' + @CARD_NUMBER + ' has been inserted'
									when @MOBILE_PHONE is not null then 'SMS has been sent to mobile phone with number ' + @MOBILE_PHONE
									else ''
								 end

		insert into Common.APPLICATION_OPERATION_LOG
			(CUSTOMER_USER_ID, SHOP_USER_ID, LOAN_TYPE_ID, OPERATION_CODE, APPLICATION_ID, OPERATION_DETAILS)
		values
			(@CUSTOMER_USER_ID, null, @LOAN_TYPE_ID, 'EDIT', @APPLICATION_ID, @OPERATION_DETAILS)

		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION
		declare @ErrorMessage nvarchar(4000) = ERROR_MESSAGE()
		RAISERROR (@ErrorMessage, 17, 1)
	END CATCH
GO
