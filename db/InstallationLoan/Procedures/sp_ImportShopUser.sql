if exists (select * from sys.objects where name='sp_ImportShopUser' and type='P')
	drop procedure IL.sp_ImportShopUser
GO

create procedure IL.sp_ImportShopUser(@LOGIN		varchar(50),
									  @FIRST_NAME	nvarchar(50),
									  @LAST_NAME	nvarchar(50),
									  @EMAIL		varchar(70),
									  @SHOP_CODE	char(4),
									  @IS_MANAGER	bit,
									  @MOBILE_PHONE	varchar(20))
AS
	declare @ShopUserID int

	BEGIN TRANSACTION

	BEGIN TRY
		insert into Common.APPLICATION_USER (LOGIN,FIRST_NAME,LAST_NAME,HASH,EMAIL,OBJECT_STATE_ID,USER_ROLE_ID)
			values (@LOGIN,Common.ahf_ANSI2Unicode(@FIRST_NAME),Common.ahf_ANSI2Unicode(@LAST_NAME),'Hyc2TfluirwkJuvgdKXGT2ddD10=',@EMAIL,1,2)

		set @ShopUserID=SCOPE_IDENTITY()

		insert into IL.SHOP_USER (APPLICATION_USER_ID,SHOP_CODE,IS_MANAGER,MOBILE_PHONE)
			values (@ShopUserID,@SHOP_CODE,@IS_MANAGER,@MOBILE_PHONE)

		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION
		declare @ErrorMessage nvarchar(4000) = ERROR_MESSAGE()
		RAISERROR (@ErrorMessage, 17, 1)
		RETURN
	END CATCH
GO
