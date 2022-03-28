using System.Data;
using System.Data.SqlClient;
using IntelART.Utilities;
using IntelART.IdentityManagement;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace IntelART.Ameria.SqlMembershipProvider
{
    public class SqlMembershipProvider : IMembershipProvider
    {
        private readonly string schemaName;
        private readonly string getUserByUsernameSpName;
        private readonly string getUserByIdSpName;
        private readonly string authenticateApplicationUserSpName;
        private readonly string changeApplicationUserPasswordSpName;

        private string connectionString;

        public SqlMembershipProvider(string connectionString)
        {
            this.schemaName = "Common";
            this.getUserByUsernameSpName = string.Format("{0}.{1}", this.schemaName, "sp_GetApplicationUser");
            this.getUserByIdSpName = string.Format("{0}.{1}", this.schemaName, "sp_GetApplicationUserByID");
            this.authenticateApplicationUserSpName = string.Format("{0}.{1}", this.schemaName, "sp_AuthenticateApplicationUser");
            this.changeApplicationUserPasswordSpName = string.Format("{0}.{1}", this.schemaName, "sp_ChangeApplicationUserPasswordByID");
            this.connectionString = connectionString;
        }

        public UserInfo GetUserByUsername(string username)
        {
            UserInfo userInfo = null;
            using (SqlConnection connection = this.GetConnection())
            {
                using (SqlCommand command = new SqlCommand(this.getUserByUsernameSpName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("LOGIN", username.Trim()));
                    using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.SingleRow))
                    {
                        userInfo = this.ReadSingleUser(reader);
                    }
                }
            }
            return userInfo;
        }

        public bool ValidatePassword(string username, string password)
        {
            bool result = false;

            using (SqlConnection connection = this.GetConnection())
            {
                using (SqlCommand command = new SqlCommand(this.authenticateApplicationUserSpName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("LOGIN", username.Trim()));
                    command.Parameters.Add(new SqlParameter("HASH", Crypto.HashString(password)));
                    using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.SingleRow))
                    {
                        result = reader.Read();
                    }
                }
            }

            return result;
            ////return repository.AuthenticateApplicationUser(username, Crypto.HashString(password));
        }

        //public Task ActivateUser(string id)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task AddUser(ApplicationUser user)
        //{
        //    throw new NotImplementedException();
        //}

        public async Task ChangeUserPassword(string id, string oldPassword, string newPassword)
        {
            using (SqlConnection connection = this.GetConnection())
            {
                using (SqlCommand command = new SqlCommand(this.changeApplicationUserPasswordSpName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("ID", id));
                    command.Parameters.Add(new SqlParameter("HASH", Crypto.HashString(newPassword)));
                    command.Parameters.Add(new SqlParameter("PASSWORD_EXPIRY_DATE", DateTime.Now.Date.AddDays(90)));
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        //public Task DeactivateUser(string id)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task DeleteUser(string id)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<IEnumerable<ApplicationUser>> GetAllUsers()
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<IEnumerable<ApplicationUser>> GetAllUsers(int from, int count)
        //{
        //    throw new NotImplementedException();
        //}

        public UserInfo GetUserById(string userId)
        {
            UserInfo userInfo = null;

            int id;
            if (int.TryParse(userId, out id))
            {
                using (SqlConnection connection = this.GetConnection())
                {
                    using (SqlCommand command = new SqlCommand(this.getUserByIdSpName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("ID", id));
                        using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.SingleRow))
                        {
                            userInfo = this.ReadSingleUser(reader);
                        }
                    }
                }
            }
            return userInfo;
        }

        //public Task<IEnumerable<ApplicationUser>> SearchUsers(string criteria)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task UpdateUserInfo(ApplicationUser user)
        //{
        //    throw new NotImplementedException();
        //}

        private UserInfo ReadSingleUser(SqlDataReader reader)
        {
            UserInfo result = null;
            if (reader.Read())
            {
                result = this.ReadUser(reader);
            }
            return result;
        }

        private UserInfo ReadUser(SqlDataReader reader)
        {
            UserInfo result = null;
            result = new UserInfo();
            result.Id = reader.GetInt32(reader.GetOrdinal("ID"));
            result.Username = reader.GetString(reader.GetOrdinal("LOGIN"));
            result.Email = reader.GetString(reader.GetOrdinal("EMAIL"));
            result.FullName = reader.GetString(reader.GetOrdinal("FIRST_NAME")) + reader.GetString(reader.GetOrdinal("LAST_NAME"));
            return result;
        }

        private SqlConnection GetConnection()
        {
            SqlConnection connection = new SqlConnection(this.connectionString);
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            return connection;
        }

        private async Task<SqlConnection> GetConnectionAsync()
        {
            SqlConnection connection = new SqlConnection(this.connectionString);
            if (connection.State != ConnectionState.Open)
            {
                await connection.OpenAsync();
            }

            return connection;
        }

        public async Task<IEnumerable<string>> GetUserRolesById(string userId)
        {
            List<string> roles = new List<string>();
            int id;
            if (int.TryParse(userId, out id))
            {
                using (SqlConnection connection = await this.GetConnectionAsync())
                {
                    using (SqlCommand command = new SqlCommand("select common.f_GetApplicationUserRoleName(@id)", connection))
                    {
                        command.CommandType = CommandType.Text;
                        command.Parameters.Add(new SqlParameter("@id", id));
                        using (SqlDataReader reader = await command.ExecuteReaderAsync(CommandBehavior.SingleRow))
                        {
                            if (reader.Read())
                            {
                                string roleName = reader.GetString(0);
                                if (roleName != null)
                                {
                                    roles.Add(roleName);
                                    if (roleName == "BankPowerUser")
                                    {
                                        roles.Add("BankUser");
                                    }
                                    else if (roleName == "ShopPowerUser")
                                    {
                                        roles.Add("ShopUser");
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return roles;
        }
    }
}
