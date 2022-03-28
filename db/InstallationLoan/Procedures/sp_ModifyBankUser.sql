if exists (select * from sys.objects where name='sp_ModifyBankUser' and type='P')
	drop procedure IL.sp_ModifyBankUser
GO

create procedure IL.sp_ModifyBankUser(@BANK_USER_ID			int,
									  @LOGIN				varchar(50) = null,
									  @FIRST_NAME			nvarchar(50) = null,
									  @LAST_NAME			nvarchar(50) = null,
									  @EMAIL				varchar(70),
									  @IS_ADMINISTRATOR		bit = null,
									  @HASH					varchar(1000) = null,
									  @OPERATION_DETAILS	nvarchar(max),
									  @APPLICATION_USER_ID	int)
AS
	declare @PASSWORD_EXPIRY_DATE date

	BEGIN TRANSACTION

	BEGIN TRY
		declare @BankUserStateID int
		select @BankUserStateID = a.OBJECT_STATE_ID
		from IL.BANK_USER as b
			join Common.APPLICATION_USER as a
				on b.APPLICATION_USER_ID = a.ID
		where b.APPLICATION_USER_ID = @BANK_USER_ID
		if @BankUserStateID = 2
			RAISERROR (N'Փակված վիճակում գտնվող բանկի օգտագործողին փոփոխել հնարավոր չէ', 17, 1)

		insert into IL.BANK_APPLICATION_OPERATION_LOG (APPLICATION_USER_ID,BANK_OBJECT_CODE,BANK_OPERATION_CODE,ENTITY_ID,OPERATION_DETAILS)
		values (@APPLICATION_USER_ID,'BANKUSER','EDIT',@BANK_USER_ID,@OPERATION_DETAILS)

		if (@HASH is null)
			set @PASSWORD_EXPIRY_DATE = null
		else
			set @PASSWORD_EXPIRY_DATE = getdate()

		update Common.APPLICATION_USER
			set
				LOGIN=isnull(@LOGIN,LOGIN),
				HASH=isnull(@HASH,HASH),
				FIRST_NAME=isnull(@FIRST_NAME,FIRST_NAME),
				LAST_NAME=isnull(@LAST_NAME,LAST_NAME),
				EMAIL=isnull(@EMAIL,EMAIL),
				PASSWORD_EXPIRY_DATE=isnull(@PASSWORD_EXPIRY_DATE,PASSWORD_EXPIRY_DATE)
			where ID=@BANK_USER_ID

		update IL.BANK_USER
			set IS_ADMINISTRATOR=isnull(@IS_ADMINISTRATOR,IS_ADMINISTRATOR)
			where APPLICATION_USER_ID=@BANK_USER_ID

		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION
		declare @ErrorMessage nvarchar(4000) = ERROR_MESSAGE()
		RAISERROR (@ErrorMessage, 17, 1)
		RETURN
	END CATCH
GO
