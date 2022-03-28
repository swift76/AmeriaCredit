if exists (select * from sys.objects where name='am0sp_GetClientData' and type='P')
	drop procedure dbo.am0sp_GetClientData
GO

create procedure am0sp_GetClientData(@CLICODE	nvarchar(8))
AS
	select
		'37455555555' as MobilePhone
GO
