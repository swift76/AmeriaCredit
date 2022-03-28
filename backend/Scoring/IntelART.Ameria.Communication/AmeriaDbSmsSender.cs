using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using IntelART.Communication;

namespace IntelART.Ameria.Communication
{
    public class AmeriaDbSmsSender : DbConsumer, ISmsSender
    {
        public AmeriaDbSmsSender(string connectionString)
            : base(connectionString)
        {
        }

        public async Task SendAsync(string to, string text)
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
                    command.Parameters.Add(new SqlParameter("@EmailSMS", false));
                    command.Parameters.Add(new SqlParameter("@Address", to));
                    command.Parameters.Add(new SqlParameter("@Subject", "Notification"));
                    command.Parameters.Add(new SqlParameter("@Body", text));
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
