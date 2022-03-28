create or alter procedure Common.sp_LogLoanStatusError(
	@ISN 			int,
	@ERROR_MESSAGE	nvarchar(1000)
)
AS
	declare @APPLICATION_ID uniqueidentifier
	select @APPLICATION_ID=ID from Common.APPLICATION with (nolock)
		where ISN=@ISN

	set @ERROR_MESSAGE = Common.ahf_ANSI2Unicode(@ERROR_MESSAGE)

	if exists(select top 1 STATE
			from Common.LOAN_STATUS with (nolock)
			where APPLICATION_ID=@APPLICATION_ID)
		update Common.LOAN_STATUS
		set ERROR_MESSAGE=@ERROR_MESSAGE
		where APPLICATION_ID=@APPLICATION_ID
	else
		insert into Common.LOAN_STATUS (APPLICATION_ID,ERROR_MESSAGE,STATE)
		values (@APPLICATION_ID,@ERROR_MESSAGE,0)
GO
