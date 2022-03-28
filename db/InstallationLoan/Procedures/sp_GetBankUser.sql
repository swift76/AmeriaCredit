if exists (select * from sys.objects where name='sp_GetBankUser' and type='P')
	drop procedure IL.sp_GetBankUser
GO

create procedure IL.sp_GetBankUser(@ID	int)
AS
	select u.ID,u.LOGIN,u.FIRST_NAME,u.LAST_NAME,u.EMAIL,u.CREATE_DATE,u.PASSWORD_EXPIRY_DATE,u.CLOSE_DATE,u.OBJECT_STATE_ID,b.IS_ADMINISTRATOR
	from IL.BANK_USER b
	join Common.APPLICATION_USER u
		on b.APPLICATION_USER_ID=u.ID
	where u.ID=@ID
GO
