if exists (select * from sys.objects where name='sp_GetManagerApplications' and type='P')
	drop procedure IL.sp_GetManagerApplications
GO

create procedure IL.sp_GetManagerApplications(@SHOP_USER_ID	int,
											  @FROM_DATE	date,
											  @TO_DATE		date,
											  @NAME			nvarchar(50) = NULL,
											  @STATUS		tinyint = NULL)
AS
	declare @SHOP_CODE char(4)
	select @SHOP_CODE=SHOP_CODE from IL.SHOP_USER where APPLICATION_USER_ID=@SHOP_USER_ID

	select 	a.ID,
			a.CREATION_DATE,
			a.STATUS as STATUS_ID,
			a.LOAN_TYPE_ID,
			rtrim(isnull(au.FIRST_NAME,'')) + ' ' + rtrim(isnull(au.LAST_NAME,'')) as SHOP_USER_NAME,
			rtrim(a.FIRST_NAME) + ' ' + rtrim(a.LAST_NAME) + ' ' + rtrim(a.PATRONYMIC_NAME) as NAME,
			convert(varchar(15), FORMAT(cast(isnull(c.FINAL_AMOUNT,a.INITIAL_AMOUNT) as numeric), '###,###,###')) as DISPLAY_AMOUNT,
			case
		       when a.STATUS in (5,8) then s.NAME_AM + ' /' + Common.f_GetApprovedAmount(a.ID,a.LOAN_TYPE_ID,a.CURRENCY_CODE) + '/'
			   when isnull(a.REFUSAL_REASON,'')<>'' then s.NAME_AM + '  /' + a.REFUSAL_REASON + '/'
			   else s.NAME_AM
		   end as STATUS_AM
	from Common.APPLICATION a with (NOLOCK)
	join Common.APPLICATION_STATUS s with (NOLOCK)
		on a.STATUS=convert(tinyint,s.CODE)
	left join IL.SHOP_USER u with (NOLOCK)
		on u.APPLICATION_USER_ID=a.SHOP_USER_ID
	left join IL.SHOP us with (NOLOCK)
		on us.CODE=u.SHOP_CODE
	left join Common.APPLICATION_USER au with (NOLOCK)
		on au.ID=u.APPLICATION_USER_ID
	left join Common.COMPLETED_APPLICATION c with (NOLOCK)
		on a.ID=c.APPLICATION_ID
	left join IL.SHOP cs with (NOLOCK)
		on cs.CODE=c.SHOP_CODE
	where @SHOP_CODE in (us.CODE,us.HEAD_CODE,cs.CODE,cs.HEAD_CODE)
		and convert(date,a.CREATION_DATE) between @FROM_DATE and @TO_DATE
		and upper(rtrim(a.FIRST_NAME) + ' ' + rtrim(a.LAST_NAME) + ' ' + rtrim(a.PATRONYMIC_NAME)) like '%' + upper(rtrim(isnull(@NAME,''))) + '%'
		and a.STATUS=isnull(@STATUS,a.STATUS)
	order by a.CREATION_DATE desc
GO
