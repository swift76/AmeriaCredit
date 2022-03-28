if exists (select * from sys.objects where name='sp_CloseShopUsers' and type='P')
	drop procedure IL.sp_CloseShopUsers
GO

create procedure IL.sp_CloseShopUsers(@SHOP_CODE	char(4))
AS
	declare @CLOSE_DATE date = getdate()
	update Common.APPLICATION_USER
	set USER_ROLE_ID=2,CLOSE_DATE=@CLOSE_DATE
	where ID in
	(
		select APPLICATION_USER_ID
		from IL.SHOP_USER
		where SHOP_CODE = @SHOP_CODE

		union all

		select u.APPLICATION_USER_ID
		from IL.SHOP s
		join IL.SHOP_USER u
			on u.SHOP_CODE=s.CODE
		where s.HEAD_CODE = @SHOP_CODE
	)
GO
