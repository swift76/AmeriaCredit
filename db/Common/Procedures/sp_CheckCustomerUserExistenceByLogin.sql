if exists (select * from sys.objects where name='sp_CheckCustomerUserExistenceByLogin' and type='P')
	drop procedure Common.sp_CheckCustomerUserExistenceByLogin
GO

create procedure Common.sp_CheckCustomerUserExistenceByLogin(@MOBILE_PHONE	char(11))
AS
	select ID
	from Common.APPLICATION_USER
	where LOGIN = @MOBILE_PHONE
		and USER_ROLE_ID=3
GO
