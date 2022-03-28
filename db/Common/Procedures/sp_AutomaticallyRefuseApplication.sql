if exists (select * from sys.objects where name='sp_AutomaticallyRefuseApplication' and type='P')
	drop procedure Common.sp_AutomaticallyRefuseApplication
GO

create procedure Common.sp_AutomaticallyRefuseApplication(@ID				uniqueidentifier,
														  @REFUSAL_REASON	nvarchar(100))
AS
	update Common.APPLICATION set REFUSAL_REASON=@REFUSAL_REASON,STATUS=6,TO_BE_SYNCHRONIZED=1 where ID=@ID
GO
