CREATE OR ALTER PROCEDURE Common.sp_GetShopData(@APPLICATION_ID uniqueidentifier)
AS
	select
		su.SHOP_CODE,
		isnull(s.HEAD_CODE,'') as PARENT_SHOP_CODE,
		convert(varchar(100),isnull(Common.ahf_Unicode2ANSI(u.FIRST_NAME),'')+' '+isnull(Common.ahf_Unicode2ANSI(u.LAST_NAME),'')) as SHOP_USER_NAME
	from Common.APPLICATION a
	join Common.APPLICATION_USER u
		on a.SHOP_USER_ID=u.ID
	join IL.SHOP_USER su
		on u.ID=su.APPLICATION_USER_ID
	join IL.SHOP s
		on su.SHOP_CODE=s.CODE
	where a.ID=@APPLICATION_ID

	union all

	select
		s.CODE as SHOP_CODE,
		isnull(s.HEAD_CODE,'') as PARENT_SHOP_CODE,
		'' as SHOP_USER_NAME
	from Common.APPLICATION a
	join Common.PARTNER_COMPANY p
		on a.PARTNER_COMPANY_CODE=p.CODE
	join IL.SHOP s
		on p.SHOP_CODE=s.CODE
	where a.ID=@APPLICATION_ID
GO
