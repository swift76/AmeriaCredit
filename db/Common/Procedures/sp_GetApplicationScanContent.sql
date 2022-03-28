if exists (select * from sys.objects where name='sp_GetApplicationScanContent' and type='P')
	drop procedure Common.sp_GetApplicationScanContent
GO

create procedure Common.sp_GetApplicationScanContent(@APPLICATION_ID				uniqueidentifier,
													 @APPLICATION_SCAN_TYPE_CODE	char(1))
AS
	SELECT CONTENT, FILE_NAME
	FROM APPLICATION_SCAN
	WHERE APPLICATION_ID = @APPLICATION_ID AND APPLICATION_SCAN_TYPE_CODE = @APPLICATION_SCAN_TYPE_CODE
GO