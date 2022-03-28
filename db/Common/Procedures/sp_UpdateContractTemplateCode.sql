CREATE OR ALTER PROCEDURE Common.sp_UpdateContractTemplateCode(@ID				uniqueidentifier,
												  @TEMPLATE_CODE	char(4))
AS
	update Common.APPLICATION
		set LOAN_TEMPLATE_CODE=@TEMPLATE_CODE
		where ID=@ID
GO
