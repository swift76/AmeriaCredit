if exists (select * from sys.objects where name='sp_SaveScoringResults' and type='P')
	drop procedure Common.sp_SaveScoringResults
GO

create procedure Common.sp_SaveScoringResults(@APPLICATION_ID	uniqueidentifier,
											  @RESULTS			Common.ScoringResults	readonly)
AS
	BEGIN TRANSACTION
	BEGIN TRY
		delete from Common.SCORING_RESULTS where APPLICATION_ID=@APPLICATION_ID

		insert into Common.SCORING_RESULTS
			(APPLICATION_ID,SCORING_OPTION,SCORING_AMOUNT,SCORING_COEFFICIENT,SCORING_INTEREST)
		select @APPLICATION_ID,SCORING_OPTION,SCORING_AMOUNT,SCORING_COEFFICIENT,SCORING_INTEREST
			from @RESULTS
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
