using Dapper;
using System;
using System.Collections.Generic;
using IntelART.Ameria.Entities;

namespace IntelART.Ameria.Repositories
{
    public class BankUserRepository : BaseRepository
    {
        public BankUserRepository(string connectionString) : base(connectionString)
        {
        }

        public IEnumerable<BankUser> GetBankUsers()
        {
            IEnumerable<BankUser> bankUsers = GetList<BankUser>(new DynamicParameters(), "IL.sp_GetBankUsers");
            return bankUsers;
        }

        public void CreateBankUser(BankUser bankUser, int applicationUserId)
        {
            DateTime passwordExpiryDate = GenerateUserPasswordExpiryDate();
            Dictionary<string, string> changes = new Dictionary<string, string>();
            changes.Add("Օգտագործող", bankUser.LOGIN.Trim());
            changes.Add("Անուն", bankUser.FIRST_NAME.Trim());
            changes.Add("Ազգանուն", bankUser.LAST_NAME.Trim());
            changes.Add("Email", bankUser.EMAIL.Trim());
            changes.Add("Ադմինիստրատոր", bankUser.IS_ADMINISTRATOR.Value.ToString());
            changes.Add("Գաղտնաբառի ժամկետ", FormatDate(passwordExpiryDate));

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("LOGIN", bankUser.LOGIN.Trim().ToUpper());
            parameters.Add("HASH", bankUser.HASH);
            parameters.Add("FIRST_NAME", bankUser.FIRST_NAME.Trim());
            parameters.Add("LAST_NAME", bankUser.LAST_NAME.Trim());
            parameters.Add("EMAIL", bankUser.EMAIL.Trim());
            parameters.Add("PASSWORD_EXPIRY_DATE", passwordExpiryDate);
            parameters.Add("IS_ADMINISTRATOR", bankUser.IS_ADMINISTRATOR.Value);
            parameters.Add("OPERATION_DETAILS", GenerateOperationDetailsString(changes));
            parameters.Add("APPLICATION_USER_ID", applicationUserId);
            Execute(parameters, "IL.sp_CreateBankUser");
        }

        public void ModifyBankUser(BankUser bankUser, int applicationUserId)
        {
            Dictionary<string, string> changes = new Dictionary<string, string>();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("BANK_USER_ID", bankUser.ID.Value);
            if (bankUser.IS_ADMINISTRATOR.HasValue)
            {
                parameters.Add("IS_ADMINISTRATOR", bankUser.IS_ADMINISTRATOR.Value);
                changes.Add("Ադմինիստրատոր", bankUser.IS_ADMINISTRATOR.Value.ToString());
            }
            if (!string.IsNullOrEmpty(bankUser.LOGIN))
            {
                parameters.Add("LOGIN", bankUser.LOGIN.Trim().ToUpper());
                changes.Add("Օգտագործող", bankUser.LOGIN.Trim());
            }
            if (!string.IsNullOrEmpty(bankUser.FIRST_NAME))
            {
                parameters.Add("FIRST_NAME", bankUser.FIRST_NAME.Trim());
                changes.Add("Անուն", bankUser.FIRST_NAME.Trim());
            }
            if (!string.IsNullOrEmpty(bankUser.LAST_NAME))
            {
                parameters.Add("LAST_NAME", bankUser.LAST_NAME.Trim());
                changes.Add("Ազգանուն", bankUser.LAST_NAME.Trim());
            }
            if (!string.IsNullOrEmpty(bankUser.EMAIL))
            {
                parameters.Add("EMAIL", bankUser.EMAIL.Trim());
                changes.Add("Email", bankUser.EMAIL.Trim());
            }
            if (!string.IsNullOrEmpty(bankUser.HASH))
            {
                parameters.Add("HASH", bankUser.HASH);
                changes.Add("Գաղտնաբառ", "***");
            }
            parameters.Add("OPERATION_DETAILS", GenerateOperationDetailsString(changes));
            parameters.Add("APPLICATION_USER_ID", applicationUserId);
            Execute(parameters, "IL.sp_ModifyBankUser");
        }

        public void CloseOpenBankUser(int bankUserID, int applicationUserID, bool closeOpen)
        {
            Dictionary<string, string> changes = new Dictionary<string, string>();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("BANK_USER_ID", bankUserID);
            parameters.Add("OPERATION_DETAILS", GenerateOperationDetailsString(changes));
            parameters.Add("APPLICATION_USER_ID", applicationUserID);
            parameters.Add("CLOSEOPEN", closeOpen);
            Execute(parameters, "IL.sp_CloseOpenBankUser");
        }

        public BankUser GetBankUser(int id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("ID", id);
            BankUser bankUser = GetSingle<BankUser>(parameters, "IL.sp_GetBankUser");
            return bankUser;
        }

        public BankUser AuthenticateBankUser(string login, string hash)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("LOGIN", login);
            parameters.Add("HASH", hash);
            BankUser bankUser = GetSingle<BankUser>(parameters, "IL.sp_AuthenticateBankUser");
            return bankUser;
        }

        public void ChangeBankUserPassword(string login, string hash)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("LOGIN", login);
            parameters.Add("HASH", hash);
            parameters.Add("PASSWORD_EXPIRY_DATE", GenerateUserPasswordExpiryDate());
            Execute(parameters, "Common.sp_ChangeApplicationUserPassword");
        }
    }
}
