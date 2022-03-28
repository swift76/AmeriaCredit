CREATE OR ALTER PROCEDURE Common.sp_DoesClientHaveLoanApplication(@CLIENT_CODE	char(8))
AS
	declare @Result bit

	if exists (select top 1 ID from Common.APPLICATION
						where CLIENT_CODE=@CLIENT_CODE)
		set @Result = 1
	else
		set @Result = 0

	select @Result
GO
