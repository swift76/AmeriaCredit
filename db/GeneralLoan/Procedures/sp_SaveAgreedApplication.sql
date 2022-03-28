if exists (select * from sys.objects where name='sp_SaveAgreedApplication' and type='P')
	drop procedure GL.sp_SaveAgreedApplication
GO

create procedure GL.sp_SaveAgreedApplication(
	@APPLICATION_ID 		uniqueidentifier,
	@EXISTING_CARD_CODE		char(16) = null,
	@IS_NEW_CARD			bit = null,
	@CREDIT_CARD_TYPE_CODE	char(3) = null,
	@IS_CARD_DELIVERY		bit = null,
	@CARD_DELIVERY_ADDRESS	nvarchar(150) = null,
	@BANK_BRANCH_CODE		char(3) = null,
	@IS_ARBITRAGE_CHECKED	bit = null,
	@ACTUAL_INTEREST		money = null,
	@OPERATION_DETAILS		nvarchar(max),
	@IS_SUBMIT				bit
)
AS
	BEGIN TRANSACTION

	BEGIN TRY
		declare @STATUS tinyint, @CUSTOMER_USER_ID int, @LOAN_TYPE_ID char(2)

		select @STATUS = STATUS, @CUSTOMER_USER_ID = CUSTOMER_USER_ID, @LOAN_TYPE_ID = LOAN_TYPE_ID
		from Common.APPLICATION with (updlock) where ID = @APPLICATION_ID

		if @STATUS<>13
			RAISERROR (N'Հայտը պայմանագրի կնքման կարգավիճակում չէ', 17, 1)

		insert into Common.APPLICATION_OPERATION_LOG
			(CUSTOMER_USER_ID, SHOP_USER_ID, LOAN_TYPE_ID, OPERATION_CODE, APPLICATION_ID, OPERATION_DETAILS)
		values
			(@CUSTOMER_USER_ID, null, @LOAN_TYPE_ID, 'EDIT', @APPLICATION_ID, @OPERATION_DETAILS)

		delete from GL.AGREED_APPLICATION
		where APPLICATION_ID = @APPLICATION_ID

		insert into GL.AGREED_APPLICATION (
			APPLICATION_ID,
			EXISTING_CARD_CODE,
			IS_NEW_CARD,
			CREDIT_CARD_TYPE_CODE,
			IS_CARD_DELIVERY,
			CARD_DELIVERY_ADDRESS,
			BANK_BRANCH_CODE,
			IS_ARBITRAGE_CHECKED,
			ACTUAL_INTEREST)
		values (
			@APPLICATION_ID,
			@EXISTING_CARD_CODE,
			@IS_NEW_CARD,
			@CREDIT_CARD_TYPE_CODE,
			@IS_CARD_DELIVERY,
			@CARD_DELIVERY_ADDRESS,
			@BANK_BRANCH_CODE,
			@IS_ARBITRAGE_CHECKED,
			@ACTUAL_INTEREST)

		if @IS_SUBMIT=1
			execute Common.sp_ChangeApplicationStatus @APPLICATION_ID,15

		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION
		declare @ErrorMessage nvarchar(4000) = ERROR_MESSAGE()
		RAISERROR (@ErrorMessage, 17, 1)
	END CATCH
GO
