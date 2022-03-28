create or alter procedure Common.sp_GetNORQCoborrowerWork(@APPLICATION_ID	uniqueidentifier)
AS
	select Common.ahf_Unicode2ANSI(ORGANIZATION_NAME) as ORGANIZATION_NAME
		,TAX_ID_NUMBER
		,Common.ahf_Unicode2ANSI(ADDRESS) as ADDRESS
		,format(FROM_DATE,'dd/MM/yyyy') as FROM_DATE
		,format(TO_DATE,'dd/MM/yyyy') as TO_DATE
		,SALARY
		,SOCIAL_PAYMENT
		,Common.ahf_Unicode2ANSI(POSITION) as POSITION
	from Common.NORQ_COBORROWER_QUERY_RESULT_WORK
	where APPLICATION_ID=@APPLICATION_ID
GO
