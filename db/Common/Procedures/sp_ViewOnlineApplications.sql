CREATE OR ALTER PROCEDURE Common.sp_ViewOnlineApplications(@FROM_DATE			date,
											  @TO_DATE				date,
											  @SOCIAL_CARD_NUMBER	char(10),
											  @DOCUMENT_NUMBER		char(9),
											  @MOBILE_PHONE			varchar(20))
AS
	select
		convert(char(19),a.CREATION_DATE,121) as Date,
		convert(char(19),norq.QUERY_DATE,121) as NORQDate,
		convert(char(19),acra.QUERY_DATE,121) as ACRADate,
		a.ID,
		a.SOURCE_ID as SourceID,
		a.LOAN_TYPE_ID as LoanTypeID,
		Common.ahf_Unicode2ANSI(t.NAME_AM) as LoanTypeName,
		a.STATUS as StatusID,
		Common.ahf_Unicode2ANSI(s.NAME_AM) as StatusName,
		Common.ahf_Unicode2ANSI(a.REFUSAL_REASON) as RefusalReason,
		Common.ahf_Unicode2ANSI(a.MANUAL_REASON) as ManualReason,
		isnull(c.FINAL_AMOUNT,a.INITIAL_AMOUNT) as Amount,
		a.CURRENCY_CODE as Currency,
		Common.ahf_Unicode2ANSI(a.FIRST_NAME) as FirstName,
		Common.ahf_Unicode2ANSI(a.LAST_NAME) as LastName,
		Common.ahf_Unicode2ANSI(a.PATRONYMIC_NAME) as PatronymicName,
		a.BIRTH_DATE as BirthDate,
		a.SOCIAL_CARD_NUMBER as SocialCard,
		a.DOCUMENT_TYPE_CODE as DocumentTypeID,
		Common.ahf_Unicode2ANSI(d.NAME_AM) as DocumentTypeName,
		a.DOCUMENT_NUMBER as DocumentNumber,
		coalesce(a.MOBILE_PHONE_1,c.MOBILE_PHONE_1,u.MOBILE_PHONE,'') as MobilePhone,
		coalesce(c.EMAIL,u.EMAIL,'') as Email,
		a.NORQ_TRY_COUNT as NorqTryCount,
		a.ACRA_TRY_COUNT as AcraTryCount,
		a.CLIENT_CODE as ClientCode,
		a.ISN
	from Common.APPLICATION a with (nolock)
	join Common.APPLICATION_STATUS s with (nolock)
		on a.STATUS=s.CODE
	join Common.LOAN_TYPE t with (nolock)
		on a.LOAN_TYPE_ID=t.CODE
	join Common.DOCUMENT_TYPE d with (nolock)
		on a.DOCUMENT_TYPE_CODE=d.CODE
	left join Common.COMPLETED_APPLICATION c with (nolock)
		on a.ID=c.APPLICATION_ID
	left join Common.CUSTOMER_USER u with (nolock)
		on a.CUSTOMER_USER_ID=u.APPLICATION_USER_ID
	left join Common.NORQ_QUERY_RESULT norq with (nolock)
		on a.ID=norq.APPLICATION_ID
	left join Common.ACRA_QUERY_RESULT acra with (nolock)
		on a.ID=acra.APPLICATION_ID
	where convert(date,a.CREATION_DATE) between @FROM_DATE and @TO_DATE
		and a.SOCIAL_CARD_NUMBER=
			case rtrim(isnull(@SOCIAL_CARD_NUMBER,''))
				when '' then a.SOCIAL_CARD_NUMBER
				else @SOCIAL_CARD_NUMBER
			end
		and a.DOCUMENT_NUMBER=
			case rtrim(isnull(@DOCUMENT_NUMBER,''))
				when '' then a.DOCUMENT_NUMBER
				else @DOCUMENT_NUMBER
			end
		and coalesce(a.MOBILE_PHONE_1,c.MOBILE_PHONE_1,u.MOBILE_PHONE,'')=
			case rtrim(isnull(@MOBILE_PHONE,''))
				when '' then coalesce(a.MOBILE_PHONE_1,c.MOBILE_PHONE_1,u.MOBILE_PHONE,'')
				else @MOBILE_PHONE
			end
	order by a.CREATION_DATE
GO
