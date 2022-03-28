﻿create or alter procedure Common.sp_GetApplicationForNORQRequestByISN(@ISN	int)
AS
	select ID,SOCIAL_CARD_NUMBER,DOCUMENT_TYPE_CODE,DOCUMENT_NUMBER,FIRST_NAME,LAST_NAME,BIRTH_DATE
		,COBORROWER_SOCIAL_CARD_NUMBER,COBORROWER_FIRST_NAME,COBORROWER_LAST_NAME,COBORROWER_BIRTH_DATE
		from Common.APPLICATION
		where ISN=@ISN and STATUS=1
GO
