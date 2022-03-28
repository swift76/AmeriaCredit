if exists (select * from sys.objects where name='sp_GetOperatorApplications' and type='P')
	drop procedure IL.sp_GetOperatorApplications
GO

create procedure IL.sp_GetOperatorApplications(@SHOP_USER_ID	int,
											   @FROM_DATE		date,
											   @TO_DATE			date,
											   @NAME			nvarchar(50) = NULL,
											   @STATUS			tinyint = NULL)
AS
	declare @HEAD_CODE char(4), @IS_DELIVERY_SHOP_USER bit
	select @HEAD_CODE = s.HEAD_CODE, @IS_DELIVERY_SHOP_USER = s.IS_DELIVERY
	from IL.SHOP_USER su
	join IL.SHOP s
		on su.SHOP_CODE = s.CODE
	where APPLICATION_USER_ID=@SHOP_USER_ID

	select 	a.ID,
			a.CREATION_DATE,
			a.STATUS as STATUS_ID,
			a.LOAN_TYPE_ID,
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
	left join Common.COMPLETED_APPLICATION c with (NOLOCK)
		on a.ID=c.APPLICATION_ID
	where (a.SHOP_USER_ID=@SHOP_USER_ID or (a.SHOP_USER_ID is null and c.SHOP_CODE=@HEAD_CODE and convert(tinyint,c.GOODS_RECEIVING_CODE)-1 = convert(tinyint,@IS_DELIVERY_SHOP_USER)))
		and convert(date,a.CREATION_DATE) between @FROM_DATE and @TO_DATE
		and upper(rtrim(a.FIRST_NAME) + ' ' + rtrim(a.LAST_NAME) + ' ' + rtrim(a.PATRONYMIC_NAME)) like '%' + upper(rtrim(isnull(@NAME,''))) + '%'
		and a.STATUS=isnull(@STATUS,a.STATUS)
	order by a.CREATION_DATE desc
GO
