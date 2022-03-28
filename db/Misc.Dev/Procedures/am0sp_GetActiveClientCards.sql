if exists (select * from sys.objects where name='am0sp_GetActiveClientCards' and type='P')
	drop procedure dbo.am0sp_GetActiveClientCards
GO

create procedure am0sp_GetActiveClientCards(@CLICODE	nvarchar(8),
											@LOANTYPE	char(2) = null,
											@CURRENCY	char(3) = null)
AS
	select
		'9051190000000024' as CardNumber,
		N'****-****-****-0024 ԱրՔա գոլդ' as CardDescription
	union all
	select
		'9051190000000020' as CardNumber,
		N'****-****-****-0020 ԱրՔա պլատինիում' as CardDescription
GO
