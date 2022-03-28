create or alter procedure Common.sp_GetRestrictedPasswords
AS
	select PASSWORD from Common.RESTRICTED_PASSWORD
GO
