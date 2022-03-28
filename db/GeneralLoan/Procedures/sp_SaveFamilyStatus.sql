if exists (select * from sys.objects where name='sp_SaveFamilyStatus' and type='P')
	drop procedure GL.sp_SaveFamilyStatus
GO

create procedure GL.sp_SaveFamilyStatus(@CODE		char(1),
										@NAME_AM	nvarchar(50),
										@NAME_EN	varchar(50))
AS
	insert into GL.FAMILY_STATUS (CODE,NAME_AM,NAME_EN)
		values (@CODE,Common.ahf_ANSI2Unicode(@NAME_AM),@NAME_EN)
GO
