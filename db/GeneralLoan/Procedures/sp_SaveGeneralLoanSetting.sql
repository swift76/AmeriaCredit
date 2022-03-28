if exists (select * from sys.objects where name='sp_SaveGeneralLoanSetting' and type='P')
	drop procedure GL.sp_SaveGeneralLoanSetting
GO

create procedure GL.sp_SaveGeneralLoanSetting(@REPEAT_COUNT 	int,
											  @REPEAT_DAY_COUNT tinyint,
											  @EXPIRE_DAY_COUNT	tinyint)
AS
	BEGIN TRANSACTION

	BEGIN TRY
		delete from GL.GENERAL_LOAN_SETTING
		insert into GL.GENERAL_LOAN_SETTING (REPEAT_COUNT,REPEAT_DAY_COUNT,EXPIRE_DAY_COUNT)
			values (@REPEAT_COUNT,@REPEAT_DAY_COUNT,@EXPIRE_DAY_COUNT)

		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION
		declare @ErrorMessage nvarchar(4000) = ERROR_MESSAGE()
		RAISERROR (@ErrorMessage, 17, 1)
		RETURN
	END CATCH
GO
