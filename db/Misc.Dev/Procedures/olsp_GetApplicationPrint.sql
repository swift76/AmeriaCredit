if exists (select * from sys.objects where name='olsp_GetApplicationPrint' and type='P')
	drop procedure dbo.olsp_GetApplicationPrint
GO

create procedure olsp_GetApplicationPrint(@APPLICATION_ID uniqueidentifier)
AS
	select '' as FILE_NAME,
		null as PROCESS_DATE,
		'' as ADRIVE_FILE_ID,
		'' as ADRIVE_DOCUMENT_ID,
		'' as ADRIVE_URL,
		'' as AGREEMENT_CODE
	where 0=1
GO
