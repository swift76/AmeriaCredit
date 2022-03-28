if exists (select * from sys.objects where name='f_GetApplicationUserRoleName' and type='FN')
	drop function Common.f_GetApplicationUserRoleName
GO

create function Common.f_GetApplicationUserRoleName(@ID	int)
RETURNS varchar(1000)
AS
BEGIN
	declare @UserRoleID tinyint,@UserRoleName varchar(1000)

	select @UserRoleID=USER_ROLE_ID
	from Common.APPLICATION_USER
	where ID=@ID

	if @UserRoleID=1
	begin
		declare @IsAdministrator bit
		select @IsAdministrator=IS_ADMINISTRATOR
		from IL.BANK_USER
		where APPLICATION_USER_ID=@ID

		if @IsAdministrator=1
			set @UserRoleName='BankPowerUser'
		else
			set @UserRoleName='BankUser'
	end
	else if @UserRoleID=2
	begin
		declare @IsManager bit
		select @IsManager=IS_MANAGER
		from IL.SHOP_USER
		where APPLICATION_USER_ID=@ID

		if @IsManager=1
			set @UserRoleName='ShopPowerUser'
		else
			set @UserRoleName='ShopUser'
	end
	else if @UserRoleID=3
		set @UserRoleName='Customer'

	RETURN @UserRoleName
END
GO
