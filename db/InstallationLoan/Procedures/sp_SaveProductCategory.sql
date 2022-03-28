create or alter procedure IL.sp_SaveProductCategory(
	@CODE		char(4),
	@NAME_AM	nvarchar(50),
	@NAME_EN	varchar(50),
	@CATEGORY	varchar(50)
)
AS
	BEGIN TRANSACTION

	BEGIN TRY
		delete from IL.PRODUCT_CATEGORY where CODE=@CODE
		insert into IL.PRODUCT_CATEGORY (CODE,NAME_AM,NAME_EN,CATEGORY)
			values (@CODE,Common.ahf_ANSI2Unicode(@NAME_AM),@NAME_EN,@CATEGORY)
		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION
		declare @ErrorMessage nvarchar(4000) = ERROR_MESSAGE()
		RAISERROR (@ErrorMessage, 17, 1)
		RETURN
	END CATCH
GO
