if exists (select * from sys.objects where name='sp_SaveUniversity' and type='P')
	drop procedure Common.sp_SaveUniversity
GO

create procedure Common.sp_SaveUniversity(@CODE		char(2),
										  @NAME		nvarchar(50),
										  @NAME_EN	varchar(50))
AS
	BEGIN TRANSACTION

	BEGIN TRY
		delete from Common.UNIVERSITY where CODE=@CODE
		insert into Common.UNIVERSITY (CODE,NAME_AM,NAME_EN)
			values (@CODE,Common.ahf_ANSI2Unicode(@NAME),@NAME_EN)

		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION
		declare @ErrorMessage nvarchar(4000) = ERROR_MESSAGE()
		RAISERROR (@ErrorMessage, 17, 1)
		RETURN
	END CATCH
GO
