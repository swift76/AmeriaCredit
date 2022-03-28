if exists (select * from sys.objects where name='sp_CreateShopUser' and type='P')
	drop procedure IL.sp_CreateShopUser
GO

create procedure IL.sp_CreateShopUser(@LOGIN				varchar(50),
									  @HASH					varchar(1000),
									  @FIRST_NAME			nvarchar(50),
									  @LAST_NAME			nvarchar(50),
									  @EMAIL				varchar(70),
									  @PASSWORD_EXPIRY_DATE	date,
									  @SHOP_CODE			char(4),
									  @IS_MANAGER			bit,
									  @MOBILE_PHONE			varchar(20),
									  @OPERATION_DETAILS	nvarchar(max),
									  @APPLICATION_USER_ID	int)
AS
	declare @ShopUserID int

	BEGIN TRANSACTION

	BEGIN TRY
		insert into Common.APPLICATION_USER (LOGIN,HASH,FIRST_NAME,LAST_NAME,EMAIL,PASSWORD_EXPIRY_DATE,OBJECT_STATE_ID,USER_ROLE_ID)
			values (@LOGIN,@HASH,@FIRST_NAME,@LAST_NAME,@EMAIL,@PASSWORD_EXPIRY_DATE,1,2)

		set @ShopUserID=SCOPE_IDENTITY()

		insert into IL.SHOP_USER (APPLICATION_USER_ID,SHOP_CODE,IS_MANAGER,MOBILE_PHONE)
			values (@ShopUserID,@SHOP_CODE,@IS_MANAGER,@MOBILE_PHONE)

		insert into IL.BANK_APPLICATION_OPERATION_LOG (APPLICATION_USER_ID,BANK_OBJECT_CODE,BANK_OPERATION_CODE,ENTITY_ID,OPERATION_DETAILS)
		values (@APPLICATION_USER_ID,'SHOPUSER','CREATE',@ShopUserID,@OPERATION_DETAILS)

		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION
		declare @ErrorMessage nvarchar(4000) = ERROR_MESSAGE()
		RAISERROR (@ErrorMessage, 17, 1)
		RETURN
	END CATCH
GO
