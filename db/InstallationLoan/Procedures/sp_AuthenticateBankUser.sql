if exists (select * from sys.objects where name='sp_AuthenticateBankUser' and type='P')
	drop procedure IL.sp_AuthenticateBankUser
GO

create procedure IL.sp_AuthenticateBankUser(@LOGIN	varchar(50),
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
		   b.IS_ADMINISTRATOR
	from IL.BANK_USER b
	join Common.APPLICATION_USER u
		on b.APPLICATION_USER_ID=u.ID
	where upper(u.LOGIN) = upper(@LOGIN) and u.HASH = @HASH and u.USER_ROLE_ID = 1
GO
