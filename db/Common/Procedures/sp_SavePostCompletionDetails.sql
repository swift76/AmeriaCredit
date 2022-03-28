if exists (select * from sys.objects where name='sp_SavePostCompletionDetails' and type='P')
	drop procedure Common.sp_SavePostCompletionDetails
GO

create procedure Common.sp_SavePostCompletionDetails(
	@APPLICATION_ID uniqueidentifier,
	@IS_INSURED		bit,
	@IS_REGISTERED	bit,
	@IS_DISBURSED	bit
)
AS
	update Common.APPLICATION
		set IS_INSURED = @IS_INSURED
			,IS_REGISTERED = @IS_REGISTERED
			,IS_DISBURSED = @IS_DISBURSED
			,TO_BE_SYNCHRONIZED = 1
	where ID = @APPLICATION_ID
GO
