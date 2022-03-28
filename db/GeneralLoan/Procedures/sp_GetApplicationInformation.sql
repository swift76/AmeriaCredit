if exists (select * from sys.objects where name='sp_GetApplicationInformation' and type='P')
	drop procedure GL.sp_GetApplicationInformation
GO

create procedure GL.sp_GetApplicationInformation(@APPLICATION_ID	uniqueidentifier)
AS
	select	STATUS as STATUS_ID,
			LOAN_TYPE_ID,
			REFUSAL_REASON,
			MANUAL_REASON,
			Common.f_GetApprovedAmount(ID, LOAN_TYPE_ID, CURRENCY_CODE) as APPROVED_AMOUNT
	from Common.APPLICATION
	where ID = @APPLICATION_ID
GO
