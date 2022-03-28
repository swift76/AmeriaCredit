if exists (select * from sys.objects where name='am0sp_DoesClientWorksAtBank' and type='P')
	drop procedure dbo.am0sp_DoesClientWorksAtBank
GO

create procedure am0sp_DoesClientWorksAtBank(@SocialCardCode	nvarchar(10))
AS

SELECT 0

GO