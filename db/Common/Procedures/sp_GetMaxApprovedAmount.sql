if exists (select * from sys.objects where name='sp_GetMaxApprovedAmount' and type='P')
	drop procedure Common.sp_GetMaxApprovedAmount
GO

create procedure Common.sp_GetMaxApprovedAmount(@APPLICATION_ID	uniqueidentifier)
AS
	select max(AMOUNT)
	from Common.APPLICATION_SCORING_RESULT
	where APPLICATION_ID = @APPLICATION_ID
GO
