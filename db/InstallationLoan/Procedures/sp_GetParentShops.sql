if exists (select * from sys.objects where name='sp_GetParentShops' and type='P')
	drop procedure IL.sp_GetParentShops
GO

create procedure IL.sp_GetParentShops
AS
	select CODE, NAME, NAME_EN, ADDRESS, ADDRESS_EN, IS_DELIVERY
	from IL.SHOP
	where HEAD_CODE = ''
GO
