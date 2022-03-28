if exists (select * from sys.objects where name='sp_AutomaticallyApproveApplication' and type='P')
	drop procedure Common.sp_AutomaticallyApproveApplication
GO

create procedure Common.sp_AutomaticallyApproveApplication(@ID					uniqueidentifier,
														   @ISN					int,
														   @HAS_BANK_CARD		bit,
														   @CLIENT_CODE			char(8),
														   @IS_DATA_COMPLETE	bit)
AS
	BEGIN TRANSACTION

	BEGIN TRY
		declare @CURRENT_STATUS tinyint,@IMPORT_ID int,@CUSTOMER_USER_ID int,@SOCIAL_CARD_NUMBER char(10)

		select @CURRENT_STATUS = STATUS,
			@IMPORT_ID = IMPORT_ID,
			@CUSTOMER_USER_ID = CUSTOMER_USER_ID,
			@SOCIAL_CARD_NUMBER = SOCIAL_CARD_NUMBER
		from Common.APPLICATION with (updlock) where ID = @ID

		if @CURRENT_STATUS <> 3
			RAISERROR (N'Հայտի կարգավիճակն արդեն փոփոխվել է', 17, 1)

		if @IMPORT_ID>0
			SELECT @CUSTOMER_USER_ID=APPLICATION_USER_ID
				FROM Common.CUSTOMER_USER
				WHERE SOCIAL_CARD_NUMBER=@SOCIAL_CARD_NUMBER

		update Common.APPLICATION set
			HAS_BANK_CARD=@HAS_BANK_CARD,
			STATUS=5,
			TO_BE_SYNCHRONIZED=0,
			ISN=@ISN,
			CLIENT_CODE=@CLIENT_CODE,
			IS_DATA_COMPLETE=@IS_DATA_COMPLETE,
			CUSTOMER_USER_ID=@CUSTOMER_USER_ID
		where ID=@ID

		if rtrim(isnull(@CLIENT_CODE,''))<>'' and not @CUSTOMER_USER_ID is null
			update Common.CUSTOMER_USER set CLIENT_CODE=@CLIENT_CODE
			where APPLICATION_USER_ID=@CUSTOMER_USER_ID

		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION
		declare @ErrorMessage nvarchar(4000) = ERROR_MESSAGE()
		RAISERROR (@ErrorMessage, 17, 1)
	END CATCH
GO
