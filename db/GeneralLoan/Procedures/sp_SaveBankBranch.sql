if exists (select * from sys.objects where name='sp_SaveBankBranch' and type='P')
	drop procedure GL.sp_SaveBankBranch
GO

create procedure GL.sp_SaveBankBranch(@CODE		char(3),
									  @NAME_AM	nvarchar(50),
									  @NAME_EN	varchar(50))
AS
	insert into GL.BANK_BRANCH (CODE,NAME_AM,NAME_EN)
		values (@CODE,Common.ahf_ANSI2Unicode(@NAME_AM),@NAME_EN)
GO
