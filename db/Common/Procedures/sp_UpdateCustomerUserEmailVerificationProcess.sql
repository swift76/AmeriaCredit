if exists (select * from sys.objects where name='sp_UpdateCustomerUserEmailVerificationProcess' and type='P')
	drop procedure Common.sp_UpdateCustomerUserEmailVerificationProcess
GO

create procedure Common.sp_UpdateCustomerUserEmailVerificationProcess(
	@PROCESS_ID	uniqueidentifier
)
AS
	declare @CUSTOMER_USER_ID int, @EMAIL varchar(70)
	BEGIN TRANSACTION

	BEGIN TRY
		select @CUSTOMER_USER_ID = CUSTOMER_USER_ID
			,@EMAIL = EMAIL
		from Common.CUSTOMER_USER_EMAIL_VERIFICATION_PROCESS
		where PROCESS_ID = @PROCESS_ID

		if not @CUSTOMER_USER_ID is null
		begin
			update Common.CUSTOMER_USER
			set	IS_EMAIL_VERIFIED = 1
			where APPLICATION_USER_ID = @CUSTOMER_USER_ID
				and EMAIL = @EMAIL

			delete from Common.CUSTOMER_USER_EMAIL_VERIFICATION_PROCESS
			where PROCESS_ID = @PROCESS_ID
		end

		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION
		declare @ErrorMessage nvarchar(4000) = ERROR_MESSAGE()
		RAISERROR (@ErrorMessage, 17, 1)
	END CATCH
GO
