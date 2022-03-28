if exists (select * from sys.objects where name='ONBOARDING_CUSTOMER' and type='U')
	drop table Common.ONBOARDING_CUSTOMER
GO

CREATE TABLE Common.ONBOARDING_CUSTOMER(
	ID					uniqueidentifier	NOT NULL PRIMARY KEY,
	CREATION_DATE		datetime			NOT NULL default getdate(),
	first_name_eng		varchar(50)			NOT NULL,
	last_name_eng		varchar(50)			NOT NULL,
	mobile_number		varchar(20)			NOT NULL,
	email				varchar(50)			NOT NULL,
	birth_date			date				NOT NULL,
	first_name_arm		nvarchar(50)		NOT NULL,
	last_name_arm		nvarchar(50)		NOT NULL,
	middle_name_arm		nvarchar(50)		NOT NULL,
	document_type_id	tinyint				NOT NULL,
	document_number		varchar(50)			NOT NULL,
	soccard_number		varchar(50)			NOT NULL,
	document_issue_date	date				NULL,
	document_issuer		char(3)				NULL,
	is_student 			bit					NULL
)
GO
