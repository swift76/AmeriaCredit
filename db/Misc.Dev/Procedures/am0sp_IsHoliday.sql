if exists (select * from sys.objects where name='am0sp_IsHoliday' and type='P')
	drop procedure am0sp_IsHoliday
GO

CREATE PROCEDURE am0sp_IsHoliday(
	@Date datetime
) AS
select 0;
GO

