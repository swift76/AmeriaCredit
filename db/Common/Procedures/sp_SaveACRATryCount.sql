if exists (select * from sys.objects where name='sp_SaveACRATryCount' and type='P')
	drop procedure Common.sp_SaveACRATryCount
GO

create procedure Common.sp_SaveACRATryCount(@APPLICATION_ID		uniqueidentifier)
AS
	update Common.APPLICATION
		set ACRA_TRY_COUNT=ACRA_TRY_COUNT+1,TO_BE_SYNCHRONIZED=1
		where ID=@APPLICATION_ID
GO
