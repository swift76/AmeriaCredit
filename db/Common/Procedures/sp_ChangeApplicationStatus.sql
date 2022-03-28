if exists (select * from sys.objects where name='sp_ChangeApplicationStatus' and type='P')
	drop procedure Common.sp_ChangeApplicationStatus
GO

create procedure Common.sp_ChangeApplicationStatus(@APPLICATION_ID	uniqueidentifier,
												   @STATUS			tinyint,
												   @TO_SYNCHRONIZE	bit = null,
												   @PREVIOUS_STATUS	tinyint = null)
AS
	if not @PREVIOUS_STATUS is null
	begin
		declare @CURRENT_STATUS tinyint

		select @CURRENT_STATUS=STATUS
		from Common.APPLICATION
		where ID=@APPLICATION_ID

		if @PREVIOUS_STATUS<>@CURRENT_STATUS
			return
	end
	declare @CurrentDate datetime = getdate()
	declare @TO_BE_SYNCHRONIZED bit
	if @STATUS in (2,3,12,13,15,16,17,19,21)
		set @TO_BE_SYNCHRONIZED=1
	else
	begin
		if @STATUS=10
		begin
			declare @LOAN_TYPE_ID char(2),@HAS_BANK_CARD bit
				,@PLEDGE_TYPE char(1),@ONBOARDING_ID uniqueidentifier
			select @LOAN_TYPE_ID=a.LOAN_TYPE_ID
				,@HAS_BANK_CARD=a.HAS_BANK_CARD
				,@PLEDGE_TYPE=t.PLEDGE_TYPE
				,@ONBOARDING_ID=o.ID
			from Common.APPLICATION a with (UPDLOCK)
			join Common.LOAN_TYPE t with (NOLOCK)
				on a.LOAN_TYPE_ID=t.CODE
			left join Common.CUSTOMER_USER u with (NOLOCK)
				on a.CUSTOMER_USER_ID=u.APPLICATION_USER_ID
			left join Common.ONBOARDING_CUSTOMER o with (NOLOCK)
				on u.ONBOARDING_ID=o.ID and o.CREATION_DATE>=dateadd(day,-2,@CurrentDate)
			where a.ID=@APPLICATION_ID

			if @LOAN_TYPE_ID='00' or isnull(@PLEDGE_TYPE,'')<>''
				set @STATUS=15
			else
			begin
				declare @OFFLINE_AUTHENTICATION bit
				select @OFFLINE_AUTHENTICATION=convert(bit,VALUE) from Common.SETTING where CODE='OFFLINE_AUTHENTICATION'

				if not @ONBOARDING_ID is null and @OFFLINE_AUTHENTICATION=0
					set @STATUS=13
				else if @HAS_BANK_CARD=1 and @OFFLINE_AUTHENTICATION=0
					set @STATUS=11
				else
					set @STATUS=12
			end
			set @TO_BE_SYNCHRONIZED=1
		end
		else
			set @TO_BE_SYNCHRONIZED=null
	end

	update Common.APPLICATION set
		STATUS=@STATUS,
		TO_BE_SYNCHRONIZED=
			case
				when @TO_SYNCHRONIZE is null then isnull(@TO_BE_SYNCHRONIZED,TO_BE_SYNCHRONIZED)
				else @TO_SYNCHRONIZE
			end
	where ID=@APPLICATION_ID
GO
