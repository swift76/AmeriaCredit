if exists (select * from sys.objects where name='LOAN_TYPE' and type='U')
	drop table Common.LOAN_TYPE
GO

CREATE TABLE Common.LOAN_TYPE (
	CODE 					char(2)			NOT NULL,
	NAME_AM					nvarchar(50)	NOT NULL,
	NAME_EN					varchar(50)		NOT NULL,
	FROM_DATE				date			NULL,
	TO_DATE					date			NULL,
	REPAYMENT_DAY_FROM		tinyint			NOT NULL,
	REPAYMENT_DAY_TO		tinyint			NOT NULL,
	IS_OVERDRAFT			bit				NOT NULL,
	IS_REPAY_DAY_FIXED		bit				NOT NULL,
	IS_CARD_ACCOUNT			bit				NOT NULL,
	IS_REPAY_START_DAY		bit				NOT NULL,
	IS_REPAY_NEXT_MONTH		bit				NOT NULL,
	REPAY_TRANSITION_DAY	tinyint			NOT NULL,
	PLEDGE_TYPE				char(1)			NOT NULL,
	IS_STUDENT				bit				NOT NULL
)
GO

CREATE UNIQUE CLUSTERED INDEX iLOAN_TYPE1 ON Common.LOAN_TYPE(CODE)
GO
