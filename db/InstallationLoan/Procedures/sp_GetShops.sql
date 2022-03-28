if exists (select * from sys.objects where name='sp_GetShops' and type='P')
	drop procedure IL.sp_GetShops
GO

create procedure IL.sp_GetShops
AS
	select CODE,NAME,NAME_EN,HEAD_CODE,ADDRESS,ADDRESS_EN,IS_DELIVERY
	from IL.SHOP
GO
