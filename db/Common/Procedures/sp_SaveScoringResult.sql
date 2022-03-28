if exists (select * from sys.objects where name='sp_SaveScoringResult' and type='P')
	drop procedure Common.sp_SaveScoringResult
GO

create procedure Common.sp_SaveScoringResult(@APPLICATION_ID		uniqueidentifier,
											 @SCORING_AMOUNT		money,
											 @SCORING_COEFFICIENT	money)
AS
	BEGIN TRANSACTION
	BEGIN TRY
		delete from Common.SCORING_RESULT where APPLICATION_ID=@APPLICATION_ID
		insert into Common.SCORING_RESULT
			(APPLICATION_ID,SCORING_AMOUNT,SCORING_COEFFICIENT)
		values
			(@APPLICATION_ID,@SCORING_AMOUNT,@SCORING_COEFFICIENT)
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION
		declare @ErrorMessage varchar(4000)
		set @ErrorMessage = ERROR_MESSAGE()
		RAISERROR (@ErrorMessage, 17, 1)
		RETURN
	END CATCH
	COMMIT TRANSACTION
GO
