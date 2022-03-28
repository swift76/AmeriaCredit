if exists (select * from sys.objects where name='sp_GetApplicationUser' and type='P')
	drop procedure Common.sp_GetApplicationUser
GO

create procedure Common.sp_GetApplicationUser(@LOGIN	varchar(50))
AS
	select ID,LOGIN,FIRST_NAME,LAST_NAME,EMAIL,CREATE_DATE,PASSWORD_EXPIRY_DATE,CLOSE_DATE,OBJECT_STATE_ID
	from Common.APPLICATION_USER
	where LOGIN=@LOGIN
GO
