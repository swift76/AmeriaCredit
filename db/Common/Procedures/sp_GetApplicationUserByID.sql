if exists (select * from sys.objects where name='sp_GetApplicationUserByID' and type='P')
	drop procedure Common.sp_GetApplicationUserByID
GO

create procedure Common.sp_GetApplicationUserByID(@ID	int)
AS
	select ID,LOGIN,FIRST_NAME,LAST_NAME,EMAIL,CREATE_DATE,PASSWORD_EXPIRY_DATE,CLOSE_DATE,OBJECT_STATE_ID
	from Common.APPLICATION_USER
	where ID=@ID
GO
