CREATE OR ALTER PROCEDURE Common.sp_GetShopUsers(@SHOP_CODE	char(4),
									@LOGIN		varchar(50),
									@FIRST_NAME	varchar(50),
									@LAST_NAME	varchar(50))
AS
	select au.ID,au.LOGIN,Common.ahf_Unicode2ANSI(au.FIRST_NAME) as FNAME,Common.ahf_Unicode2ANSI(au.LAST_NAME) as LNAME,su.SHOP_CODE as SHOP,su.IS_MANAGER as MANAGER
		from IL.SHOP_USER su
		join Common.APPLICATION_USER au
			on su.APPLICATION_USER_ID=au.ID
	where au.USER_ROLE_ID=2
		and su.SHOP_CODE=(case isnull(@SHOP_CODE,'') when '' then su.SHOP_CODE else @SHOP_CODE end)
		and au.LOGIN=(case isnull(@LOGIN,'') when '' then au.LOGIN else @LOGIN end)
		and au.FIRST_NAME=(case isnull(@FIRST_NAME,'') when '' then au.FIRST_NAME else Common.ahf_ANSI2Unicode(@FIRST_NAME) end)
		and au.LAST_NAME=(case isnull(@LAST_NAME,'') when '' then au.LAST_NAME else Common.ahf_ANSI2Unicode(@LAST_NAME) end)
GO
