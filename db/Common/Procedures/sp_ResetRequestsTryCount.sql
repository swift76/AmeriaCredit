if exists (select * from sys.objects where name='sp_ResetRequestsTryCount' and type='P')
	drop procedure Common.sp_ResetRequestsTryCount
GO

create procedure Common.sp_ResetRequestsTryCount(@ISN	int)
AS
	update Common.APPLICATION set
		NORQ_TRY_COUNT=0,
		ACRA_TRY_COUNT=0
	where ISN=@ISN
GO
