if exists (select * from sys.objects where name='sp_GetACRAResponseForCRM' and type='P')
	drop procedure Common.sp_GetACRAResponseForCRM
GO

create procedure Common.sp_GetACRAResponseForCRM(@ID uniqueidentifier)
AS
	select top 1 RESPONSE_XML
	from Common.ACRA_QUERY_RESULT
	where APPLICATION_ID=@ID
GO
