if exists (select * from sys.objects where name='sp_EmptyDirectoriesBeforeSynchronization' and type='P')
	drop procedure Common.sp_EmptyDirectoriesBeforeSynchronization
GO

create procedure Common.sp_EmptyDirectoriesBeforeSynchronization
AS
	BEGIN TRANSACTION

	BEGIN TRY
		truncate table Common.DOCUMENT_TYPE
		truncate table Common.COMMUNICATION_TYPE
		truncate table Common.ORGANIZATION_ACTIVITY
		truncate table Common.CARD_RECEIVING_OPTIONS
		truncate table Common.COUNTRY
		truncate table Common.STATE
		truncate table Common.CITY
		truncate table Common.APPLICATION_SCAN_TYPE
		truncate table Common.MONTHLY_NET_SALARY

		truncate table GL.WORKING_EXPERIENCE
		truncate table GL.FAMILY_STATUS
		truncate table GL.BANK_BRANCH

		truncate table IL.GOODS_RECEIVING_OPTIONS

		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION
		declare @ErrorMessage nvarchar(4000) = ERROR_MESSAGE()
		RAISERROR (@ErrorMessage, 17, 1)
		RETURN
	END CATCH
GO
