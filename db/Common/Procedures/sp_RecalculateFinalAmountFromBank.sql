if exists (select * from sys.objects where name='sp_RecalculateFinalAmountFromBank' and type='P')
	drop procedure Common.sp_RecalculateFinalAmountFromBank
GO

create procedure Common.sp_RecalculateFinalAmountFromBank(
		@APPLICATION_ID uniqueidentifier,
		@FINAL_AMOUNT	money)

AS
	update Common.COMPLETED_APPLICATION
		set FINAL_AMOUNT=@FINAL_AMOUNT
	where APPLICATION_ID=@APPLICATION_ID
GO
