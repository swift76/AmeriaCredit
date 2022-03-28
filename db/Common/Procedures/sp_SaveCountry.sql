if exists (select * from sys.objects where name='sp_SaveCountry' and type='P')
	drop procedure Common.sp_SaveCountry
GO

create procedure Common.sp_SaveCountry(@CODE	char(2),
									   @NAME_AM	nvarchar(50),
									   @NAME_EN	varchar(50))
AS
	insert into Common.COUNTRY (CODE,NAME_AM,NAME_EN)
		values (@CODE,Common.ahf_ANSI2Unicode(@NAME_AM),@NAME_EN)
GO
