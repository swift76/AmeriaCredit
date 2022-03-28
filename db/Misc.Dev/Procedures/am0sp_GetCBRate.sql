if exists (select * from sys.objects where name='am0sp_GetCBRate' and type='P')
	drop procedure dbo.am0sp_GetCBRate
GO

create procedure am0sp_GetCBRate(@CURRENCY	char(3))
AS
	declare @RESULT money=1
	select @RESULT as VALUE
GO
