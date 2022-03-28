if exists (select * from sys.objects where name='sp_GetApplicationForNORQRequestByID' and type='P')
	drop procedure Common.sp_GetApplicationForNORQRequestByID
GO

create procedure Common.sp_GetApplicationForNORQRequestByID(@ID	uniqueidentifier)
AS
	select ID,SOCIAL_CARD_NUMBER,DOCUMENT_TYPE_CODE,DOCUMENT_NUMBER,FIRST_NAME,LAST_NAME,BIRTH_DATE
		,COBORROWER_SOCIAL_CARD_NUMBER,COBORROWER_FIRST_NAME,COBORROWER_LAST_NAME,COBORROWER_BIRTH_DATE
		from Common.APPLICATION
		where ID=@ID and STATUS=1
GO
