if exists (select * from sys.objects where name='sp_UpdateApplicationScan' and type='P')
	drop procedure Common.sp_UpdateApplicationScan
GO

create procedure Common.sp_UpdateApplicationScan(@APPLICATION_ID				uniqueidentifier,
												 @APPLICATION_SCAN_TYPE_CODE	char(1),
												 @FILE_NAME						nvarchar(250),
												 @CONTENT						varbinary(max))
AS
	update Common.APPLICATION_SCAN
	set    CONTENT = @CONTENT, FILE_NAME = @FILE_NAME
	where  APPLICATION_ID = @APPLICATION_ID AND APPLICATION_SCAN_TYPE_CODE = @APPLICATION_SCAN_TYPE_CODE
GO
