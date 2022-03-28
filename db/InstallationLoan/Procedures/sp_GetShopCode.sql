if exists (select * from sys.objects where name='sp_GetShopCode' and type='P')
	drop procedure IL.sp_GetShopCode
GO

create procedure IL.sp_GetShopCode(@ID int)
AS
	select SHOP_CODE
	from IL.SHOP_USER
	where APPLICATION_USER_ID = @ID
GO
