CREATE TYPE Common.NORQQueryResultWork AS TABLE
(
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
