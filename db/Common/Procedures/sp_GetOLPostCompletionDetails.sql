CREATE OR ALTER PROCEDURE Common.sp_GetOLPostCompletionDetails(@ID	uniqueidentifier)
AS
	select
		isnull(IS_INSURED,0) as IS_INSURED,
		isnull(IS_REGISTERED,0) as IS_REGISTERED,
		isnull(IS_DISBURSED,0) as IS_DISBURSED
	from Common.APPLICATION
	where ID = @ID
GO
