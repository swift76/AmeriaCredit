if exists (select * from sys.objects where name='sp_GetClientDataForCardValidation' and type='P')
	drop procedure Common.sp_GetClientDataForCardValidation
GO

create procedure Common.sp_GetClientDataForCardValidation(@APPLICATION_ID	uniqueidentifier)

AS
	select a.CLIENT_CODE,
		c.FIRST_NAME_EN,
		c.LAST_NAME_EN,
		a.LOAN_TYPE_ID,
		a.CURRENCY_CODE
	from Common.APPLICATION a
	left join Common.COMPLETED_APPLICATION c
		on c.APPLICATION_ID = a.ID
	where a.ID = @APPLICATION_ID
GO
