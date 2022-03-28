if exists (select * from sys.objects where name='sp_ProcessScoringQueriesByISN' and type='P')
	drop procedure Common.sp_ProcessScoringQueriesByISN
GO



if exists (select * from sys.objects where name='sp_ProcessScoringQueriesByID' and type='P')
	drop procedure Common.sp_ProcessScoringQueriesByID
GO



if exists (select * from sys.objects where name='sp_ProcessACRAQueryByID' and type='P')
	drop procedure Common.sp_ProcessACRAQueryByID
GO



if exists (select * from sys.objects where name='sp_ProcessNORQQueryByID' and type='P')
	drop procedure Common.sp_ProcessNORQQueryByID
GO



if exists (select * from sys.objects where name='sp_ProcessScoringQueries' and type='P')
	drop procedure Common.sp_ProcessScoringQueries
GO



if exists (select * from sys.objects where name='f_GetCardStatus' and type='FN')
	drop function Common.f_GetCardStatus
GO

CREATE FUNCTION Common.f_GetCardStatus(@cardNumber nchar(16),@expiryDate datetime)
RETURNS int
AS
begin
	declare @result int = 2
	return @result
end
GO



CREATE PROCEDURE [Common].[sp_ProcessScoringQueries]
@queryTimeout INT
AS
GO



CREATE PROCEDURE [Common].[sp_ProcessNORQQueryByID](
	@queryTimeout	INT,
	@ID				uniqueidentifier
)
AS
GO



CREATE PROCEDURE [Common].[sp_ProcessACRAQueryByID](
	@queryTimeout	INT,
	@ID				uniqueidentifier
)
AS
GO



CREATE PROCEDURE [Common].[sp_ProcessScoringQueriesByID](
	@queryTimeout	INT,
	@ID				uniqueidentifier
)
AS
GO



CREATE PROCEDURE [Common].[sp_ProcessScoringQueriesByISN](
	@queryTimeout	INT,
	@ID				INT
)
AS
GO
