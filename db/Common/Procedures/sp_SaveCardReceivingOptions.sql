if exists (select * from sys.objects where name='sp_SaveCardReceivingOptions' and type='P')
	drop procedure Common.sp_SaveCardReceivingOptions
GO

create procedure Common.sp_SaveCardReceivingOptions(@CODE		char(2),
										   			@NAME_AM	nvarchar(50),
										   			@NAME_EN	varchar(50))
AS
	insert into Common.CARD_RECEIVING_OPTIONS (CODE,NAME_AM,NAME_EN)
		values (@CODE,Common.ahf_ANSI2Unicode(@NAME_AM),@NAME_EN)
GO
