if exists (select * from sys.objects where name='sp_SaveInstallationLoanSetting' and type='P')
	drop procedure IL.sp_SaveInstallationLoanSetting
GO

create procedure IL.sp_SaveInstallationLoanSetting(@EXPIRE_DAY_COUNT	tinyint)
AS
	BEGIN TRANSACTION

	BEGIN TRY
		delete from IL.INSTALLATION_LOAN_SETTING
		insert into IL.INSTALLATION_LOAN_SETTING (EXPIRE_DAY_COUNT)
			values (@EXPIRE_DAY_COUNT)

		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION
		declare @ErrorMessage nvarchar(4000) = ERROR_MESSAGE()
		RAISERROR (@ErrorMessage, 17, 1)
		RETURN
	END CATCH
GO
