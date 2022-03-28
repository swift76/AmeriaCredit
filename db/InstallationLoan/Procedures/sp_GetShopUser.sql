if exists (select * from sys.objects where name='sp_GetShopUser' and type='P')
	drop procedure IL.sp_GetShopUser
GO

create procedure IL.sp_GetShopUser(@ID	int)
AS
	select u.ID,u.LOGIN,u.FIRST_NAME,u.LAST_NAME,u.EMAIL,u.CREATE_DATE,u.PASSWORD_EXPIRY_DATE,u.CLOSE_DATE,u.OBJECT_STATE_ID,s.SHOP_CODE,s.IS_MANAGER,s.MOBILE_PHONE
	from IL.SHOP_USER s
	join Common.APPLICATION_USER u
		on s.APPLICATION_USER_ID=u.ID
	where u.ID=@ID
GO
