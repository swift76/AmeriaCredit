create or alter procedure Common.sp_GetACRACoborrowerInterrelated(@APPLICATION_ID	uniqueidentifier)
AS
	select FROM_DATE,TO_DATE,
		case CUR when 'RUR' then 'RUB' else CUR end as CUR,
		Common.ahf_Unicode2ANSI(RISK) as RISK,CONTRACT_AMOUNT,DUE_AMOUNT,OVERDUE_AMOUNT,OUTSTANDING_PERCENT
	from Common.ACRA_COBORROWER_QUERY_RESULT_INTERRELATED
	where APPLICATION_ID=@APPLICATION_ID
GO
