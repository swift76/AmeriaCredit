if exists (select * from sys.objects where name='GENERAL_LOAN_SETTING' and type='U')
	drop table GL.GENERAL_LOAN_SETTING
GO

CREATE TABLE GL.GENERAL_LOAN_SETTING (
	REPEAT_COUNT 		int		NOT NULL,
	REPEAT_DAY_COUNT 	tinyint	NOT NULL,
	EXPIRE_DAY_COUNT	tinyint	NOT NULL
)
GO
