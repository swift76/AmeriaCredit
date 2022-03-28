if exists (select * from sys.objects where name='am0sp_GetPassHolidayDate' and type='P')
	drop procedure am0sp_GetPassHolidayDate
GO

CREATE PROCEDURE am0sp_GetPassHolidayDate(
	@SubsystemCode varchar(10),
	@TemplateCode varchar(10),
	@Date datetime
) AS
select GETDATE();
GO
