using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using IntelART.Communication;

namespace IntelART.Ameria.Communication
{
    public class AmeriaDbEmailSender : DbConsumer, IEmailSender
    {
        public AmeriaDbEmailSender(string connectionString)
            : base(connectionString)
        {
        }

        public async Task SendAsync(EmailAddress from, EmailAddress to, string subject, string body)
        {
            await this.SendAsync(to, subject, body);
        }

        public async Task SendAsync(EmailAddress to, string subject, string body)
        {
            using (SqlConnection connection = await this.GetConnectionAsync())
            {
                string sendDB = string.Empty;
                using (SqlCommand command = new SqlCommand("Common.sp_GetSettings", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@CODE", "SEND_SERVER_DATABASE"));
                    using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.SingleRow))
                    {
                        if (reader.Read())
                        {
                            sendDB = reader.GetString(reader.GetOrdinal("VALUE"));
                        }
                    }
                }

                using (SqlCommand command = new SqlCommand(string.Format("{0}dbo.am0sp_SendLoanApplicationEmailSMSNotification", sendDB), connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@EmailSMS", true));
                    command.Parameters.Add(new SqlParameter("@Address", to.Address));
                    command.Parameters.Add(new SqlParameter("@Subject", subject));
                    command.Parameters.Add(new SqlParameter("@Body", body));
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
