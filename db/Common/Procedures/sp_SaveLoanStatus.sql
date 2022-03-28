create or alter procedure Common.sp_SaveLoanStatus(
	@APPLICATION_ID 		uniqueidentifier,
	@STATE					tinyint,
	@LOAN_AGREEMENT_CODE	nvarchar(14)=null
)
AS
	if not @LOAN_AGREEMENT_CODE is null
		set @LOAN_AGREEMENT_CODE = Common.ahf_ANSI2Unicode(@LOAN_AGREEMENT_CODE)

	if exists(select top 1 STATE
			from Common.LOAN_STATUS with (nolock)
			where APPLICATION_ID=@APPLICATION_ID)
		update Common.LOAN_STATUS
		set STATE=@STATE
			,LOAN_AGREEMENT_CODE=isnull(@LOAN_AGREEMENT_CODE,LOAN_AGREEMENT_CODE)
		where APPLICATION_ID=@APPLICATION_ID
	else
		insert into Common.LOAN_STATUS (APPLICATION_ID,STATE,LOAN_AGREEMENT_CODE)
		values (@APPLICATION_ID,@STATE,@LOAN_AGREEMENT_CODE)
GO
