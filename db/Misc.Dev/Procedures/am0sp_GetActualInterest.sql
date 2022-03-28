if exists (select * from sys.objects where name='am0sp_GetActualInterest' and type='P')
	drop procedure am0sp_GetActualInterest
GO

CREATE PROCEDURE am0sp_GetActualInterest(
	@TERM_MONTH int,
	@AGREEMENT_FROM datetime,
	@FIRST_REPAYMENT datetime,
	@AGREEMENT_TO datetime,
	@TEMPLATE_CODE varchar(10),
	@SUBSYSTEM_CODE varchar(10),
	@AMOUNT decimal,
	@CURRENCY varchar(10),
	@CARD_TYPE char(3),
	@INTEREST decimal,
	@REPAY_DAY int,
	@LOAN_TYPE varchar(2)
) AS
select 1;
GO

