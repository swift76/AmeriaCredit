using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using IntelART.Ameria.Entities;

namespace IntelART.Ameria.Repositories
{
    public class CustomerUserRepository : BaseRepository
    {
        public CustomerUserRepository(string connectionString) : base(connectionString)
        {
        }

        public void StartRegistrationProcess(CustomerUserRegistrationPreVerification registrationProcess)
        {
            this.CheckCustomerUserExistence(registrationProcess.MOBILE_PHONE, registrationProcess.EMAIL, registrationProcess.SOCIAL_CARD_NUMBER);

            Dictionary<string, string> changes = new Dictionary<string, string>();
            DynamicParameters parameters = new DynamicParameters();

            PrepareParameters(changes, parameters, "FIRST_NAME_EN",      registrationProcess.FIRST_NAME_EN,      true);
            PrepareParameters(changes, parameters, "LAST_NAME_EN",       registrationProcess.LAST_NAME_EN,       true);
            PrepareParameters(changes, parameters, "SOCIAL_CARD_NUMBER", registrationProcess.SOCIAL_CARD_NUMBER, true);
            PrepareParameters(changes, parameters, "MOBILE_PHONE",       registrationProcess.MOBILE_PHONE,       true);
            PrepareParameters(changes, parameters, "EMAIL",              registrationProcess.EMAIL,              true);
            PrepareParameters(changes, parameters, "PROCESS_ID",         registrationProcess.PROCESS_ID,         true);
            PrepareParameters(changes, parameters, "VERIFICATION_CODE",  registrationProcess.VERIFICATION_CODE,  true);

            parameters.Add("HASH", registrationProcess.HASH);
            parameters.Add("ONBOARDING_ID", registrationProcess.ONBOARDING_ID);
            parameters.Add("IS_STUDENT", registrationProcess.IS_STUDENT);
            Execute(parameters, "Common.sp_StartCustomerUserRegistrationProcess");
        }

        public CustomerUserRegistrationPreVerification GetRegistrationProcess(Guid registrationProcessId)
        {
            Dictionary<string, string> changes = new Dictionary<string, string>();
            DynamicParameters parameters = new DynamicParameters();
            PrepareParameters(changes, parameters, "PROCESS_ID", registrationProcessId, true);
            CustomerUserRegistrationPreVerification result = GetSingle<CustomerUserRegistrationPreVerification>(parameters, "Common.sp_GetCustomerUserRegistrationProcess");
            return result;
        }

        public void UpdateRegistrationProcess(Guid registrationProcessId, string verificationCode)
        {
            Dictionary<string, string> changes = new Dictionary<string, string>();
            DynamicParameters parameters = new DynamicParameters();
            PrepareParameters(changes, parameters, "PROCESS_ID", registrationProcessId, true);
            PrepareParameters(changes, parameters, "VERIFICATION_CODE", verificationCode, true);
            Execute(parameters, "Common.sp_UpdateCustomerUserRegistrationProcess");
        }

        /// <summary>
        /// Creates a customer user during registration
        /// </summary>
        public int CreateCustomerUser(CustomerUser customerUser)
        {
            this.CheckCustomerUserExistence(customerUser.MOBILE_PHONE, customerUser.EMAIL, customerUser.SOCIAL_CARD_NUMBER);

            Dictionary<string, string> changes = new Dictionary<string, string>();
            DynamicParameters parameters = new DynamicParameters();

            PrepareParameters(changes, parameters, "FIRST_NAME_EN",      customerUser.FIRST_NAME_EN,      true);
            PrepareParameters(changes, parameters, "LAST_NAME_EN",       customerUser.LAST_NAME_EN,       true);
            PrepareParameters(changes, parameters, "SOCIAL_CARD_NUMBER", customerUser.SOCIAL_CARD_NUMBER, true);
            PrepareParameters(changes, parameters, "MOBILE_PHONE",       customerUser.MOBILE_PHONE,       true);
            PrepareParameters(changes, parameters, "EMAIL",              customerUser.EMAIL,              true);
            
            parameters.Add("HASH", customerUser.HASH);
            parameters.Add("ONBOARDING_ID", customerUser.ONBOARDING_ID);
            parameters.Add("IS_STUDENT", customerUser.IS_STUDENT);
            parameters.Add("IS_EMAIL_VERIFIED", customerUser.IS_EMAIL_VERIFIED);
            parameters.Add("OPERATION_DETAILS", GenerateOperationDetailsString(changes));
            parameters.Add("APPLICATION_USER_ID", dbType: DbType.Int32, direction: ParameterDirection.Output);
            Execute(parameters, "Common.sp_CreateCustomerUser");
            
            int id = parameters.Get<int>("APPLICATION_USER_ID");
            return id;
        }

        /// <summary>
        /// Modifies a customer user from personal data page
        /// </summary>
        public void ModifyCustomerUser(CustomerUser customerUser, int userID)
        {
            this.CheckCustomerUserExistence(customerUser.MOBILE_PHONE, customerUser.EMAIL, customerUser.SOCIAL_CARD_NUMBER, userID);

            Dictionary<string, string> changes = new Dictionary<string, string>();
            DynamicParameters parameters = new DynamicParameters();
            PrepareParameters(changes, parameters, "APPLICATION_USER_ID",       userID, true);
            PrepareParameters(changes, parameters, "FIRST_NAME_EN",             customerUser.FIRST_NAME_EN, true);
            PrepareParameters(changes, parameters, "LAST_NAME_EN",              customerUser.LAST_NAME_EN, true);
            PrepareParameters(changes, parameters, "FIRST_NAME",                customerUser.FIRST_NAME, true);
            PrepareParameters(changes, parameters, "LAST_NAME",                 customerUser.LAST_NAME, true);
            PrepareParameters(changes, parameters, "PATRONYMIC_NAME",           customerUser.PATRONYMIC_NAME, true);
            PrepareParameters(changes, parameters, "BIRTH_DATE",                customerUser.BIRTH_DATE);
            PrepareParameters(changes, parameters, "BIRTH_PLACE_CODE",          customerUser.BIRTH_PLACE_CODE);
            PrepareParameters(changes, parameters, "CITIZENSHIP_CODE",          customerUser.CITIZENSHIP_CODE);
            PrepareParameters(changes, parameters, "MOBILE_PHONE",              customerUser.MOBILE_PHONE, true);
            PrepareParameters(changes, parameters, "EMAIL",                     customerUser.EMAIL, true);
            PrepareParameters(changes, parameters, "SOCIAL_CARD_NUMBER",        customerUser.SOCIAL_CARD_NUMBER, true);
            PrepareParameters(changes, parameters, "DOCUMENT_TYPE_CODE",        customerUser.DOCUMENT_TYPE_CODE);
            PrepareParameters(changes, parameters, "DOCUMENT_NUMBER",           customerUser.DOCUMENT_NUMBER);
            PrepareParameters(changes, parameters, "DOCUMENT_GIVEN_DATE",       customerUser.DOCUMENT_GIVEN_DATE);
            PrepareParameters(changes, parameters, "DOCUMENT_EXPIRY_DATE",      customerUser.DOCUMENT_EXPIRY_DATE);
            PrepareParameters(changes, parameters, "DOCUMENT_GIVEN_BY",         customerUser.DOCUMENT_GIVEN_BY);
            PrepareParameters(changes, parameters, "REGISTRATION_COUNTRY_CODE", customerUser.REGISTRATION_COUNTRY_CODE);
            PrepareParameters(changes, parameters, "REGISTRATION_STATE_CODE",   customerUser.REGISTRATION_STATE_CODE);
            PrepareParameters(changes, parameters, "REGISTRATION_CITY_CODE",    customerUser.REGISTRATION_CITY_CODE);
            PrepareParameters(changes, parameters, "REGISTRATION_STREET",       customerUser.REGISTRATION_STREET);
            PrepareParameters(changes, parameters, "REGISTRATION_BUILDNUM",     customerUser.REGISTRATION_BUILDNUM);
            PrepareParameters(changes, parameters, "REGISTRATION_APARTMENT",    customerUser.REGISTRATION_APARTMENT);
            PrepareParameters(changes, parameters, "CURRENT_COUNTRY_CODE",      customerUser.CURRENT_COUNTRY_CODE);
            PrepareParameters(changes, parameters, "CURRENT_STATE_CODE",        customerUser.CURRENT_STATE_CODE);
            PrepareParameters(changes, parameters, "CURRENT_CITY_CODE",         customerUser.CURRENT_CITY_CODE);
            PrepareParameters(changes, parameters, "CURRENT_STREET",            customerUser.CURRENT_STREET);
            PrepareParameters(changes, parameters, "CURRENT_BUILDNUM",          customerUser.CURRENT_BUILDNUM);
            PrepareParameters(changes, parameters, "CURRENT_APARTMENT",         customerUser.CURRENT_APARTMENT);

            parameters.Add("OPERATION_DETAILS", GenerateOperationDetailsString(changes));
            Execute(parameters, "Common.sp_ModifyCustomerUser");
        }

        private bool CheckCustomerUserExistenceByParameter(string parameterName, string parameterValue, int? userID, string procedureName)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add(parameterName, parameterValue);
            List<int?> customerUser = GetList<int?>(parameters, string.Format("Common.{0}", procedureName)).ToList();
            if (userID.HasValue &&
                (customerUser.Count > 1 || (customerUser.Count == 1 && !customerUser.Contains(userID)))) // modify customer user
            {
                return true;
            }
            else if (!userID.HasValue && customerUser.Count > 0) // register customer user
            {
                return true;
            }
            return false;
        }

        private void CheckCustomerUserExistence(string mobilePhone, string email, string socialCard, int? userID = null)
        {
            if (this.CheckCustomerUserExistenceByParameter("MOBILE_PHONE", mobilePhone, userID, "sp_CheckCustomerUserExistenceByMobilePhone"))
            {
                throw new ApplicationException("ERR-0040", "Նման բջջային հեռախոսահամարով առկա է օգտատեր։");
            }
            else if (this.CheckCustomerUserExistenceByParameter("EMAIL", email, userID, "sp_CheckCustomerUserExistenceByEmail"))
            {
                throw new ApplicationException("ERR-0041", "Նման էլեկտրոնային հասցեով առկա է օգտատեր։");
            }
            else if (this.CheckCustomerUserExistenceByParameter("SOCIAL_CARD_NUMBER", socialCard, userID, "sp_CheckCustomerUserExistenceBySocialCard"))
            {
                throw new ApplicationException("ERR-0042", "Նման ՀԾՀՀ / սոցիալական քարտի համարով առկա է օգտատեր։");
            }
        }

        public bool CheckCustomerUserExistenceByPhone(string mobilePhone)
        {
            return this.CheckCustomerUserExistenceByParameter("MOBILE_PHONE", mobilePhone, null, "sp_CheckCustomerUserExistenceByLogin");
        }

        public CustomerUser GetCustomerUser(int id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("APPLICATION_USER_ID", id);
            CustomerUser customerUser = GetSingle<CustomerUser>(parameters, "Common.sp_GetCustomerUser");
            return customerUser;
        }

        public CustomerUser AuthenticateCustomerUser(string login, string hash)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("LOGIN", login);
            parameters.Add("HASH", hash);
            CustomerUser customerUser = GetSingle<CustomerUser>(parameters, "Common.sp_AuthenticateCustomerUser");
            return customerUser;
        }

        public void ChangeCustomerUserPassword(string login, string hash)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("LOGIN", login);
            parameters.Add("HASH", hash);
            parameters.Add("PASSWORD_EXPIRY_DATE", GenerateUserPasswordExpiryDate());
            Execute(parameters, "Common.sp_ChangeApplicationUserPassword");
        }

        public void ResetCustomerUserPassword(string login, string smsCode, Guid processId, string passwordHash)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("PROCESS_ID", processId);
            parameters.Add("PHONE", login);
            parameters.Add("VERIFICATION_CODE_HASH", smsCode);
            parameters.Add("PASSWORD_HASH", passwordHash);
            Execute(parameters, "Common.sp_UpdateCustomerUserPassword");
        }

        public void StartCustomerUserPasswordResetProcess(string phone, Guid processId, string smsCode)
        {
            Dictionary<string, string> changes = new Dictionary<string, string>();
            DynamicParameters parameters = new DynamicParameters();

            PrepareParameters(changes, parameters, "PROCESS_ID", processId, true);
            PrepareParameters(changes, parameters, "PHONE", phone, true);
            PrepareParameters(changes, parameters, "HASH", smsCode, true);

            Execute(parameters, "Common.sp_StartCustomerUserPasswordResetProcess");
        }

        public void SaveOnboardingCustomer(OnboardingData onboardingData)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("ID", onboardingData.id);
            parameters.Add("first_name_eng", onboardingData.first_name_eng);
            parameters.Add("last_name_eng", onboardingData.last_name_eng);
            parameters.Add("mobile_number", onboardingData.mobile_number);
            parameters.Add("email", onboardingData.email);
            parameters.Add("birth_date", onboardingData.birth_date);
            parameters.Add("first_name_arm", onboardingData.first_name_arm);
            parameters.Add("last_name_arm", onboardingData.last_name_arm);
            parameters.Add("middle_name_arm", onboardingData.middle_name_arm);
            parameters.Add("document_type_id", onboardingData.document_type_id);
            parameters.Add("document_number", onboardingData.document_number);
            parameters.Add("soccard_number", onboardingData.soccard_number);
            parameters.Add("document_issue_date", onboardingData.document_issue_date);
            parameters.Add("document_issuer", onboardingData.document_issuer);
            parameters.Add("is_student", onboardingData.is_student);
            Execute(parameters, "Common.sp_SaveOnboardingCustomer");
        }

        public OnboardingData GetOnboardingCustomer(Guid id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("ID", id);
            OnboardingData onboardingData = GetSingle<OnboardingData>(parameters, "Common.sp_GetOnboardingCustomer");
            if (onboardingData != null)
                onboardingData.id = id;
            return onboardingData;
        }

        public ServiceConfiguration GetServiceConfiguration(string code)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("CODE", code);
            ServiceConfiguration config = GetSingle<ServiceConfiguration>(parameters, "Common.sp_GetServiceConfiguration");
            return config;
        }

        public ApplicationForOnboarding GetApplicationForOnboarding(Guid applicationID)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("ID", applicationID);
            ApplicationForOnboarding application = GetSingle<ApplicationForOnboarding>(parameters, "Common.sp_GetApplicationForOnboarding");
            return application;
        }

        public void StartCustomerUserEmailVerificationProcess(Guid processId, int userId, string email)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("PROCESS_ID", processId);
            parameters.Add("CUSTOMER_USER_ID", userId);
            parameters.Add("EMAIL", email);
            Execute(parameters, "Common.sp_StartCustomerUserEmailVerificationProcess");
        }

        public void UpdateCustomerUserEmailVerificationProcess(Guid processId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("PROCESS_ID", processId);
            Execute(parameters, "Common.sp_UpdateCustomerUserEmailVerificationProcess");
        }

        public void SetTryCustomerUserRegistrationProcess(Guid processId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("PROCESS_ID", processId);
            Execute(parameters, "Common.sp_SetTryCustomerUserRegistrationProcess");
        }

        public async Task<IEnumerable<string>> GetRestrictedPasswords()
        {
            return await GetListAsync<string>(new DynamicParameters(), "Common.sp_GetRestrictedPasswords");
        }
    }
}
