if exists (select * from sys.objects where name='sp_GetShopUsers' and type='P')
	drop procedure IL.sp_GetShopUsers
GO

create procedure IL.sp_GetShopUsers
AS
	select	u.ID,
			u.LOGIN,
			u.FIRST_NAME,
			u.LAST_NAME,
			u.EMAIL,
			u.CREATE_DATE,
			u.PASSWORD_EXPIRY_DATE,
			u.CLOSE_DATE,
			u.OBJECT_STATE_ID,
			s.SHOP_CODE,
			s.IS_MANAGER,
			s.MOBILE_PHONE,
			p.NAME as SHOP_NAME,
			o.DESCRIPTION as OBJECT_STATE_DESCRIPTION
	from IL.SHOP_USER s
	join Common.APPLICATION_USER u
		on s.APPLICATION_USER_ID = u.ID
	join Common.OBJECT_STATE o
		on u.OBJECT_STATE_ID = o.ID
	join IL.SHOP p
		on p.CODE = s.SHOP_CODE
GO
