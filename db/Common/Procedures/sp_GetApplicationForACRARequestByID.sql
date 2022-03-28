if exists (select * from sys.objects where name='sp_GetApplicationForACRARequestByID' and type='P')
	drop procedure Common.sp_GetApplicationForACRARequestByID
GO

create procedure Common.sp_GetApplicationForACRARequestByID(@ID	uniqueidentifier)
AS
	select a.ID,n.FIRST_NAME,n.LAST_NAME,n.BIRTH_DATE,n.PASSPORT_NUMBER,n.SOCIAL_CARD_NUMBER,case a.DOCUMENT_TYPE_CODE when '2' then a.DOCUMENT_NUMBER else '' end as ID_CARD_NUMBER
		,isnull(a.IMPORT_ID,0) as IMPORT_ID
		,nc.FIRST_NAME as COBORROWER_FIRST_NAME,nc.LAST_NAME as COBORROWER_LAST_NAME,nc.BIRTH_DATE as COBORROWER_BIRTH_DATE,nc.SOCIAL_CARD_NUMBER as COBORROWER_SOCIAL_CARD_NUMBER
		,coalesce(nc.PASSPORT_NUMBER,nc.BIOMETRIC_PASSPORT_NUMBER,nc.ID_CARD_NUMBER,'') as COBORROWER_PASSPORT_NUMBER
		,isnull(nc.ID_CARD_NUMBER,'') as COBORROWER_ID_CARD_NUMBER
	from Common.APPLICATION a
	join Common.NORQ_QUERY_RESULT n
		on n.APPLICATION_ID=a.ID
	left join Common.NORQ_COBORROWER_QUERY_RESULT nc
		on nc.APPLICATION_ID=a.ID
	where a.ID=@ID and a.STATUS=2
GO
