if exists (select * from sys.objects where name='sp_LockApplicationByID' and type='P')
	drop procedure Common.sp_LockApplicationByID
GO

create procedure Common.sp_LockApplicationByID(@ID		uniqueidentifier,
											   @STATUS	tinyint)
AS
	select ID
		from Common.APPLICATION with (UPDLOCK)
		where ID=@ID and STATUS=@STATUS
GO
