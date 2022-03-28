if exists (select * from sys.objects where name='sp_SaveApplicationRefinancingTotal' and type='P')
	drop procedure Common.sp_SaveApplicationRefinancingTotal
GO

create procedure Common.sp_SaveApplicationRefinancingTotal(@ID							uniqueidentifier,
														   @REFINANCING_LOANS_AMOUNT	money,
														   @REFINANCING_LOANS_COUNT		int)
AS
	update Common.APPLICATION
	set REFINANCING_LOANS_AMOUNT = @REFINANCING_LOANS_AMOUNT
		,REFINANCING_LOANS_COUNT = @REFINANCING_LOANS_COUNT
	where ID=@ID
GO
