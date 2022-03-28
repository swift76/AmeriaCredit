if exists (select * from sys.objects where name='sp_DeleteApplication' and type='P')
	drop procedure Common.sp_DeleteApplication
GO

create procedure Common.sp_DeleteApplication(@ID				uniqueidentifier,
										     @OPERATION_DETAILS	nvarchar(max))

AS
	BEGIN TRANSACTION

	BEGIN TRY
		declare @STATUS tinyint, @CUSTOMER_USER_ID int, @SHOP_USER_ID int, @PARTNER_COMPANY_CODE varchar(8), @LOAN_TYPE_ID char(2)

		select @STATUS = STATUS, @CUSTOMER_USER_ID = CUSTOMER_USER_ID, @SHOP_USER_ID = SHOP_USER_ID, @PARTNER_COMPANY_CODE = PARTNER_COMPANY_CODE, @LOAN_TYPE_ID = LOAN_TYPE_ID
		from Common.APPLICATION with (updlock)
		where ID = @ID

		if @STATUS<>0
			RAISERROR (N'Հայտը նախնական կարգավիճակում չէ', 17, 1)

		insert into Common.APPLICATION_OPERATION_LOG
				(CUSTOMER_USER_ID, SHOP_USER_ID, PARTNER_COMPANY_CODE, LOAN_TYPE_ID, OPERATION_CODE, APPLICATION_ID, OPERATION_DETAILS)
		values
				(@CUSTOMER_USER_ID, @SHOP_USER_ID, @PARTNER_COMPANY_CODE, @LOAN_TYPE_ID, 'DELETE', @ID, @OPERATION_DETAILS)

		if @LOAN_TYPE_ID = '00'
			execute IL.sp_DeleteApplicationProducts @ID

		delete from Common.APPLICATION_SCAN where APPLICATION_ID = @ID
		delete from Common.APPLICATION where ID = @ID

		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION
		declare @ErrorMessage nvarchar(4000) = ERROR_MESSAGE()
		RAISERROR (@ErrorMessage, 17, 1)
	END CATCH
GO
