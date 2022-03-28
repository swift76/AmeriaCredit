if exists (select * from sys.objects where name='sp_ResetUserPassword' and type='P')
	drop procedure IL.sp_ResetUserPassword
GO

create procedure IL.sp_ResetUserPassword(@ID	int)
AS
	update Common.APPLICATION_USER
		set HASH='Hyc2TfluirwkJuvgdKXGT2ddD10='
		where ID=@ID
GO
