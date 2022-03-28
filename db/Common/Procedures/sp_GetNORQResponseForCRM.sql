if exists (select * from sys.objects where name='sp_GetNORQResponseForCRM' and type='P')
	drop procedure Common.sp_GetNORQResponseForCRM
GO

create procedure Common.sp_GetNORQResponseForCRM(@ID uniqueidentifier)
AS
	select top 1 RESPONSE_XML
	from Common.NORQ_QUERY_RESULT
	where APPLICATION_ID=@ID
GO
