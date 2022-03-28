using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace IntelART.Ameria.Communication
{
    public abstract class DbConsumer
    {
        private string connectionString;

        public DbConsumer(string connectionString)
        {
            this.connectionString = connectionString;
        }

        protected async Task<SqlConnection> GetConnectionAsync()
        {
            SqlConnection connection = new SqlConnection(this.connectionString);
            if (connection.State != ConnectionState.Open)
            {
                await connection.OpenAsync();
            }

            return connection;
        }
    }
}
