if exists (select * from sys.objects where name='sp_FinalizeDueApplications' and type='P')
	drop procedure Common.sp_FinalizeDueApplications
GO

create procedure Common.sp_FinalizeDueApplications
AS
	declare @ILDueDays int,@GLDueDays int
	BEGIN TRY
		select @ILDueDays=EXPIRE_DAY_COUNT from IL.INSTALLATION_LOAN_SETTING
		select @GLDueDays=EXPIRE_DAY_COUNT from GL.GENERAL_LOAN_SETTING
		declare @CurrentDate date = convert(date,getdate())

		if @ILDueDays>0
			update Common.APPLICATION
			set STATUS=55,
				TO_BE_SYNCHRONIZED=1
			where LOAN_TYPE_ID='00'
				and STATUS in (1,2,3,5,7,8,10,11,12,13,15,19,20,99)
				and datediff(day, convert(date,CREATION_DATE), @CurrentDate)>@ILDueDays

		if @GLDueDays>0
			update Common.APPLICATION
			set STATUS=55,
				TO_BE_SYNCHRONIZED=1
			where LOAN_TYPE_ID<>'00'
				and STATUS in (1,2,3,5,7,8,10,11,12,13,15,19,20,99)
				and datediff(day, convert(date,CREATION_DATE), @CurrentDate)>@GLDueDays
	END TRY
	BEGIN CATCH

	END CATCH
GO
