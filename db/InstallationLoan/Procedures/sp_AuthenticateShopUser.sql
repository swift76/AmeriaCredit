if exists (select * from sys.objects where name='sp_AuthenticateShopUser' and type='P')
	drop procedure IL.sp_AuthenticateShopUser
GO

create procedure IL.sp_AuthenticateShopUser(@LOGIN	varchar(50),
					 					    @HASH	varchar(1000))
AS
	select u.ID,
	       u.LOGIN,
		   u.FIRST_NAME,
		   u.LAST_NAME,
		   u.CREATE_DATE,
		   u.PASSWORD_EXPIRY_DATE,
		   u.CLOSE_DATE,
		   u.OBJECT_STATE_ID,
		   s.SHOP_CODE,
		   s.IS_MANAGER,
		   s.MOBILE_PHONE
	from IL.SHOP_USER s
	join Common.APPLICATION_USER u
		on s.APPLICATION_USER_ID = u.ID
	where upper(u.LOGIN) = upper(@LOGIN) and u.HASH = @HASH and u.USER_ROLE_ID = 2
GO
