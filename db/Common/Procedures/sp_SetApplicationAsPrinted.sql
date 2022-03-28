if exists (select * from sys.objects where name='sp_SetApplicationAsPrinted' and type='P')
	drop procedure Common.sp_SetApplicationAsPrinted
GO

create procedure Common.sp_SetApplicationAsPrinted(@APPLICATION_ID	uniqueidentifier)
AS
	update Common.APPLICATION set PRINT_EXISTS=1
	where ID=@APPLICATION_ID
GO
