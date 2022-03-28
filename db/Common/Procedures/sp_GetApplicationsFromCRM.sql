if exists (select * from sys.objects where name='sp_GetApplicationsFromCRM' and type='P')
	drop procedure Common.sp_GetApplicationsFromCRM
GO

create procedure Common.sp_GetApplicationsFromCRM(@FROM_DATE			date,
												  @TO_DATE				date,
												  @STATUS				char(2),
												  @SOCIAL_CARD_NUMBER	char(10))
AS
	select a.ID,
		   a.LOAN_TYPE_ID,
		   a.CREATION_DATE,
		   isnull(c.FINAL_AMOUNT,a.INITIAL_AMOUNT) as DISPLAY_AMOUNT,
		   a.CURRENCY_CODE,
		   a.STATUS,
		   case isnull(s.UI_NAME_AM,'') when '' then s.NAME_AM else s.UI_NAME_AM end as STATUS_NAME_AM,
		   case isnull(s.UI_NAME_EN,'') when '' then s.NAME_EN else s.UI_NAME_EN end as STATUS_NAME_EN,
		   max(r.AMOUNT) as APPROVED_AMOUNT,
		   t.NAME_AM as LOAN_TYPE_NAME_AM,
		   t.NAME_EN as LOAN_TYPE_NAME_EN,
		   a.REFUSAL_REASON,
		   a.MANUAL_REASON
	from Common.APPLICATION a with (NOLOCK)
	join Common.APPLICATION_STATUS s with (NOLOCK)
		on a.STATUS = convert(tinyint,s.CODE)
	join Common.LOAN_TYPE t with (NOLOCK)
		on a.LOAN_TYPE_ID = t.CODE
	left join Common.APPLICATION_SCORING_RESULT r
		on r.APPLICATION_ID = a.ID
	left join Common.COMPLETED_APPLICATION as c with (NOLOCK)
		on a.ID = c.APPLICATION_ID
	where a.CREATION_DATE between @FROM_DATE and @TO_DATE
		and a.LOAN_TYPE_ID<>'00'
		and isnull(@STATUS,a.STATUS)=a.STATUS
		and isnull(@SOCIAL_CARD_NUMBER,a.SOCIAL_CARD_NUMBER)=a.SOCIAL_CARD_NUMBER
	group by a.ID,a.LOAN_TYPE_ID,a.CREATION_DATE,c.FINAL_AMOUNT,a.INITIAL_AMOUNT,a.CURRENCY_CODE
		,a.STATUS,s.UI_NAME_AM,s.NAME_AM,s.UI_NAME_EN,s.NAME_EN,t.NAME_AM,t.NAME_EN,a.REFUSAL_REASON,a.MANUAL_REASON
	order by a.CREATION_DATE desc
GO
