create or alter procedure Common.sp_SavePledgeStatus(
	@APPLICATION_ID 		uniqueidentifier,
	@STATE					tinyint,
	@PLEDGE_AGREEMENT_CODE	nvarchar(14)=null
)
AS
	if not @PLEDGE_AGREEMENT_CODE is null
		set @PLEDGE_AGREEMENT_CODE = Common.ahf_ANSI2Unicode(@PLEDGE_AGREEMENT_CODE)

	if exists(select top 1 STATE
			from Common.LOAN_STATUS with (nolock)
			where APPLICATION_ID=@APPLICATION_ID)
		update Common.LOAN_STATUS
		set STATE=@STATE
			,PLEDGE_AGREEMENT_CODE=isnull(@PLEDGE_AGREEMENT_CODE,@PLEDGE_AGREEMENT_CODE)
		where APPLICATION_ID=@APPLICATION_ID
	else
		insert into Common.LOAN_STATUS (APPLICATION_ID,STATE,PLEDGE_AGREEMENT_CODE)
		values (@APPLICATION_ID,@STATE,@PLEDGE_AGREEMENT_CODE)
GO
