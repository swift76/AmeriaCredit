if exists (select * from sys.objects where name='sp_GetBankUsers' and type='P')
	drop procedure IL.sp_GetBankUsers
GO

create procedure IL.sp_GetBankUsers
AS
	select u.ID,u.LOGIN,u.FIRST_NAME,u.LAST_NAME,u.EMAIL,u.CREATE_DATE,u.PASSWORD_EXPIRY_DATE,u.CLOSE_DATE,u.OBJECT_STATE_ID,b.IS_ADMINISTRATOR
		,o.DESCRIPTION as OBJECT_STATE_DESCRIPTION
	from IL.BANK_USER b
	join Common.APPLICATION_USER u
		on b.APPLICATION_USER_ID=u.ID
	join Common.OBJECT_STATE o
		on u.OBJECT_STATE_ID=o.ID
GO
