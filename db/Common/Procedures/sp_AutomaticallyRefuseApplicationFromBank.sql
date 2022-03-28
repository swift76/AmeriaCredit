if exists (select * from sys.objects where name='sp_AutomaticallyRefuseApplicationFromBank' and type='P')
	drop procedure Common.sp_AutomaticallyRefuseApplicationFromBank
GO

create procedure Common.sp_AutomaticallyRefuseApplicationFromBank(@ID				uniqueidentifier,
																  @ISN				int,
																  @REFUSAL_REASON	varchar(100),
																  @CLIENT_CODE		char(8))
AS
	BEGIN TRANSACTION

	BEGIN TRY
		declare @CURRENT_STATUS tinyint

		select @CURRENT_STATUS = STATUS from Common.APPLICATION with (updlock) where ID = @ID

		if @CURRENT_STATUS <> 3
			RAISERROR (N'Հայտի կարգավիճակն արդեն փոփոխվել է', 17, 1)

		update Common.APPLICATION set
			REFUSAL_REASON=Common.ahf_ANSI2Unicode(@REFUSAL_REASON),
			STATUS=6,
			TO_BE_SYNCHRONIZED=0,
			ISN=@ISN,
			CLIENT_CODE=@CLIENT_CODE
		where ID=@ID

		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION
		declare @ErrorMessage nvarchar(4000) = ERROR_MESSAGE()
		RAISERROR (@ErrorMessage, 17, 1)
	END CATCH
GO
