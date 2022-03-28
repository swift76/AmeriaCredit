﻿if exists (select * from sys.objects where name='NORQ_COBORROWER_QUERY_RESULT_WORK' and type='U')
	drop table Common.NORQ_COBORROWER_QUERY_RESULT_WORK
GO

CREATE TABLE Common.NORQ_COBORROWER_QUERY_RESULT_WORK(
	APPLICATION_ID		uniqueidentifier	NOT NULL,
	ORGANIZATION_NAME	nvarchar(200)		NOT NULL,
	TAX_ID_NUMBER		char(8)				NOT NULL,
	ADDRESS				nvarchar(200)		NOT NULL,
	FROM_DATE			date				NOT NULL,
	TO_DATE				date				NOT NULL,
	SALARY				money				NOT NULL,
	SOCIAL_PAYMENT		money				NOT NULL,
	POSITION			nvarchar(400)		NOT NULL
)
GO

CREATE CLUSTERED INDEX iNORQ_COBORROWER_QUERY_RESULT_WORK1 ON Common.NORQ_COBORROWER_QUERY_RESULT_WORK(APPLICATION_ID)
GO
