if exists (select * from sys.objects where name='sp_ModifyShopUser' and type='P')
	drop procedure IL.sp_ModifyShopUser
GO

create procedure IL.sp_ModifyShopUser(@SHOP_USER_ID			int,
									  @LOGIN				varchar(50) = null,
									  @FIRST_NAME			nvarchar(50) = null,
									  @LAST_NAME			nvarchar(50) = null,
									  @EMAIL				varchar(70),
									  @SHOP_CODE			char(4) = null,
									  @IS_MANAGER			bit = null,
									  @MOBILE_PHONE			varchar(20) = null,
									  @HASH					varchar(1000) = null,
									  @OPERATION_DETAILS	nvarchar(max),
									  @APPLICATION_USER_ID	int)
AS
	declare @PASSWORD_EXPIRY_DATE date

	BEGIN TRANSACTION

	BEGIN TRY
		declare @ShopUserStateID int
		select @ShopUserStateID = a.OBJECT_STATE_ID
		from IL.SHOP_USER as s
			join Common.APPLICATION_USER as a
				on s.APPLICATION_USER_ID = a.ID
		where s.APPLICATION_USER_ID = @SHOP_USER_ID
		if @ShopUserStateID = 2
			RAISERROR (N'Փակված վիճակում գտնվող խանութի օգտագործողին փոփոխել հնարավոր չէ', 17, 1)

		insert into IL.BANK_APPLICATION_OPERATION_LOG (APPLICATION_USER_ID,BANK_OBJECT_CODE,BANK_OPERATION_CODE,ENTITY_ID,OPERATION_DETAILS)
		values (@APPLICATION_USER_ID,'SHOPUSER','EDIT',@SHOP_USER_ID,@OPERATION_DETAILS)

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
			where ID=@SHOP_USER_ID

		update IL.SHOP_USER
			set
				SHOP_CODE=isnull(@SHOP_CODE,SHOP_CODE),
				MOBILE_PHONE=isnull(@MOBILE_PHONE,MOBILE_PHONE),
				IS_MANAGER=isnull(@IS_MANAGER,IS_MANAGER)
			where APPLICATION_USER_ID=@SHOP_USER_ID

		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION
		declare @ErrorMessage nvarchar(4000) = ERROR_MESSAGE()
		RAISERROR (@ErrorMessage, 17, 1)
		RETURN
	END CATCH
GO
