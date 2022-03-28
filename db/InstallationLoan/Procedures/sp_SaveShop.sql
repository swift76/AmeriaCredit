if exists (select * from sys.objects where name='sp_SaveShop' and type='P')
	drop procedure IL.sp_SaveShop
GO

create procedure IL.sp_SaveShop(@CODE			char(4),
								@NAME			nvarchar(50),
								@NAME_EN		varchar(50),
								@HEAD_CODE		char(4),
								@ADDRESS		nvarchar(100),
								@ADDRESS_EN		varchar(100),
								@IS_DELIVERY	bit)
AS
	BEGIN TRANSACTION

	BEGIN TRY
		delete from IL.SHOP where CODE=@CODE
		delete from IL.SHOP_CATEGORY where SHOP_CODE=@CODE
		insert into IL.SHOP (CODE,NAME,NAME_EN,HEAD_CODE,ADDRESS,ADDRESS_EN,IS_DELIVERY)
			values (@CODE,Common.ahf_ANSI2Unicode(@NAME),@NAME_EN,@HEAD_CODE,Common.ahf_ANSI2Unicode(@ADDRESS),@ADDRESS_EN,@IS_DELIVERY)

		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION
		declare @ErrorMessage nvarchar(4000) = ERROR_MESSAGE()
		RAISERROR (@ErrorMessage, 17, 1)
		RETURN
	END CATCH
GO
