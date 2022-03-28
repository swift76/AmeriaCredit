if exists (select * from sys.objects where name='sp_GetCommunicationTypes' and type='P')
	drop procedure Common.sp_GetCommunicationTypes
GO

create procedure Common.sp_GetCommunicationTypes(@LANGUAGE_CODE	char(2))
AS
	select CODE,
		case @LANGUAGE_CODE
			when 'AM' then NAME_AM
			else NAME_EN
		end as NAME
	from Common.COMMUNICATION_TYPE
	where CODE<>'1'
GO

