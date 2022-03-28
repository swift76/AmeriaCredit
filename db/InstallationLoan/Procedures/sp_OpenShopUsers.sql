if exists (select * from sys.objects where name='sp_OpenShopUsers' and type='P')
	drop procedure IL.sp_OpenShopUsers
GO

create procedure IL.sp_OpenShopUsers(@SHOP_CODE		char(4),
									 @CLOSE_DATE	date)
AS
	update Common.APPLICATION_USER
	set USER_ROLE_ID=1,CLOSE_DATE=null
	where CLOSE_DATE>@CLOSE_DATE and ID in
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
