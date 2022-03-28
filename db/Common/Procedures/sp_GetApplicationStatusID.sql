if exists (select * from sys.objects where name='sp_GetApplicationStatusID' and type='P')
	drop procedure Common.sp_GetApplicationStatusID
GO

create procedure Common.sp_GetApplicationStatusID(@ID	uniqueidentifier)
AS
	select STATUS
	from Common.APPLICATION
	where ID = @ID
GO
