if exists (select * from sys.objects where name='sp_GetDocumentTypes' and type='P')
	drop procedure Common.sp_GetDocumentTypes
GO

create procedure Common.sp_GetDocumentTypes(@LANGUAGE_CODE	char(2))
AS
	select CODE,
		case @LANGUAGE_CODE
			when 'AM' then NAME_AM
			else NAME_EN
		end as NAME
	from Common.DOCUMENT_TYPE
GO
