create or alter procedure GL.sp_GetPledgeAgreementDate(
	@APPLICATION_ID uniqueidentifier
)
AS
	select PLEDGE_AGREEMENT_DATE
	from GL.AGREED_APPLICATION
	where APPLICATION_ID=@APPLICATION_ID
GO
