if exists (select * from sys.objects where name='sp_GetApplicationStatus' and type='P')
	drop procedure Common.sp_GetApplicationStatus
GO

create procedure Common.sp_GetApplicationStatus(
	@ID				uniqueidentifier,
	@STATUS			tinyint OUTPUT,
	@PRINT_EXISTS	bit OUTPUT,
	@CLIENT_CODE	char(8) OUTPUT
)
AS
	select @STATUS=STATUS,
		@PRINT_EXISTS=PRINT_EXISTS,
		@CLIENT_CODE=CLIENT_CODE
	from Common.APPLICATION
	where ID = @ID
GO
