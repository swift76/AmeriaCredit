if exists (select * from sys.objects where name='sp_AssignClientCode' and type='P')
	drop procedure Common.sp_AssignClientCode
GO

create procedure Common.sp_AssignClientCode(@APPLICATION_ID	uniqueidentifier,
											@CLIENT_CODE	char(8))
AS
	update Common.APPLICATION set CLIENT_CODE=@CLIENT_CODE
	where ID=@APPLICATION_ID
GO
