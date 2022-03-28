if exists (select * from sys.objects where name='sp_SaveMonthlyNetSalary' and type='P')
	drop procedure Common.sp_SaveMonthlyNetSalary
GO

create procedure Common.sp_SaveMonthlyNetSalary(@CODE		char(1),
												@NAME_AM	nvarchar(50),
												@NAME_EN	varchar(50))
AS
	insert into Common.MONTHLY_NET_SALARY (CODE,NAME_AM,NAME_EN)
		values (@CODE,Common.ahf_ANSI2Unicode(@NAME_AM),@NAME_EN)
GO
