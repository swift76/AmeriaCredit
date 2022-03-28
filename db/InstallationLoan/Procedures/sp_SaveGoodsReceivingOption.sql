if exists (select * from sys.objects where name='sp_SaveGoodsReceivingOption' and type='P')
	drop procedure IL.sp_SaveGoodsReceivingOption
GO

create procedure IL.sp_SaveGoodsReceivingOption(@CODE		char(1),
												@NAME_AM	nvarchar(50),
												@NAME_EN	varchar(50))
AS
	insert into IL.GOODS_RECEIVING_OPTIONS (CODE,NAME_AM,NAME_EN)
		values (@CODE,Common.ahf_ANSI2Unicode(@NAME_AM),@NAME_EN)
GO
