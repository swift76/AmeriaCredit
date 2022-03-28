if exists (select * from sys.objects where name='sp_CreateApplicationScan' and type='P')
	drop procedure Common.sp_CreateApplicationScan
GO

create procedure Common.sp_CreateApplicationScan(@APPLICATION_ID				uniqueidentifier,
												 @APPLICATION_SCAN_TYPE_CODE 	char(1),
												 @FILE_NAME						nvarchar(250),
												 @CONTENT						varbinary(max))
AS
	insert into Common.APPLICATION_SCAN
		(APPLICATION_ID, APPLICATION_SCAN_TYPE_CODE, FILE_NAME, CONTENT)
	values
		(@APPLICATION_ID, @APPLICATION_SCAN_TYPE_CODE, @FILE_NAME, @CONTENT)
GO