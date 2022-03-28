if exists (select * from sysobjects where id = object_id('dbo.am0sp_IsClientStudent') and sysstat & 0xf = 4)
	drop procedure dbo.am0sp_IsClientStudent
GO

CREATE PROCEDURE am0sp_IsClientStudent(@SocialCardCode	char(20))
AS
	SELECT 0
GO
