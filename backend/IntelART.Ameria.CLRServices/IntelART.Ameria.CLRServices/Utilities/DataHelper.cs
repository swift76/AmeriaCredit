using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace IntelART.Ameria.CLRServices
{
    public class DataHelper : IDisposable
    {
        public ServiceConfig GetServiceConfig(string serviceCode)
        {
            ServiceConfig result = new ServiceConfig();
            using (SqlCommand cmd = new SqlCommand(string.Format("Common.sp_Get{0}ConfigData", serviceCode), ActiveConnection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = CommandTimeoutInterval;
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    if (rdr.Read())
                    {
                        result.URL = rdr.GetString(0);
                        result.UserName = rdr.GetString(1);
                        result.UserPassword = rdr.GetString(2);
                    }
                }
            }
            return result;
        }

        public DateTime GetServerDate()
        {
            DateTime result = DateTime.Now;
            using (SqlCommand cmd = new SqlCommand("select convert(date,getdate())", ActiveConnection))
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = CommandTimeoutInterval;
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    if (rdr.Read())
                        result = rdr.GetDateTime(0);
                }
            }
            return result;
        }

        public void AutomaticallyRefuseApplication(Guid id, string reason)
        {
            using (SqlCommand cmd = new SqlCommand("Common.sp_AutomaticallyRefuseApplication", ActiveConnection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = CommandTimeoutInterval;
                cmd.Parameters.Add(new SqlParameter("@ID", SqlDbType.UniqueIdentifier)).Value = id;
                cmd.Parameters.Add(new SqlParameter("@REFUSAL_REASON", SqlDbType.NVarChar, 100)).Value = reason;
                cmd.ExecuteNonQuery();
            }
        }

        public void LogError(string operation, string errorMessage, Guid? id = null)
        {
            using (SqlCommand cmd = new SqlCommand("Common.sp_LogScoringError", ActiveConnection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = CommandTimeoutInterval;
                if (id.HasValue)
                    cmd.Parameters.Add(new SqlParameter("@APPLICATION_ID", SqlDbType.UniqueIdentifier)).Value = id.Value;
                cmd.Parameters.Add(new SqlParameter("@OPERATION", SqlDbType.VarChar, 200)).Value = operation;
                cmd.Parameters.Add(new SqlParameter("@ERROR_MESSAGE", SqlDbType.NVarChar, -1)).Value = errorMessage;
                cmd.ExecuteNonQuery();
            }
        }

        public void SaveNORQQueryResult(Guid id, NORQData data)
        {
            using (SqlCommand cmd = new SqlCommand("Common.sp_SaveNORQQueryResult", ActiveConnection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = CommandTimeoutInterval;
                cmd.Parameters.Add(new SqlParameter("@APPLICATION_ID", SqlDbType.UniqueIdentifier)).Value = id;

                cmd.Parameters.Add(new SqlParameter("@FIRST_NAME", SqlDbType.NVarChar, 20)).Value = data.Main.FirstName;
                cmd.Parameters.Add(new SqlParameter("@LAST_NAME", SqlDbType.NVarChar, 20)).Value = data.Main.LastName;
                cmd.Parameters.Add(new SqlParameter("@PATRONYMIC_NAME", SqlDbType.NVarChar, 20)).Value = data.Main.MiddleName;
                cmd.Parameters.Add(new SqlParameter("@BIRTH_DATE", SqlDbType.Date)).Value = data.Main.BirthDate;
                cmd.Parameters.Add(new SqlParameter("@GENDER", SqlDbType.Bit)).Value = data.Main.Gender;
                cmd.Parameters.Add(new SqlParameter("@DISTRICT", SqlDbType.NVarChar, 20)).Value = data.Main.District;
                cmd.Parameters.Add(new SqlParameter("@COMMUNITY", SqlDbType.NVarChar, 40)).Value = data.Main.Community;
                cmd.Parameters.Add(new SqlParameter("@STREET", SqlDbType.NVarChar, 100)).Value = data.Main.Street;
                cmd.Parameters.Add(new SqlParameter("@BUILDING", SqlDbType.NVarChar, 40)).Value = data.Main.Building;
                cmd.Parameters.Add(new SqlParameter("@APARTMENT", SqlDbType.NVarChar, 40)).Value = data.Main.Apartment;
                if (data.Main.NonBiometricPassport.IsValid())
                {
                    cmd.Parameters.Add(new SqlParameter("@PASSPORT_NUMBER", SqlDbType.Char, 9)).Value = data.Main.NonBiometricPassport.Number;
                    cmd.Parameters.Add(new SqlParameter("@PASSPORT_DATE", SqlDbType.Date)).Value = data.Main.NonBiometricPassport.IssueDate;
                    cmd.Parameters.Add(new SqlParameter("@PASSPORT_EXPIRY_DATE", SqlDbType.Date)).Value = data.Main.NonBiometricPassport.ExpiryDate;
                    cmd.Parameters.Add(new SqlParameter("@PASSPORT_BY", SqlDbType.Char, 3)).Value = data.Main.NonBiometricPassport.IssuedBy;
                }
                if (data.Main.BiometricPassport.IsValid())
                {
                    cmd.Parameters.Add(new SqlParameter("@BIOMETRIC_PASSPORT_NUMBER", SqlDbType.Char, 9)).Value = data.Main.BiometricPassport.Number;
                    cmd.Parameters.Add(new SqlParameter("@BIOMETRIC_PASSPORT_ISSUE_DATE", SqlDbType.Date)).Value = data.Main.BiometricPassport.IssueDate;
                    cmd.Parameters.Add(new SqlParameter("@BIOMETRIC_PASSPORT_EXPIRY_DATE", SqlDbType.Date)).Value = data.Main.BiometricPassport.ExpiryDate;
                    cmd.Parameters.Add(new SqlParameter("@BIOMETRIC_PASSPORT_ISSUED_BY", SqlDbType.Char, 3)).Value = data.Main.BiometricPassport.IssuedBy;
                }
                if (data.Main.IDCard.IsValid())
                {
                    cmd.Parameters.Add(new SqlParameter("@ID_CARD_NUMBER", SqlDbType.Char, 9)).Value = data.Main.IDCard.Number;
                    cmd.Parameters.Add(new SqlParameter("@ID_CARD_ISSUE_DATE", SqlDbType.Date)).Value = data.Main.IDCard.IssueDate;
                    cmd.Parameters.Add(new SqlParameter("@ID_CARD_EXPIRY_DATE", SqlDbType.Date)).Value = data.Main.IDCard.ExpiryDate;
                    cmd.Parameters.Add(new SqlParameter("@ID_CARD_ISSUED_BY", SqlDbType.Char, 3)).Value = data.Main.IDCard.IssuedBy;
                }
                cmd.Parameters.Add(new SqlParameter("@SOCIAL_CARD_NUMBER", SqlDbType.Char, 10)).Value = data.Main.SocialCardNumber;
                cmd.Parameters.Add(new SqlParameter("@FEE", SqlDbType.Money)).Value = data.Main.Salary;
                cmd.Parameters.Add(new SqlParameter("@SOCIAL_PAYMENT", SqlDbType.Money)).Value = data.Main.SocialPayment;
                cmd.Parameters.Add(new SqlParameter("@RESPONSE_XML", SqlDbType.NVarChar, -1)).Value = ServiceHelper.GetFormattedXML(data.Main.XML);

                DataTable tableWork = new DataTable("Common.NORQQueryResultWork");
                tableWork.Columns.Add("ORGANIZATION_NAME", typeof(string));
                tableWork.Columns.Add("TAX_ID_NUMBER", typeof(string));
                tableWork.Columns.Add("ADDRESS", typeof(string));
                tableWork.Columns.Add("FROM_DATE", typeof(DateTime));
                tableWork.Columns.Add("TO_DATE", typeof(DateTime));
                tableWork.Columns.Add("SALARY", typeof(decimal));
                tableWork.Columns.Add("SOCIAL_PAYMENT", typeof(decimal));
                tableWork.Columns.Add("POSITION", typeof(string));
                for (int i = 0; i < data.Main.Employment.Count; i++)
                    tableWork.Rows.Add(data.Main.Employment[i].OrganizationName
                        , data.Main.Employment[i].TaxCode
                        , data.Main.Employment[i].OrganizationAddress
                        , data.Main.Employment[i].AgreementStartDate
                        , data.Main.Employment[i].AgreementEndDate
                        , data.Main.Employment[i].Salary
                        , data.Main.Employment[i].SocialPayment
                        , data.Main.Employment[i].Position);
                cmd.Parameters.AddWithValue("@WORK_DATA", tableWork).SqlDbType = SqlDbType.Structured;

                if (data.Coborrower != null)
                {
                    cmd.Parameters.Add(new SqlParameter("@COBORROWER_FIRST_NAME", SqlDbType.NVarChar, 20)).Value = data.Coborrower.FirstName;
                    cmd.Parameters.Add(new SqlParameter("@COBORROWER_LAST_NAME", SqlDbType.NVarChar, 20)).Value = data.Coborrower.LastName;
                    cmd.Parameters.Add(new SqlParameter("@COBORROWER_PATRONYMIC_NAME", SqlDbType.NVarChar, 20)).Value = data.Coborrower.MiddleName;
                    cmd.Parameters.Add(new SqlParameter("@COBORROWER_BIRTH_DATE", SqlDbType.Date)).Value = data.Coborrower.BirthDate;
                    cmd.Parameters.Add(new SqlParameter("@COBORROWER_GENDER", SqlDbType.Bit)).Value = data.Coborrower.Gender;
                    cmd.Parameters.Add(new SqlParameter("@COBORROWER_DISTRICT", SqlDbType.NVarChar, 20)).Value = data.Coborrower.District;
                    cmd.Parameters.Add(new SqlParameter("@COBORROWER_COMMUNITY", SqlDbType.NVarChar, 40)).Value = data.Coborrower.Community;
                    cmd.Parameters.Add(new SqlParameter("@COBORROWER_STREET", SqlDbType.NVarChar, 100)).Value = data.Coborrower.Street;
                    cmd.Parameters.Add(new SqlParameter("@COBORROWER_BUILDING", SqlDbType.NVarChar, 40)).Value = data.Coborrower.Building;
                    cmd.Parameters.Add(new SqlParameter("@COBORROWER_APARTMENT", SqlDbType.NVarChar, 40)).Value = data.Coborrower.Apartment;
                    if (data.Coborrower.NonBiometricPassport.IsValid())
                    {
                        cmd.Parameters.Add(new SqlParameter("@COBORROWER_PASSPORT_NUMBER", SqlDbType.Char, 9)).Value = data.Coborrower.NonBiometricPassport.Number;
                        cmd.Parameters.Add(new SqlParameter("@COBORROWER_PASSPORT_DATE", SqlDbType.Date)).Value = data.Coborrower.NonBiometricPassport.IssueDate;
                        cmd.Parameters.Add(new SqlParameter("@COBORROWER_PASSPORT_EXPIRY_DATE", SqlDbType.Date)).Value = data.Coborrower.NonBiometricPassport.ExpiryDate;
                        cmd.Parameters.Add(new SqlParameter("@COBORROWER_PASSPORT_BY", SqlDbType.Char, 3)).Value = data.Coborrower.NonBiometricPassport.IssuedBy;
                    }
                    if (data.Coborrower.BiometricPassport.IsValid())
                    {
                        cmd.Parameters.Add(new SqlParameter("@COBORROWER_BIOMETRIC_PASSPORT_NUMBER", SqlDbType.Char, 9)).Value = data.Coborrower.BiometricPassport.Number;
                        cmd.Parameters.Add(new SqlParameter("@COBORROWER_BIOMETRIC_PASSPORT_ISSUE_DATE", SqlDbType.Date)).Value = data.Coborrower.BiometricPassport.IssueDate;
                        cmd.Parameters.Add(new SqlParameter("@COBORROWER_BIOMETRIC_PASSPORT_EXPIRY_DATE", SqlDbType.Date)).Value = data.Coborrower.BiometricPassport.ExpiryDate;
                        cmd.Parameters.Add(new SqlParameter("@COBORROWER_BIOMETRIC_PASSPORT_ISSUED_BY", SqlDbType.Char, 3)).Value = data.Coborrower.BiometricPassport.IssuedBy;
                    }
                    if (data.Coborrower.IDCard.IsValid())
                    {
                        cmd.Parameters.Add(new SqlParameter("@COBORROWER_ID_CARD_NUMBER", SqlDbType.Char, 9)).Value = data.Coborrower.IDCard.Number;
                        cmd.Parameters.Add(new SqlParameter("@COBORROWER_ID_CARD_ISSUE_DATE", SqlDbType.Date)).Value = data.Coborrower.IDCard.IssueDate;
                        cmd.Parameters.Add(new SqlParameter("@COBORROWER_ID_CARD_EXPIRY_DATE", SqlDbType.Date)).Value = data.Coborrower.IDCard.ExpiryDate;
                        cmd.Parameters.Add(new SqlParameter("@COBORROWER_ID_CARD_ISSUED_BY", SqlDbType.Char, 3)).Value = data.Coborrower.IDCard.IssuedBy;
                    }
                    cmd.Parameters.Add(new SqlParameter("@COBORROWER_SOCIAL_CARD_NUMBER", SqlDbType.Char, 10)).Value = data.Coborrower.SocialCardNumber;
                    cmd.Parameters.Add(new SqlParameter("@COBORROWER_FEE", SqlDbType.Money)).Value = data.Coborrower.Salary;
                    cmd.Parameters.Add(new SqlParameter("@COBORROWER_SOCIAL_PAYMENT", SqlDbType.Money)).Value = data.Coborrower.SocialPayment;
                    cmd.Parameters.Add(new SqlParameter("@COBORROWER_RESPONSE_XML", SqlDbType.NVarChar, -1)).Value = ServiceHelper.GetFormattedXML(data.Coborrower.XML);

                    DataTable tableCoborrowerWork = new DataTable("Common.NORQQueryResultWork");
                    tableCoborrowerWork.Columns.Add("ORGANIZATION_NAME", typeof(string));
                    tableCoborrowerWork.Columns.Add("TAX_ID_NUMBER", typeof(string));
                    tableCoborrowerWork.Columns.Add("ADDRESS", typeof(string));
                    tableCoborrowerWork.Columns.Add("FROM_DATE", typeof(DateTime));
                    tableCoborrowerWork.Columns.Add("TO_DATE", typeof(DateTime));
                    tableCoborrowerWork.Columns.Add("SALARY", typeof(decimal));
                    tableCoborrowerWork.Columns.Add("SOCIAL_PAYMENT", typeof(decimal));
                    tableCoborrowerWork.Columns.Add("POSITION", typeof(string));
                    for (int i = 0; i < data.Coborrower.Employment.Count; i++)
                        tableCoborrowerWork.Rows.Add(data.Coborrower.Employment[i].OrganizationName
                            , data.Coborrower.Employment[i].TaxCode
                            , data.Coborrower.Employment[i].OrganizationAddress
                            , data.Coborrower.Employment[i].AgreementStartDate
                            , data.Coborrower.Employment[i].AgreementEndDate
                            , data.Coborrower.Employment[i].Salary
                            , data.Coborrower.Employment[i].SocialPayment
                            , data.Coborrower.Employment[i].Position);
                    cmd.Parameters.AddWithValue("@COBORROWER_WORK_DATA", tableCoborrowerWork).SqlDbType = SqlDbType.Structured;
                }
                cmd.ExecuteNonQuery();
            }
        }

        public void SaveACRAQueryResult(Guid id, ACRAData data)
        {
            using (SqlCommand cmd = new SqlCommand("Common.sp_SaveACRAQueryResult", ActiveConnection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = CommandTimeoutInterval;
                cmd.Parameters.Add(new SqlParameter("@APPLICATION_ID", SqlDbType.UniqueIdentifier)).Value = id;

                cmd.Parameters.Add(new SqlParameter("@FICO_SCORE", SqlDbType.Char, 3)).Value = data.Main.FicoScore;
                cmd.Parameters.Add(new SqlParameter("@RESPONSE_XML", SqlDbType.NVarChar, -1)).Value = ServiceHelper.GetFormattedXML(data.Main.XML);

                DataTable tableDetails = new DataTable("Common.ACRAQueryResultDetails");
                tableDetails.Columns.Add("STATUS", typeof(string));
                tableDetails.Columns.Add("FROM_DATE", typeof(DateTime));
                tableDetails.Columns.Add("TO_DATE", typeof(DateTime));
                tableDetails.Columns.Add("TYPE", typeof(string));
                tableDetails.Columns.Add("CUR", typeof(string));
                tableDetails.Columns.Add("CONTRACT_AMOUNT", typeof(decimal));
                tableDetails.Columns.Add("DEBT", typeof(decimal));
                tableDetails.Columns.Add("PAST_DUE_DATE", typeof(DateTime));
                tableDetails.Columns.Add("RISK", typeof(string));
                tableDetails.Columns.Add("CLASSIFICATION_DATE", typeof(DateTime));
                tableDetails.Columns.Add("INTEREST_RATE", typeof(decimal));
                tableDetails.Columns.Add("PLEDGE", typeof(string));
                tableDetails.Columns.Add("PLEDGE_AMOUNT", typeof(decimal));
                tableDetails.Columns.Add("OUTSTANDING_AMOUNT", typeof(decimal));
                tableDetails.Columns.Add("OUTSTANDING_PERCENT", typeof(decimal));
                tableDetails.Columns.Add("BANK_NAME", typeof(string));
                tableDetails.Columns.Add("IS_GUARANTEE", typeof(bool));
                tableDetails.Columns.Add("DUE_DAYS_1", typeof(int));
                tableDetails.Columns.Add("DUE_DAYS_2", typeof(int));
                tableDetails.Columns.Add("DUE_DAYS_3", typeof(int));
                tableDetails.Columns.Add("DUE_DAYS_4", typeof(int));
                tableDetails.Columns.Add("LOAN_ID", typeof(string));
                for (int i = 0; i < data.Main.Details.Count; i++)
                    if (data.Main.Details[i].CUR.Length == 3)
                        tableDetails.Rows.Add(data.Main.Details[i].STATUS, data.Main.Details[i].FROM_DATE, data.Main.Details[i].TO_DATE, data.Main.Details[i].TYPE
                            , data.Main.Details[i].CUR, data.Main.Details[i].CONTRACT_AMOUNT, data.Main.Details[i].DEBT, data.Main.Details[i].PAST_DUE_DATE, data.Main.Details[i].RISK, data.Main.Details[i].CLASSIFICATION_DATE
                            , data.Main.Details[i].INTEREST_RATE, data.Main.Details[i].PLEDGE, data.Main.Details[i].PLEDGE_AMOUNT, data.Main.Details[i].OUTSTANDING_AMOUNT, data.Main.Details[i].OUTSTANDING_PERCENT
                            , data.Main.Details[i].BANK_NAME, data.Main.Details[i].IS_GUARANTEE, data.Main.Details[i].DUE_DAYS_1, data.Main.Details[i].DUE_DAYS_2, data.Main.Details[i].DUE_DAYS_3, data.Main.Details[i].DUE_DAYS_4, data.Main.Details[i].LOAN_ID);
                cmd.Parameters.AddWithValue("@DETAILS", tableDetails).SqlDbType = SqlDbType.Structured;

                DataTable tableQueries = new DataTable("Common.ACRAQueryResultQueries");
                tableQueries.Columns.Add("DATE", typeof(DateTime));
                tableQueries.Columns.Add("BANK_NAME", typeof(string));
                for (int i = 0; i < data.Main.Queries.Count; i++)
                    tableQueries.Rows.Add(data.Main.Queries[i].DATE, data.Main.Queries[i].BANK_NAME);
                cmd.Parameters.AddWithValue("@QUERIES", tableQueries).SqlDbType = SqlDbType.Structured;

                DataTable tableInterrelated = new DataTable("ACRAQueryResultInterrelated");
                tableInterrelated.Columns.Add("FROM_DATE", typeof(DateTime));
                tableInterrelated.Columns.Add("TO_DATE", typeof(DateTime));
                tableInterrelated.Columns.Add("CUR", typeof(string));
                tableInterrelated.Columns.Add("RISK", typeof(string));
                tableInterrelated.Columns.Add("CONTRACT_AMOUNT", typeof(decimal));
                tableInterrelated.Columns.Add("DUE_AMOUNT", typeof(decimal));
                tableInterrelated.Columns.Add("OVERDUE_AMOUNT", typeof(decimal));
                tableInterrelated.Columns.Add("OUTSTANDING_PERCENT", typeof(decimal));
                for (int i = 0; i < data.Main.Interrelated.Count; i++)
                    if (data.Main.Interrelated[i].CUR.Length == 3)
                        tableInterrelated.Rows.Add(data.Main.Interrelated[i].FROM_DATE, data.Main.Interrelated[i].TO_DATE
                        , data.Main.Interrelated[i].CUR, data.Main.Interrelated[i].RISK, data.Main.Interrelated[i].CONTRACT_AMOUNT, data.Main.Interrelated[i].DUE_AMOUNT, data.Main.Interrelated[i].OVERDUE_AMOUNT, data.Main.Interrelated[i].OUTSTANDING_PERCENT);
                cmd.Parameters.AddWithValue("@INTERRELATED", tableInterrelated).SqlDbType = SqlDbType.Structured;

                DataTable tableCoborrowerDetails = new DataTable("Common.ACRAQueryResultDetails");
                tableCoborrowerDetails.Columns.Add("STATUS", typeof(string));
                tableCoborrowerDetails.Columns.Add("FROM_DATE", typeof(DateTime));
                tableCoborrowerDetails.Columns.Add("TO_DATE", typeof(DateTime));
                tableCoborrowerDetails.Columns.Add("TYPE", typeof(string));
                tableCoborrowerDetails.Columns.Add("CUR", typeof(string));
                tableCoborrowerDetails.Columns.Add("CONTRACT_AMOUNT", typeof(decimal));
                tableCoborrowerDetails.Columns.Add("DEBT", typeof(decimal));
                tableCoborrowerDetails.Columns.Add("PAST_DUE_DATE", typeof(DateTime));
                tableCoborrowerDetails.Columns.Add("RISK", typeof(string));
                tableCoborrowerDetails.Columns.Add("CLASSIFICATION_DATE", typeof(DateTime));
                tableCoborrowerDetails.Columns.Add("INTEREST_RATE", typeof(decimal));
                tableCoborrowerDetails.Columns.Add("PLEDGE", typeof(string));
                tableCoborrowerDetails.Columns.Add("PLEDGE_AMOUNT", typeof(decimal));
                tableCoborrowerDetails.Columns.Add("OUTSTANDING_AMOUNT", typeof(decimal));
                tableCoborrowerDetails.Columns.Add("OUTSTANDING_PERCENT", typeof(decimal));
                tableCoborrowerDetails.Columns.Add("BANK_NAME", typeof(string));
                tableCoborrowerDetails.Columns.Add("IS_GUARANTEE", typeof(bool));
                tableCoborrowerDetails.Columns.Add("DUE_DAYS_1", typeof(int));
                tableCoborrowerDetails.Columns.Add("DUE_DAYS_2", typeof(int));
                tableCoborrowerDetails.Columns.Add("DUE_DAYS_3", typeof(int));
                tableCoborrowerDetails.Columns.Add("DUE_DAYS_4", typeof(int));
                tableCoborrowerDetails.Columns.Add("LOAN_ID", typeof(string));

                DataTable tableCoborrowerQueries = new DataTable("Common.ACRAQueryResultQueries");
                tableCoborrowerQueries.Columns.Add("DATE", typeof(DateTime));
                tableCoborrowerQueries.Columns.Add("BANK_NAME", typeof(string));

                DataTable tableCoborrowerInterrelated = new DataTable("ACRAQueryResultInterrelated");
                tableCoborrowerInterrelated.Columns.Add("FROM_DATE", typeof(DateTime));
                tableCoborrowerInterrelated.Columns.Add("TO_DATE", typeof(DateTime));
                tableCoborrowerInterrelated.Columns.Add("CUR", typeof(string));
                tableCoborrowerInterrelated.Columns.Add("RISK", typeof(string));
                tableCoborrowerInterrelated.Columns.Add("CONTRACT_AMOUNT", typeof(decimal));
                tableCoborrowerInterrelated.Columns.Add("DUE_AMOUNT", typeof(decimal));
                tableCoborrowerInterrelated.Columns.Add("OVERDUE_AMOUNT", typeof(decimal));
                tableCoborrowerInterrelated.Columns.Add("OUTSTANDING_PERCENT", typeof(decimal));

                if (data.Coborrower != null)
                {
                    cmd.Parameters.Add(new SqlParameter("@COBORROWER_FICO_SCORE", SqlDbType.Char, 3)).Value = data.Coborrower.FicoScore;
                    cmd.Parameters.Add(new SqlParameter("@COBORROWER_RESPONSE_XML", SqlDbType.NVarChar, -1)).Value = ServiceHelper.GetFormattedXML(data.Coborrower.XML);

                    for (int i = 0; i < data.Coborrower.Details.Count; i++)
                        if (data.Coborrower.Details[i].CUR.Length == 3)
                            tableCoborrowerDetails.Rows.Add(data.Coborrower.Details[i].STATUS, data.Coborrower.Details[i].FROM_DATE, data.Coborrower.Details[i].TO_DATE, data.Coborrower.Details[i].TYPE
                                , data.Coborrower.Details[i].CUR, data.Coborrower.Details[i].CONTRACT_AMOUNT, data.Coborrower.Details[i].DEBT, data.Coborrower.Details[i].PAST_DUE_DATE, data.Coborrower.Details[i].RISK, data.Coborrower.Details[i].CLASSIFICATION_DATE
                                , data.Coborrower.Details[i].INTEREST_RATE, data.Coborrower.Details[i].PLEDGE, data.Coborrower.Details[i].PLEDGE_AMOUNT, data.Coborrower.Details[i].OUTSTANDING_AMOUNT, data.Coborrower.Details[i].OUTSTANDING_PERCENT
                                , data.Coborrower.Details[i].BANK_NAME, data.Coborrower.Details[i].IS_GUARANTEE, data.Coborrower.Details[i].DUE_DAYS_1, data.Coborrower.Details[i].DUE_DAYS_2, data.Coborrower.Details[i].DUE_DAYS_3, data.Coborrower.Details[i].DUE_DAYS_4, data.Coborrower.Details[i].LOAN_ID);

                    for (int i = 0; i < data.Coborrower.Queries.Count; i++)
                        tableCoborrowerQueries.Rows.Add(data.Coborrower.Queries[i].DATE, data.Coborrower.Queries[i].BANK_NAME);

                    for (int i = 0; i < data.Coborrower.Interrelated.Count; i++)
                        if (data.Coborrower.Interrelated[i].CUR.Length == 3)
                            tableInterrelated.Rows.Add(data.Coborrower.Interrelated[i].FROM_DATE, data.Coborrower.Interrelated[i].TO_DATE
                            , data.Coborrower.Interrelated[i].CUR, data.Coborrower.Interrelated[i].RISK, data.Coborrower.Interrelated[i].CONTRACT_AMOUNT, data.Coborrower.Interrelated[i].DUE_AMOUNT, data.Coborrower.Interrelated[i].OVERDUE_AMOUNT, data.Coborrower.Interrelated[i].OUTSTANDING_PERCENT);
                }

                cmd.Parameters.AddWithValue("@COBORROWER_DETAILS", tableCoborrowerDetails).SqlDbType = SqlDbType.Structured;
                cmd.Parameters.AddWithValue("@COBORROWER_QUERIES", tableCoborrowerQueries).SqlDbType = SqlDbType.Structured;
                cmd.Parameters.AddWithValue("@COBORROWER_INTERRELATED", tableCoborrowerInterrelated).SqlDbType = SqlDbType.Structured;

                cmd.ExecuteNonQuery();
            }
        }

        public List<NORQEntity> GetApplicationsForNORQRequest()
        {
            using (SqlCommand cmd = new SqlCommand("Common.sp_GetApplicationsForNORQRequest", ActiveConnection))
                return GetNORQEntities(cmd);
        }

        public List<ACRAEntity> GetApplicationsForACRARequest()
        {
            using (SqlCommand cmd = new SqlCommand("Common.sp_GetApplicationsForACRARequest", ActiveConnection))
                return GetACRAEntities(cmd);
        }

        public List<NORQEntity> GetApplicationForNORQRequestByID(Guid id)
        {
            using (SqlCommand cmd = new SqlCommand("Common.sp_GetApplicationForNORQRequestByID", ActiveConnection))
            {
                cmd.Parameters.Add(new SqlParameter("@ID", SqlDbType.UniqueIdentifier)).Value = id;
                return GetNORQEntities(cmd);
            }
        }

        public List<ACRAEntity> GetApplicationForACRARequestByID(Guid id)
        {
            using (SqlCommand cmd = new SqlCommand("Common.sp_GetApplicationForACRARequestByID", ActiveConnection))
            {
                cmd.Parameters.Add(new SqlParameter("@ID", SqlDbType.UniqueIdentifier)).Value = id;
                return GetACRAEntities(cmd);
            }
        }

        public List<NORQEntity> GetApplicationForNORQRequestByISN(int isn)
        {
            using (SqlCommand cmd = new SqlCommand("Common.sp_GetApplicationForNORQRequestByISN", ActiveConnection))
            {
                cmd.Parameters.Add(new SqlParameter("@ISN", SqlDbType.Int)).Value = isn;
                return GetNORQEntities(cmd);
            }
        }

        public List<ACRAEntity> GetApplicationForACRARequestByISN(int isn)
        {
            using (SqlCommand cmd = new SqlCommand("Common.sp_GetApplicationForACRARequestByISN", ActiveConnection))
            {
                cmd.Parameters.Add(new SqlParameter("@ISN", SqlDbType.Int)).Value = isn;
                return GetACRAEntities(cmd);
            }
        }

        public bool LockApplicationByID(Guid id, byte status)
        {
            bool result;
            using (SqlCommand cmd = new SqlCommand("Common.sp_LockApplicationByID", ActiveConnection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = CommandTimeoutInterval;
                cmd.Parameters.Add(new SqlParameter("@ID", SqlDbType.UniqueIdentifier)).Value = id;
                cmd.Parameters.Add(new SqlParameter("@STATUS", SqlDbType.TinyInt)).Value = status;
                using (SqlDataReader reader = cmd.ExecuteReader())
                    result = reader.Read();
            }
            return result;
        }

        private List<NORQEntity> GetNORQEntities(SqlCommand cmd)
        {
            List<NORQEntity> result = new List<NORQEntity>();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = CommandTimeoutInterval;
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    NORQEntity entity = new NORQEntity()
                    {
                        ID = reader.GetGuid(0),
                        SocialCardNumber = reader.GetString(1),
                        FirstName = reader.GetString(4),
                        LastName = reader.GetString(5),
                        BirthDate = reader.GetDateTime(6)
                    };
                    if (!reader.IsDBNull(7))
                    {
                        entity.CoborrowerSocialCardNumber = reader.GetString(7);
                        entity.CoborrowerFirstName = reader.GetString(8);
                        entity.CoborrowerLastName = reader.GetString(9);
                        entity.CoborrowerBirthDate = reader.GetDateTime(10);
                    }
                    result.Add(entity);
                }
            }
            return result;
        }

        private List<ACRAEntity> GetACRAEntities(SqlCommand cmd)
        {
            List<ACRAEntity> result = new List<ACRAEntity>();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = CommandTimeoutInterval;
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    ACRAEntity entity = new ACRAEntity()
                    {
                        ID = reader.GetGuid(0),
                        FirstName = reader.GetString(1),
                        LastName = reader.GetString(2),
                        BirthDate = reader.GetDateTime(3),
                        PassportNumber = reader.GetString(4),
                        SocialCardNumber = reader.GetString(5),
                        IDCardNumber = reader.GetString(6),
                        ImportID = reader.GetInt32(7)
                    };
                    if (!reader.IsDBNull(11))
                    {
                        entity.CoborrowerFirstName = reader.GetString(8);
                        entity.CoborrowerLastName = reader.GetString(9);
                        entity.CoborrowerBirthDate = reader.GetDateTime(10);
                        entity.CoborrowerSocialCardNumber = reader.GetString(11);
                        entity.CoborrowerPassportNumber = reader.GetString(12);
                        entity.CoborrowerIDCardNumber = reader.GetString(13);
                    }
                    result.Add(entity);
                }
            }
            return result;
        }

        public void SaveNORQTryCount(Guid id)
        {
            using (SqlCommand cmd = new SqlCommand("Common.sp_SaveNORQTryCount", ActiveConnection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = CommandTimeoutInterval;
                cmd.Parameters.Add(new SqlParameter("@APPLICATION_ID", SqlDbType.UniqueIdentifier)).Value = id;
                cmd.ExecuteNonQuery();
            }
        }

        public void SaveACRATryCount(Guid id)
        {
            using (SqlCommand cmd = new SqlCommand("Common.sp_SaveACRATryCount", ActiveConnection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = CommandTimeoutInterval;
                cmd.Parameters.Add(new SqlParameter("@APPLICATION_ID", SqlDbType.UniqueIdentifier)).Value = id;
                cmd.ExecuteNonQuery();
            }
        }

        public string GetSettingValue(string settingCode)
        {
            string result = string.Empty;
            using (SqlCommand cmd = new SqlCommand("Common.sp_GetSettings", ActiveConnection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = CommandTimeoutInterval;
                cmd.Parameters.Add(new SqlParameter("@CODE", SqlDbType.VarChar, 30)).Value = settingCode;
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    if (rdr.Read())
                    {
                        result = rdr.GetString(1);
                    }
                }
            }
            return result;
        }

        public string GetCachedNORQResponse(string socialCard)
        {
            string result = string.Empty;
            using (SqlCommand cmd = new SqlCommand("Common.sp_GetCachedNORQResponse", ActiveConnection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = CommandTimeoutInterval;
                cmd.Parameters.Add(new SqlParameter("@SOCIAL_CARD_NUMBER", SqlDbType.VarChar, 10)).Value = socialCard;
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    if (rdr.Read())
                    {
                        result = rdr.GetString(0);
                    }
                }
            }
            return result;
        }

        public string GetCachedACRAResponse(string socialCard)
        {
            string result = string.Empty;
            using (SqlCommand cmd = new SqlCommand("Common.sp_GetCachedACRAResponse", ActiveConnection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = CommandTimeoutInterval;
                cmd.Parameters.Add(new SqlParameter("@SOCIAL_CARD_NUMBER", SqlDbType.VarChar, 10)).Value = socialCard;
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    if (rdr.Read())
                    {
                        result = rdr.GetString(0);
                    }
                }
            }
            return result;
        }

        public string GetCachedACRAClientResponse(string socialCard)
        {
            string result = string.Empty;
            using (SqlCommand cmd = new SqlCommand("Common.sp_GetCachedACRAClientResponse", ActiveConnection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = CommandTimeoutInterval;
                cmd.Parameters.Add(new SqlParameter("@SOCIAL_CARD_NUMBER", SqlDbType.VarChar, 10)).Value = socialCard;
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    if (rdr.Read())
                    {
                        result = rdr.GetString(0);
                    }
                }
            }
            return result;
        }

        public void SaveNORQClientQueryResult(NORQQueryResult response, string userID)
        {
            using (SqlCommand cmd = new SqlCommand("Common.sp_SaveNORQClientQueryResult", ActiveConnection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = CommandTimeoutInterval;
                cmd.Parameters.Add(new SqlParameter("@SOCIAL_CARD_NUMBER", SqlDbType.Char, 10)).Value = response.SocialCardNumber;
                cmd.Parameters.Add(new SqlParameter("@FIRST_NAME", SqlDbType.NVarChar, 20)).Value = response.FirstName;
                cmd.Parameters.Add(new SqlParameter("@LAST_NAME", SqlDbType.NVarChar, 20)).Value = response.LastName;
                cmd.Parameters.Add(new SqlParameter("@PATRONYMIC_NAME", SqlDbType.NVarChar, 20)).Value = response.MiddleName;
                cmd.Parameters.Add(new SqlParameter("@BIRTH_DATE", SqlDbType.Date)).Value = response.BirthDate;
                cmd.Parameters.Add(new SqlParameter("@IS_DEAD", SqlDbType.Bit)).Value = response.IsDead;
                cmd.Parameters.Add(new SqlParameter("@GENDER", SqlDbType.Bit)).Value = response.Gender;
                cmd.Parameters.Add(new SqlParameter("@DISTRICT", SqlDbType.NVarChar, 20)).Value = response.District;
                cmd.Parameters.Add(new SqlParameter("@COMMUNITY", SqlDbType.NVarChar, 40)).Value = response.Community;
                cmd.Parameters.Add(new SqlParameter("@STREET", SqlDbType.NVarChar, 100)).Value = response.Street;
                cmd.Parameters.Add(new SqlParameter("@BUILDING", SqlDbType.NVarChar, 40)).Value = response.Building;
                cmd.Parameters.Add(new SqlParameter("@APARTMENT", SqlDbType.NVarChar, 40)).Value = response.Apartment;
                if (response.NonBiometricPassport.IsValid())
                {
                    cmd.Parameters.Add(new SqlParameter("@NON_BIOMETRIC_PASSPORT_NUMBER", SqlDbType.Char, 9)).Value = response.NonBiometricPassport.Number;
                    cmd.Parameters.Add(new SqlParameter("@NON_BIOMETRIC_PASSPORT_ISSUE_DATE", SqlDbType.Date)).Value = response.NonBiometricPassport.IssueDate;
                    cmd.Parameters.Add(new SqlParameter("@NON_BIOMETRIC_PASSPORT_EXPIRY_DATE", SqlDbType.Date)).Value = response.NonBiometricPassport.ExpiryDate;
                    cmd.Parameters.Add(new SqlParameter("@NON_BIOMETRIC_PASSPORT_ISSUED_BY", SqlDbType.Char, 3)).Value = response.NonBiometricPassport.IssuedBy;
                }
                if (response.BiometricPassport.IsValid())
                {
                    cmd.Parameters.Add(new SqlParameter("@BIOMETRIC_PASSPORT_NUMBER", SqlDbType.Char, 9)).Value = response.BiometricPassport.Number;
                    cmd.Parameters.Add(new SqlParameter("@BIOMETRIC_PASSPORT_ISSUE_DATE", SqlDbType.Date)).Value = response.BiometricPassport.IssueDate;
                    cmd.Parameters.Add(new SqlParameter("@BIOMETRIC_PASSPORT_EXPIRY_DATE", SqlDbType.Date)).Value = response.BiometricPassport.ExpiryDate;
                    cmd.Parameters.Add(new SqlParameter("@BIOMETRIC_PASSPORT_ISSUED_BY", SqlDbType.Char, 3)).Value = response.BiometricPassport.IssuedBy;
                }
                if (response.IDCard.IsValid())
                {
                    cmd.Parameters.Add(new SqlParameter("@ID_CARD_NUMBER", SqlDbType.Char, 9)).Value = response.IDCard.Number;
                    cmd.Parameters.Add(new SqlParameter("@ID_CARD_ISSUE_DATE", SqlDbType.Date)).Value = response.IDCard.IssueDate;
                    cmd.Parameters.Add(new SqlParameter("@ID_CARD_EXPIRY_DATE", SqlDbType.Date)).Value = response.IDCard.ExpiryDate;
                    cmd.Parameters.Add(new SqlParameter("@ID_CARD_ISSUED_BY", SqlDbType.Char, 3)).Value = response.IDCard.IssuedBy;
                }
                if (response.Employment.Count > 0)
                {
                    cmd.Parameters.Add(new SqlParameter("@ORGANIZATION_NAME", SqlDbType.NVarChar, 100)).Value = response.Employment[0].OrganizationName;
                    cmd.Parameters.Add(new SqlParameter("@REGISTRATION_CODE", SqlDbType.NVarChar, 20)).Value = response.Employment[0].RegistryCode;
                    cmd.Parameters.Add(new SqlParameter("@TAX_CODE", SqlDbType.VarChar, 20)).Value = response.Employment[0].TaxCode;
                    cmd.Parameters.Add(new SqlParameter("@ORGANIZATION_ADDRESS", SqlDbType.NVarChar, 100)).Value = response.Employment[0].OrganizationAddress;
                    cmd.Parameters.Add(new SqlParameter("@POSITION", SqlDbType.NVarChar, 100)).Value = response.Employment[0].Position;
                    cmd.Parameters.Add(new SqlParameter("@AGREEMENT_START_DATE", SqlDbType.Date)).Value = response.Employment[0].AgreementStartDate;
                    cmd.Parameters.Add(new SqlParameter("@AGREEMENT_END_DATE", SqlDbType.Date)).Value = response.Employment[0].AgreementEndDate;
                }
                cmd.Parameters.Add(new SqlParameter("@FEE", SqlDbType.Money)).Value = response.Salary;
                cmd.Parameters.Add(new SqlParameter("@RESPONSE_XML", SqlDbType.NVarChar, -1)).Value = ServiceHelper.GetFormattedXML(response.XML);
                cmd.Parameters.Add(new SqlParameter("@USER_ID", SqlDbType.Char, 4)).Value = userID;
                cmd.ExecuteNonQuery();
            }
        }

        public void SaveACRAClientQueryResult(ACRAQueryResult result, string socialCard, string firstName, string lastName, DateTime birthDate, string passport, string idCard, string userID)
        {
            using (SqlCommand cmd = new SqlCommand("Common.sp_SaveACRAClientQueryResult", ActiveConnection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = CommandTimeoutInterval;
                cmd.Parameters.Add(new SqlParameter("@SOCIAL_CARD_NUMBER", SqlDbType.Char, 10)).Value = socialCard;
                cmd.Parameters.Add(new SqlParameter("@FIRST_NAME", SqlDbType.NVarChar, 20)).Value = firstName;
                cmd.Parameters.Add(new SqlParameter("@LAST_NAME", SqlDbType.NVarChar, 20)).Value = lastName;
                cmd.Parameters.Add(new SqlParameter("@BIRTH_DATE", SqlDbType.Date)).Value = birthDate;
                cmd.Parameters.Add(new SqlParameter("@PASSPORT_NUMBER", SqlDbType.Char, 9)).Value = passport;
                cmd.Parameters.Add(new SqlParameter("@ID_CARD_NUMBER", SqlDbType.Char, 9)).Value = idCard;
                cmd.Parameters.Add(new SqlParameter("@USER_ID", SqlDbType.Char, 4)).Value = userID;
                cmd.Parameters.Add(new SqlParameter("@IS_BLOCKED", SqlDbType.Bit)).Value = result.IsBlocked;
                cmd.Parameters.Add(new SqlParameter("@FICO_SCORE", SqlDbType.Char, 3)).Value = result.FicoScore;
                cmd.Parameters.Add(new SqlParameter("@RESPONSE_XML", SqlDbType.NVarChar, -1)).Value = ServiceHelper.GetFormattedXML(result.XML);

                DataTable tableDetails = new DataTable("Common.ACRAQueryResultDetails");
                tableDetails.Columns.Add("STATUS", typeof(string));
                tableDetails.Columns.Add("FROM_DATE", typeof(DateTime));
                tableDetails.Columns.Add("TO_DATE", typeof(DateTime));
                tableDetails.Columns.Add("TYPE", typeof(string));
                tableDetails.Columns.Add("CUR", typeof(string));
                tableDetails.Columns.Add("CONTRACT_AMOUNT", typeof(decimal));
                tableDetails.Columns.Add("DEBT", typeof(decimal));
                tableDetails.Columns.Add("PAST_DUE_DATE", typeof(DateTime));
                tableDetails.Columns.Add("RISK", typeof(string));
                tableDetails.Columns.Add("CLASSIFICATION_DATE", typeof(DateTime));
                tableDetails.Columns.Add("INTEREST_RATE", typeof(decimal));
                tableDetails.Columns.Add("PLEDGE", typeof(string));
                tableDetails.Columns.Add("PLEDGE_AMOUNT", typeof(decimal));
                tableDetails.Columns.Add("OUTSTANDING_AMOUNT", typeof(decimal));
                tableDetails.Columns.Add("OUTSTANDING_PERCENT", typeof(decimal));
                tableDetails.Columns.Add("BANK_NAME", typeof(string));
                tableDetails.Columns.Add("IS_GUARANTEE", typeof(bool));
                tableDetails.Columns.Add("DUE_DAYS_1", typeof(int));
                tableDetails.Columns.Add("DUE_DAYS_2", typeof(int));
                tableDetails.Columns.Add("DUE_DAYS_3", typeof(int));
                tableDetails.Columns.Add("DUE_DAYS_4", typeof(int));
                tableDetails.Columns.Add("LOAN_ID", typeof(string));
                for (int i = 0; i < result.Details.Count; i++)
                    if (result.Details[i].CUR.Length == 3)
                        tableDetails.Rows.Add(result.Details[i].STATUS, result.Details[i].FROM_DATE, result.Details[i].TO_DATE, result.Details[i].TYPE
                            , result.Details[i].CUR, result.Details[i].CONTRACT_AMOUNT, result.Details[i].DEBT, result.Details[i].PAST_DUE_DATE, result.Details[i].RISK, result.Details[i].CLASSIFICATION_DATE
                            , result.Details[i].INTEREST_RATE, result.Details[i].PLEDGE, result.Details[i].PLEDGE_AMOUNT, result.Details[i].OUTSTANDING_AMOUNT, result.Details[i].OUTSTANDING_PERCENT
                            , result.Details[i].BANK_NAME, result.Details[i].IS_GUARANTEE, result.Details[i].DUE_DAYS_1, result.Details[i].DUE_DAYS_2, result.Details[i].DUE_DAYS_3, result.Details[i].DUE_DAYS_4, result.Details[i].LOAN_ID);
                cmd.Parameters.AddWithValue("@DETAILS", tableDetails).SqlDbType = SqlDbType.Structured;

                DataTable tableQueries = new DataTable("Common.ACRAQueryResultQueries");
                tableQueries.Columns.Add("DATE", typeof(DateTime));
                tableQueries.Columns.Add("BANK_NAME", typeof(string));
                for (int i = 0; i < result.Queries.Count; i++)
                    tableQueries.Rows.Add(result.Queries[i].DATE, result.Queries[i].BANK_NAME);
                cmd.Parameters.AddWithValue("@QUERIES", tableQueries).SqlDbType = SqlDbType.Structured;

                DataTable tableInterrelated = new DataTable("ACRAQueryResultInterrelated");
                tableInterrelated.Columns.Add("FROM_DATE", typeof(DateTime));
                tableInterrelated.Columns.Add("TO_DATE", typeof(DateTime));
                tableInterrelated.Columns.Add("CUR", typeof(string));
                tableInterrelated.Columns.Add("RISK", typeof(string));
                tableInterrelated.Columns.Add("CONTRACT_AMOUNT", typeof(decimal));
                tableInterrelated.Columns.Add("DUE_AMOUNT", typeof(decimal));
                tableInterrelated.Columns.Add("OVERDUE_AMOUNT", typeof(decimal));
                tableInterrelated.Columns.Add("OUTSTANDING_PERCENT", typeof(decimal));
                for (int i = 0; i < result.Interrelated.Count; i++)
                    if (result.Interrelated[i].CUR.Length == 3)
                        tableInterrelated.Rows.Add(result.Interrelated[i].FROM_DATE, result.Interrelated[i].TO_DATE
                        , result.Interrelated[i].CUR, result.Interrelated[i].RISK, result.Interrelated[i].CONTRACT_AMOUNT, result.Interrelated[i].DUE_AMOUNT, result.Interrelated[i].OVERDUE_AMOUNT, result.Interrelated[i].OUTSTANDING_PERCENT);
                cmd.Parameters.AddWithValue("@INTERRELATED", tableInterrelated).SqlDbType = SqlDbType.Structured;

                cmd.ExecuteNonQuery();
            }
        }

        public void SaveScoringResult(ScoringResult result)
        {
            using (SqlCommand cmd = new SqlCommand("Common.sp_SaveScoringResult", ActiveConnection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = CommandTimeoutInterval;
                cmd.Parameters.Add(new SqlParameter("@APPLICATION_ID", SqlDbType.UniqueIdentifier)).Value = result.ID;
                cmd.Parameters.Add(new SqlParameter("@SCORING_AMOUNT", SqlDbType.Money)).Value = result.Amount;
                cmd.Parameters.Add(new SqlParameter("@SCORING_COEFFICIENT", SqlDbType.Money)).Value = result.Coefficient;
                cmd.ExecuteNonQuery();
            }
        }

        public void SaveScoringResults(Guid id, List<ScoringResult> results)
        {
            using (SqlCommand cmd = new SqlCommand("Common.sp_SaveScoringResults", ActiveConnection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = CommandTimeoutInterval;
                cmd.Parameters.Add(new SqlParameter("@APPLICATION_ID", SqlDbType.UniqueIdentifier)).Value = id;

                DataTable tableResults = new DataTable("Common.ScoringResults");
                tableResults.Columns.Add("SCORING_OPTION", typeof(byte));
                tableResults.Columns.Add("SCORING_AMOUNT", typeof(decimal));
                tableResults.Columns.Add("SCORING_COEFFICIENT", typeof(decimal));
                tableResults.Columns.Add("SCORING_INTEREST", typeof(decimal));
                foreach (ScoringResult result in results)
                    tableResults.Rows.Add(result.Option, result.Amount, result.Coefficient, result.Interest);
                cmd.Parameters.AddWithValue("@RESULTS", tableResults).SqlDbType = SqlDbType.Structured;

                cmd.ExecuteNonQuery();
            }
        }

        public static int CommandTimeoutInterval { get; set; }

        public DataHelper()
        {
            this.ActiveConnection = new SqlConnection("context connection=true");
            this.ActiveConnection.Open();
        }

        ~DataHelper()
        {
            this.DisposeConnection();
        }

        public SqlConnection ActiveConnection { get; set; }

        public void Dispose()
        {
            this.DisposeConnection();
            GC.SuppressFinalize(this);
        }

        public void DisposeConnection()
        {
            if (this.ActiveConnection != null)
            {
                this.ActiveConnection.Close();
                this.ActiveConnection = null;
            }
        }
    }
}
