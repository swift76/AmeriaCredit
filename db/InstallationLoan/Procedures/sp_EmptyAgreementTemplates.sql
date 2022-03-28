if exists (select * from sys.objects where name='sp_EmptyAgreementTemplates' and type='P')
	drop procedure IL.sp_EmptyAgreementTemplates
GO

create procedure IL.sp_EmptyAgreementTemplates
AS
	truncate table IL.AGREEMENT_TEMPLATE
GO
