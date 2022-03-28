if exists (select * from sys.objects where name='sp_AuthenticateApplicationUser' and type='P')
	drop procedure Common.sp_AuthenticateApplicationUser
GO

create procedure Common.sp_AuthenticateApplicationUser(@LOGIN	varchar(50),
													   @HASH	varchar(1000))
AS
	select ID,
		LOGIN,
		FIRST_NAME,
		LAST_NAME,
		EMAIL,
		CREATE_DATE,
		PASSWORD_EXPIRY_DATE,
		CLOSE_DATE,
		OBJECT_STATE_ID,
		USER_ROLE_ID
	from Common.APPLICATION_USER
	where upper(LOGIN)=upper(@LOGIN) and HASH=@HASH and OBJECT_STATE_ID=1
GO
