if exists (select * from sys.objects where name='sp_SavePartnerCompany' and type='P')
	drop procedure Common.sp_SavePartnerCompany
GO

create procedure Common.sp_SavePartnerCompany(
	@CODE			varchar(8),
	@NAME			nvarchar(40),
	@PARENT_CODE	varchar(8),
	@SHOP_CODE		char(4)
)
AS
	BEGIN TRANSACTION
	BEGIN TRY
		delete from Common.PARTNER_COMPANY where CODE=@CODE
		insert into Common.PARTNER_COMPANY (CODE,NAME,PARENT_CODE,SHOP_CODE)
			values (@CODE,Common.ahf_ANSI2Unicode(@NAME),@PARENT_CODE,@SHOP_CODE)

		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION
		declare @ErrorMessage nvarchar(4000) = ERROR_MESSAGE()
		RAISERROR (@ErrorMessage, 17, 1)
		RETURN
	END CATCH
GO
