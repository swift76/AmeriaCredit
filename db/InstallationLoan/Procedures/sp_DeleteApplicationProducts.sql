if exists (select * from sys.objects where name='sp_DeleteApplicationProducts' and type='P')
	drop procedure IL.sp_DeleteApplicationProducts
GO

create procedure IL.sp_DeleteApplicationProducts(@APPLICATION_ID	uniqueidentifier)

AS
	delete from IL.APPLICATION_PRODUCT
	where APPLICATION_ID = @APPLICATION_ID
GO
