if exists (select * from sys.objects where name='am0sp_SendLoanApplicationEmailSMSNotification' and type='P')
	drop procedure dbo.am0sp_SendLoanApplicationEmailSMSNotification
GO

create procedure am0sp_SendLoanApplicationEmailSMSNotification(@EmailSMS	bit,
															   @Address		varchar(100),
															   @Subject		nvarchar(100),
															   @Body		nvarchar(max))
AS
GO
