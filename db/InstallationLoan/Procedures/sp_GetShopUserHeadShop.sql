create or alter procedure IL.sp_GetShopUserHeadShop(@ID int)
AS
	declare @HeadShopCode char(4)=IL.f_GetShopUserHeadShopCode(@ID)
	select CODE,NAME,NAME_EN,HEAD_CODE,ADDRESS,ADDRESS_EN,IS_DELIVERY
	from IL.SHOP with (nolock)
	where CODE=@HeadShopCode
GO
