create or alter procedure GL.sp_SavePledgeAgreementDate(
	@APPLICATION_ID 		uniqueidentifier,
	@PLEDGE_AGREEMENT_DATE	datetime
)
AS
	update GL.AGREED_APPLICATION
	set PLEDGE_AGREEMENT_DATE=@PLEDGE_AGREEMENT_DATE
	where APPLICATION_ID=@APPLICATION_ID
GO
