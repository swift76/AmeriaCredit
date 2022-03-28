if exists (select * from sys.objects where name='sp_GetApplicationsForNORQRequest' and type='P')
	drop procedure Common.sp_GetApplicationsForNORQRequest
GO

create procedure Common.sp_GetApplicationsForNORQRequest
AS
	declare @NORQ_TRY_COUNT	tinyint
	select @NORQ_TRY_COUNT=convert(tinyint,VALUE)
		from Common.SETTING
		where CODE='NORQ_TRY_COUNT'

	select ID,SOCIAL_CARD_NUMBER,DOCUMENT_TYPE_CODE,DOCUMENT_NUMBER,FIRST_NAME,LAST_NAME,BIRTH_DATE
		,COBORROWER_SOCIAL_CARD_NUMBER,COBORROWER_FIRST_NAME,COBORROWER_LAST_NAME,COBORROWER_BIRTH_DATE
		from Common.APPLICATION
		where STATUS=1 and NORQ_TRY_COUNT<@NORQ_TRY_COUNT
	order by CREATION_DATE
GO
