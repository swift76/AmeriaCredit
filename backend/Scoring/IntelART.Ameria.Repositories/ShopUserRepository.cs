using Dapper;
using System;
using System.Collections.Generic;
using IntelART.Ameria.Entities;

namespace IntelART.Ameria.Repositories
{
    public class ShopUserRepository : BaseRepository
    {
        public ShopUserRepository(string connectionString) : base(connectionString)
        {
        }

        public IEnumerable<ShopUser> GetShopUsers()
        {
            IEnumerable<ShopUser> shopUsers = GetList<ShopUser>(new DynamicParameters(), "IL.sp_GetShopUsers");
            return shopUsers;
        }

        public void CreateShopUser(ShopUser shopUser, int applicationUserID)
        {
            string login = shopUser.LOGIN.Replace(" ","").ToUpper();
            DateTime passwordExpiryDate = GenerateUserPasswordExpiryDate();
            Dictionary<string, string> changes = new Dictionary<string, string>();
            changes.Add("Օգտագործող", login);
            changes.Add("Անուն", shopUser.FIRST_NAME.Trim());
            changes.Add("Ազգանուն", shopUser.LAST_NAME.Trim());
            changes.Add("Email", shopUser.EMAIL.Trim());
            changes.Add("Հեռախոս", shopUser.MOBILE_PHONE.Trim());
            changes.Add("Խանութ", shopUser.SHOP_CODE.Trim());
            changes.Add("Ղեկավար", shopUser.IS_MANAGER.Value.ToString());
            changes.Add("Գաղտնաբառի ժամկետ", FormatDate(passwordExpiryDate));

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("LOGIN", login);
            parameters.Add("HASH", shopUser.HASH);
            parameters.Add("FIRST_NAME", shopUser.FIRST_NAME.Trim());
            parameters.Add("LAST_NAME", shopUser.LAST_NAME.Trim());
            parameters.Add("EMAIL", shopUser.EMAIL.Trim());
            parameters.Add("PASSWORD_EXPIRY_DATE", passwordExpiryDate);
            parameters.Add("SHOP_CODE", shopUser.SHOP_CODE.Trim());
            parameters.Add("IS_MANAGER", shopUser.IS_MANAGER.Value.ToString());
            parameters.Add("MOBILE_PHONE", shopUser.MOBILE_PHONE.Trim());
            parameters.Add("OPERATION_DETAILS", GenerateOperationDetailsString(changes));
            parameters.Add("APPLICATION_USER_ID", applicationUserID);
            Execute(parameters, "IL.sp_CreateShopUser");
        }

        public void ModifyShopUser(ShopUser shopUser, int applicationUserID)
        {
            string login = shopUser.LOGIN.Replace(" ", "").ToUpper();
            Dictionary<string, string> changes = new Dictionary<string, string>();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("SHOP_USER_ID", shopUser.ID.Value);
            if (shopUser.IS_MANAGER.HasValue)
            {
                parameters.Add("IS_MANAGER", shopUser.IS_MANAGER.Value);
                changes.Add("Ղեկավար", shopUser.IS_MANAGER.Value.ToString());
            }
            if (!string.IsNullOrEmpty(login))
            {
                parameters.Add("LOGIN", login);
                changes.Add("Օգտագործող", login);
            }
            if (!string.IsNullOrEmpty(shopUser.FIRST_NAME.Trim()))
            {
                parameters.Add("FIRST_NAME", shopUser.FIRST_NAME.Trim());
                changes.Add("Անուն", shopUser.FIRST_NAME.Trim());
            }
            if (!string.IsNullOrEmpty(shopUser.LAST_NAME.Trim()))
            {
                parameters.Add("LAST_NAME", shopUser.LAST_NAME.Trim());
                changes.Add("Ազգանուն", shopUser.LAST_NAME.Trim());
            }
            if (!string.IsNullOrEmpty(shopUser.EMAIL))
            {
                parameters.Add("EMAIL", shopUser.EMAIL.Trim());
                changes.Add("Email", shopUser.EMAIL.Trim());
            }
            if (!string.IsNullOrEmpty(shopUser.HASH))
            {
                parameters.Add("HASH", shopUser.HASH);
                changes.Add("Գաղտնաբառ", "***");
            }
            if (!string.IsNullOrEmpty(shopUser.SHOP_CODE.Trim()))
            {
                parameters.Add("SHOP_CODE", shopUser.SHOP_CODE.Trim());
                changes.Add("Խանութ", shopUser.SHOP_CODE.Trim());
            }
            if (!string.IsNullOrEmpty(shopUser.MOBILE_PHONE.Trim()))
            {
                parameters.Add("MOBILE_PHONE", shopUser.MOBILE_PHONE.Trim());
                changes.Add("Հեռախոս", shopUser.MOBILE_PHONE.Trim());
            }
            parameters.Add("OPERATION_DETAILS", GenerateOperationDetailsString(changes));
            parameters.Add("APPLICATION_USER_ID", applicationUserID);
            Execute(parameters, "IL.sp_ModifyShopUser");
        }

        public void CloseOpenShopUser(int shop_user_id, int application_user_id, bool closeOpen)
        {
            Dictionary<string, string> changes = new Dictionary<string, string>();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("SHOP_USER_ID", shop_user_id);
            parameters.Add("OPERATION_DETAILS", GenerateOperationDetailsString(changes));
            parameters.Add("APPLICATION_USER_ID", application_user_id);
            parameters.Add("CLOSEOPEN", closeOpen);
            Execute(parameters, "IL.sp_CloseOpenShopUser");
        }

        public ShopUser GetShopUser(int id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("ID", id);
            ShopUser shopUser = GetSingle<ShopUser>(parameters, "IL.sp_GetShopUser");
            return shopUser;
        }

        public ShopUser AuthenticateShopUser(string login, string hash)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("LOGIN", login);
            parameters.Add("HASH", hash);
            ShopUser shopUser = GetSingle<ShopUser>(parameters, "IL.sp_AuthenticateShopUser");
            return shopUser;
        }

        public void ChangeShopUserPassword(string login, string hash)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("LOGIN", login);
            parameters.Add("HASH", hash);
            parameters.Add("PASSWORD_EXPIRY_DATE", GenerateUserPasswordExpiryDate());
            Execute(parameters, "Common.sp_ChangeApplicationUserPassword");
        }
    }
}
