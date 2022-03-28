if exists (select * from sys.objects where name='sp_UpdateShopSequenceNumber' and type='P')
	drop procedure IL.sp_UpdateShopSequenceNumber
GO

create procedure IL.sp_UpdateShopSequenceNumber(@APPLICATION_ID	uniqueidentifier)
AS
	declare @SHOP_CODE char(4)
	select @SHOP_CODE = SHOP_CODE
	from Common.COMPLETED_APPLICATION
	where APPLICATION_ID = @APPLICATION_ID

	declare @SEQUENCE_NUMBER int
	select @SEQUENCE_NUMBER = SEQUENCE_NUMBER
	from IL.SHOP
	where CODE = @SHOP_CODE

	update Common.COMPLETED_APPLICATION set
	SHOP_SEQUENCE_NUMBER = @SHOP_CODE + '/' + right('0000000'+cast(@SEQUENCE_NUMBER as varchar),7)
	where APPLICATION_ID = @APPLICATION_ID

	update IL.SHOP set
	SEQUENCE_NUMBER = @SEQUENCE_NUMBER + 1
	where CODE = @SHOP_CODE
GO
