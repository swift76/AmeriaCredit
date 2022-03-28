if exists (select * from sys.objects where name='sp_ChangeApplicationStatusByISN' and type='P')
	drop procedure Common.sp_ChangeApplicationStatusByISN
GO

create procedure Common.sp_ChangeApplicationStatusByISN(@ISN			int,
														@STATUS			tinyint,
														@REFUSAL_REASON	varchar(100))
AS
	if @STATUS=10
	begin
		declare @LOAN_TYPE_ID char(2),@HAS_BANK_CARD bit
		select @LOAN_TYPE_ID=LOAN_TYPE_ID,@HAS_BANK_CARD=HAS_BANK_CARD
			from Common.APPLICATION with (UPDLOCK) where ISN=@ISN

		if @LOAN_TYPE_ID='00'
			set @STATUS=15
		else
		begin
			if @HAS_BANK_CARD=1
				set @STATUS=11
			else
				set @STATUS=12
		end
	end

	update Common.APPLICATION set
		STATUS=@STATUS,
		REFUSAL_REASON=case isnull(@REFUSAL_REASON,'') when '' then REFUSAL_REASON else Common.ahf_ANSI2Unicode(@REFUSAL_REASON) end,
		TO_BE_SYNCHRONIZED=0
	where ISN=@ISN
GO
