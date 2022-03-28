using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using IntelART.Ameria.Entities;

namespace IntelART.Ameria.Repositories
{
    public class BaseRepository
    {
        public string ConnectionString;

        public BaseRepository(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        public IEnumerable<T> GetList<T>(DynamicParameters parameters, string procedureName, int timeoutInterval = 180, CommandType cmdType = CommandType.StoredProcedure)
        {
            using (IDbConnection connection = new SqlConnection(ConnectionString))
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                return connection.Query<T>(procedureName, parameters, commandType: cmdType, commandTimeout: timeoutInterval);
            }
        }

        public T GetScalarValue<T>(DynamicParameters parameters, string procedureName, int timeoutInterval = 180, CommandType cmdType = CommandType.Text)
        {
            using (IDbConnection connection = new SqlConnection(ConnectionString))
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                return connection.ExecuteScalar<T>(procedureName, parameters, commandType: cmdType, commandTimeout: timeoutInterval);
            }
        }

        public async Task<IEnumerable<T>> GetListAsync<T>(DynamicParameters parameters, string procedureName, int timeoutInterval = 180, CommandType cmdType = CommandType.StoredProcedure)
        {
            using (IDbConnection connection = new SqlConnection(ConnectionString))
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                return await connection.QueryAsync<T>(procedureName, parameters, commandType: cmdType, commandTimeout: timeoutInterval);
            }
        }

        public T GetSingle<T>(DynamicParameters parameters, string procedureName, int timeoutInterval = 180, CommandType cmdType = CommandType.StoredProcedure)
        {
            return GetList<T>(parameters, procedureName, timeoutInterval, cmdType).FirstOrDefault();
        }

        public async Task<T> GetSingleAsync<T>(DynamicParameters parameters, string procedureName, int timeoutInterval = 180, CommandType cmdType = CommandType.StoredProcedure)
        {
            using (IDbConnection connection = new SqlConnection(ConnectionString))
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                return await connection.QueryFirstOrDefaultAsync<T>(procedureName, parameters, commandType: cmdType, commandTimeout: timeoutInterval);
            }
        }

        public void Execute(DynamicParameters parameters, string procedureName, int timeoutInterval = 180, CommandType cmdType = CommandType.StoredProcedure)
        {
            using (IDbConnection connection = new SqlConnection(ConnectionString))
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                connection.Execute(procedureName, parameters, commandType: cmdType, commandTimeout: timeoutInterval);
            }
        }

        public async Task ExecuteAsync(DynamicParameters parameters, string procedureName, int timeoutInterval = 180, CommandType cmdType = CommandType.StoredProcedure)
        {
            using (IDbConnection connection = new SqlConnection(ConnectionString))
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                await connection.ExecuteAsync(procedureName, parameters, commandType: cmdType, commandTimeout: timeoutInterval);
            }
        }

        public DateTime GetServerDate()
        {
            return GetSingle<DateTime>(new DynamicParameters(), "SELECT GETDATE()", cmdType: CommandType.Text);
        }

        public IEnumerable<Setting> GetSettings()
        {
            return GetList<Setting>(new DynamicParameters(), "Common.sp_GetSettings");
        }

        public string GetSetting(string code)
        {
            string result = null;
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("CODE", code);
            Setting setting = GetSingle<Setting>(parameters, "Common.sp_GetSettings");
            if (setting != null)
                result = setting.VALUE;
            return result;
        }
        public static string GenerateOperationDetailsString(Dictionary<string, string> changes)
        {
            StringBuilder builder = new StringBuilder();
            foreach (string key in changes.Keys)
                builder.AppendLine(string.Format("{0}:{1}", key, changes[key]));
            return builder.ToString();
        }

        public DateTime GenerateUserPasswordExpiryDate()
        {
            DateTime current = GetServerDate();
            int expiry_days = int.Parse(GetSetting("USER_PASSWORD_EXPIRY"));
            return current.AddDays(expiry_days);
        }

        public static string FormatDate(DateTime date)
        {
            return date.ToString("dd/MM/yyyy");
        }

        public static void PrepareParameters<T>(Dictionary<string, string> changes,
            DynamicParameters parameters, string entityName, T entityValue, bool check = false)
        {
            string entityValueString = null;
            if (entityValue != null)
            {
                if (typeof(T) == typeof(DateTime)) // special approach for DateTime type
                {
                    DateTime entityValueDate = (DateTime)(object)entityValue;
                    if (entityValueDate != default(DateTime))
                    {
                        entityValueString = FormatDate(entityValueDate);
                    }
                }
                else
                {
                    entityValueString = entityValue.ToString().Trim();
                }
            }

            if (!string.IsNullOrEmpty(entityValueString))
            {
                parameters.Add(entityName, entityValue);
                changes.Add(entityName, entityValueString);
            }
            else if (check) // the field is mandatory and cannot be null or empty
            {
                throw new ApplicationException("ERR-0017", string.Format("{0} must have a value", entityName));
            }
        }

        /// <summary>
        /// Maps the values of "APPLICATION_STATUS" table
        /// with the strings of Application state diagram.
        /// </summary>
        protected string MapApplicationStatus(int statusID)
        {
            switch (statusID)
            {
                case 0:
                    return "NEW";
                case 1:
                case 2:
                case 3:
                    return "PENDING_PRE_APPROVAL";
                case 5:
                case 8:
                    return "PRE_APPROVAL_SUCCESS";
                case 6:
                case 9:
                    return "PRE_APPROVAL_FAIL";
                case 7:
                    return "PRE_APPROVAL_REVIEW";
                case 10:
                case 11:
                    return "PENDING_APPROVAL";
                case 12:
                    return "APPROVAL_REVIEW";
                case 13:
                    return "APPROVAL_SUCCESS";
                case 14:
                    return "APPROVAL_FAIL";
                case 15:
                    return "AGREED";
                case 16:
                case 17:
                    return "CANCELLED";
                case 19:
                    return "PHONE_VERIFICATION_PENDING";
                case 20:
                    return "DELIVERING";
                case 21:
                case 22:
                    return "COMPLETED";
                case 55:
                    return "EXPIRED";
                default:
                    throw new ApplicationException("ERR-0025", "Incorrect application status ID");
            }
        }

        /// <summary>
        /// Maps the values of "LOAN_TYPE" table
        /// with the defined strings.
        /// </summary>
        protected string MapLoanTypeStatus(string stringID)
        {
            int id = int.Parse(stringID);
            switch (id)
            {
                case 0:
                    return "INSTALLATION_LOAN";
                ////case 1:
                ////    return "GENERAL_LOAN";
                ////default:
                ////    throw new ApplicationException("ERR-0026", "Incorrect loan type ID");
                default:
                    return string.Format("GENERAL_LOAN_{0}", stringID);

            }
        }

        public async void ExecuteAsyncNoWait(DynamicParameters parameters, string procedureName, int timeoutInterval = 180, CommandType cmdType = CommandType.StoredProcedure)
        {
            using (IDbConnection connection = new SqlConnection(ConnectionString))
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                await connection.ExecuteAsync(procedureName, parameters, commandType: cmdType, commandTimeout: timeoutInterval);
            }
        }

        public string GetAuthorizationCode()
        {
            return (Guid.NewGuid()).ToString().Replace("-", string.Empty).ToUpper().Substring(0, 6);
        }
    }
}
