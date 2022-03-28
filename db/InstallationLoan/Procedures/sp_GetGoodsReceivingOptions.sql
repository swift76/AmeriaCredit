if exists (select * from sys.objects where name='sp_GetGoodsReceivingOptions' and type='P')
	drop procedure IL.sp_GetGoodsReceivingOptions
GO

create procedure IL.sp_GetGoodsReceivingOptions(@LANGUAGE_CODE	char(2))
AS
	select CODE,
		case @LANGUAGE_CODE
			when 'AM' then NAME_AM
			else NAME_EN
		end as NAME
	from IL.GOODS_RECEIVING_OPTIONS
GO
