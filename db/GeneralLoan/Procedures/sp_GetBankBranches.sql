if exists (select * from sys.objects where name='sp_GetBankBranches' and type='P')
	drop procedure GL.sp_GetBankBranches
GO

create procedure GL.sp_GetBankBranches(@LANGUAGE_CODE	char(2))
AS
	select CODE,
		case @LANGUAGE_CODE
			when 'AM' then NAME_AM
			else NAME_EN
		end as NAME
	from GL.BANK_BRANCH
GO
