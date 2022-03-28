if exists (select * from sys.objects where name='sp_GetStates' and type='P')
	drop procedure Common.sp_GetStates
GO

create procedure Common.sp_GetStates(@LANGUAGE_CODE	char(2))
AS
	select CODE,
		case @LANGUAGE_CODE
			when 'AM' then NAME_AM
			else NAME_EN
		end as NAME
	from Common.STATE
GO
