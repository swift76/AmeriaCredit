if exists (select * from sys.objects where name='sp_GetApplications' and type='P')
	drop procedure Common.sp_GetApplications
GO

create procedure Common.sp_GetApplications(@CUSTOMER_USER_ID	int)
AS
	select a.LOAN_TYPE_ID,
		   a.CREATION_DATE,
		   convert(varchar(15), FORMAT(cast(isnull(c.FINAL_AMOUNT,a.INITIAL_AMOUNT) as numeric), '###,###,###')) +
				' ' + isnull(a.CURRENCY_CODE,'') as DISPLAY_AMOUNT,
		   a.STATUS as STATUS_ID,
		   a.ID,
		   case isnull(s.UI_NAME_AM,'') when '' then s.NAME_AM else s.UI_NAME_AM end +
		   case
		       when a.STATUS in (5,8) then ' /' + Common.f_GetApprovedAmount(a.ID,a.LOAN_TYPE_ID,a.CURRENCY_CODE) + '/'
--			   when isnull(a.REFUSAL_REASON,'')<>'' then ' /' + a.REFUSAL_REASON + '/'
			   else ''
		   end as STATUS_AM,
		   s.NAME_EN as STATUS_EN,
		   t.NAME_AM as LOAN_TYPE_AM,
		   t.NAME_EN as LOAN_TYPE_EN,
		   cu.ONBOARDING_ID
	from Common.APPLICATION a with (NOLOCK)
	join Common.APPLICATION_STATUS s with (NOLOCK)
		on a.STATUS = convert(tinyint,s.CODE)
	join Common.LOAN_TYPE t with (NOLOCK)
		on a.LOAN_TYPE_ID = t.CODE
	left join Common.COMPLETED_APPLICATION as c with (NOLOCK)
		on a.ID = c.APPLICATION_ID
	left join Common.CUSTOMER_USER as cu with (NOLOCK)
		on a.CUSTOMER_USER_ID = cu.APPLICATION_USER_ID
	where a.CUSTOMER_USER_ID = @CUSTOMER_USER_ID
	order by a.CREATION_DATE desc
GO
