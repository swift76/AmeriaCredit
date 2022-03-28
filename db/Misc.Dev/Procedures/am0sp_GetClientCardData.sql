if exists (select * from sys.objects where name='am0sp_GetClientCardData' and type='P')
	drop procedure dbo.am0sp_GetClientCardData
GO

create procedure am0sp_GetClientCardData(@CLICODE	char(8),
										 @CARDNUM	char(16),
										 @EXPIRY	smalldatetime)
AS
	select
		'JOHN SMITH' as EmbossedName,
		'37455555555' as MobilePhone
	where @CARDNUM='9051190400000020'
GO
