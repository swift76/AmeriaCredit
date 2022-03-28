if exists (select * from sys.objects where name='sp_GetSettings' and type='P')
	drop procedure Common.sp_GetSettings
GO

create procedure Common.sp_GetSettings(@CODE varchar(30) = null)
AS
	select CODE, VALUE, DESCRIPTION
	from Common.SETTING
	where CODE = isnull(@CODE,CODE)
GO
