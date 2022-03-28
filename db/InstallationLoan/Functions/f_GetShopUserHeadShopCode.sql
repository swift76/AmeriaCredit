if exists (select * from sys.objects where name='f_GetShopUserHeadShopCode' and type='FN')
	drop function IL.f_GetShopUserHeadShopCode
GO

create function IL.f_GetShopUserHeadShopCode(@ID int)
returns char(4)
AS
begin
	declare @HeadShopCode char(4)

	select @HeadShopCode = s.HEAD_CODE
	from IL.SHOP s
		join IL.SHOP_USER su
		on s.CODE = su.SHOP_CODE
	where su.APPLICATION_USER_ID = @ID

	if @HeadShopCode = ''
	begin
		select @HeadShopCode = s.CODE
		from IL.SHOP s
			join IL.SHOP_USER su
			on s.CODE = su.SHOP_CODE
		where su.APPLICATION_USER_ID = @ID
	end

	return @HeadShopCode
end
go
