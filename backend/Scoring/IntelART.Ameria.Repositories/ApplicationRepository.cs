using System;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using IntelART.Ameria.Entities;
using IntelART.Utilities;
using System.Collections.Generic;

namespace IntelART.Ameria.Repositories
{
    public class ApplicationRepository : BaseRepository
    {
        public ApplicationRepository(string connectionString) : base(connectionString)
        {
        }

        #region Initial Application

        /// <summary>
        /// Create initial application to get scoring results
        /// </summary>
        public async Task<Guid> CreateInitialApplication(InitialApplication application, int? currentUserID, bool isShopUser)
        {
            Dictionary<string, string> changes = new Dictionary<string, string>();
            DynamicParameters parameters = new DynamicParameters();

            bool isMandatory = isShopUser || application.SUBMIT;
            PrepareParameters(changes, parameters, "SOURCE_ID",            isShopUser ? 2 : 1,               isMandatory);
            PrepareParameters(changes, parameters, "LOAN_TYPE_ID",         application.LOAN_TYPE_ID,         isMandatory);
            PrepareParameters(changes, parameters, "FIRST_NAME",           application.FIRST_NAME,           isMandatory);
            PrepareParameters(changes, parameters, "LAST_NAME",            application.LAST_NAME,            isMandatory);
            PrepareParameters(changes, parameters, "PATRONYMIC_NAME",      application.PATRONYMIC_NAME,      isMandatory);
            PrepareParameters(changes, parameters, "BIRTH_DATE",           application.BIRTH_DATE,           isMandatory);
            PrepareParameters(changes, parameters, "SOCIAL_CARD_NUMBER",   application.SOCIAL_CARD_NUMBER,   isMandatory);
            PrepareParameters(changes, parameters, "DOCUMENT_TYPE_CODE",   application.DOCUMENT_TYPE_CODE,   isMandatory);
            PrepareParameters(changes, parameters, "DOCUMENT_NUMBER",      application.DOCUMENT_NUMBER,      isMandatory);
            PrepareParameters(changes, parameters, "DOCUMENT_GIVEN_DATE",  application.DOCUMENT_GIVEN_DATE);
            PrepareParameters(changes, parameters, "DOCUMENT_EXPIRY_DATE", application.DOCUMENT_EXPIRY_DATE);
            PrepareParameters(changes, parameters, "DOCUMENT_GIVEN_BY",    application.DOCUMENT_GIVEN_BY);
            PrepareParameters(changes, parameters, "INITIAL_AMOUNT",       application.INITIAL_AMOUNT,       isMandatory);

            if (application.LOAN_TYPE_ID == "00")
            {
                PrepareParameters(changes, parameters, "MOBILE_PHONE_1",        application.MOBILE_PHONE_1,        isMandatory);
                PrepareParameters(changes, parameters, "PRODUCT_CATEGORY_CODE", application.PRODUCT_CATEGORY_CODE, isMandatory);
                PrepareParameters(changes, parameters, "LOAN_TEMPLATE_CODE",    application.LOAN_TEMPLATE_CODE,    isMandatory);
            }
            else
            {
                PrepareParameters(changes, parameters, "ORGANIZATION_ACTIVITY_CODE", application.ORGANIZATION_ACTIVITY_CODE, isMandatory);
                PrepareParameters(changes, parameters, "IS_REFINANCING",             application.IS_REFINANCING);
            }

            PrepareParameters(changes, parameters, "CURRENCY_CODE",        application.CURRENCY_CODE,        isMandatory);

            PrepareParameters(changes, parameters, "UNIVERSITY_CODE", application.UNIVERSITY_CODE);
            PrepareParameters(changes, parameters, "UNIVERSITY_FACULTY", application.UNIVERSITY_FACULTY);
            PrepareParameters(changes, parameters, "UNIVERSITY_YEAR", application.UNIVERSITY_YEAR);

            if (currentUserID != null)
                PrepareParameters(changes, parameters, isShopUser ? "SHOP_USER_ID" : "CUSTOMER_USER_ID", currentUserID, isMandatory);
            PrepareParameters(changes, parameters, "PARTNER_COMPANY_CODE", application.PARTNER_COMPANY_CODE, false);

            parameters.Add("OPERATION_DETAILS", GenerateOperationDetailsString(changes));
            parameters.Add("ID", dbType: DbType.Guid, direction: ParameterDirection.InputOutput, value: application.ID);
            parameters.Add("IS_SUBMIT", application.SUBMIT);
            await ExecuteAsync(parameters, "Common.sp_SaveInitialApplication");
            Guid id = parameters.Get<Guid>("ID");

            if (application.SUBMIT)
            {
                parameters = new DynamicParameters();
                parameters.Add("ID", id);
                parameters.Add("queryTimeout", 180);
                ExecuteAsyncNoWait(parameters, "Common.sp_ProcessScoringQueriesByID");
            }

            return id;
        }

        public async Task<InitialApplication> GetInitialApplication(Guid id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("ID", id);
            InitialApplication application = await GetSingleAsync<InitialApplication>(parameters, "Common.sp_GetInitialApplication");
            this.ApplyMappingSingle(application);
            return application;
        }

        public async Task<ApplicationContractDetails> GetApplicationContractDetails(Guid id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("ID", id);
            ApplicationContractDetails application = await GetSingleAsync<ApplicationContractDetails>(parameters, "Common.sp_GetApplicationContractDetails");
            return application;
        }
        
        public async Task SubmitInitialApplication(Guid id)
        {
            string statusState = await GetApplicationStatusState(id);
            if (statusState == "NEW")
            {
                await this.ChangeApplicationStatus(id, 1);
            }
        }

        #endregion

        #region Main Application

        /// <summary>
        /// Create main application after getting scoring results
        /// </summary>
        public async Task CreateMainApplication(Guid id, MainApplication application, InitialApplication initial, int? currentUserID, bool isShopUser)
        {
            Dictionary<string, string> changes = new Dictionary<string, string>();
            DynamicParameters parameters = new DynamicParameters();
            if (application.SUBMIT &&
                !application.AGREED_WITH_TERMS &&
                !isShopUser &&
                application.LOAN_TYPE_STATE == "INSTALLATION_LOAN")
            {
                throw new ApplicationException("ERR-0036", "Պայմաններին և կանոններին Ձեր համաձայնությունը պարտադիր է։");
            }
            bool isMandatory = isShopUser || application.SUBMIT;
            if (isMandatory && application.FINAL_AMOUNT > GetMaxApprovedAmount(id))
            {
                throw new ApplicationException("ERR-0037", "Գումարի սահմանաչափի խախտում:");
            }
            bool canSkipPersonalDetails = false;
            if (initial != null)
            {
                canSkipPersonalDetails = application.LOAN_TYPE_STATE != "INSTALLATION_LOAN"
                    && !isShopUser
                    && initial.IS_DATA_COMPLETE;
            }

            application.EMAIL = application.EMAIL.Trim();

            PrepareParameters(changes, parameters, "APPLICATION_ID",   id,                           isMandatory);
            PrepareParameters(changes, parameters, "FINAL_AMOUNT",     application.FINAL_AMOUNT,     isMandatory);
            PrepareParameters(changes, parameters, "INTEREST",         application.INTEREST);
            PrepareParameters(changes, parameters, "REPAY_DAY",        application.REPAY_DAY);
            PrepareParameters(changes, parameters, "PERIOD_TYPE_CODE", application.PERIOD_TYPE_CODE, isMandatory);
            PrepareParameters(changes, parameters, "FIRST_NAME_EN",    application.FIRST_NAME_EN,    isMandatory || canSkipPersonalDetails);
            PrepareParameters(changes, parameters, "LAST_NAME_EN",     application.LAST_NAME_EN,     isMandatory || canSkipPersonalDetails);
            PrepareParameters(changes, parameters, "BIRTH_PLACE_CODE", application.BIRTH_PLACE_CODE, isMandatory || canSkipPersonalDetails);
            PrepareParameters(changes, parameters, "CITIZENSHIP_CODE", application.CITIZENSHIP_CODE, isMandatory || canSkipPersonalDetails);
            PrepareParameters(changes, parameters, "FIXED_PHONE",      application.FIXED_PHONE);
            PrepareParameters(changes, parameters, "EMAIL",            application.EMAIL,            isMandatory);
            PrepareParameters(changes, parameters, "REGISTRATION_COUNTRY_CODE", application.REGISTRATION_COUNTRY_CODE, isMandatory || canSkipPersonalDetails);
            PrepareParameters(changes, parameters, "REGISTRATION_STATE_CODE",   application.REGISTRATION_STATE_CODE,   isMandatory || canSkipPersonalDetails);
            PrepareParameters(changes, parameters, "REGISTRATION_CITY_CODE",    application.REGISTRATION_CITY_CODE,    isMandatory || canSkipPersonalDetails);
            PrepareParameters(changes, parameters, "REGISTRATION_STREET",       application.REGISTRATION_STREET,       isMandatory || canSkipPersonalDetails);
            PrepareParameters(changes, parameters, "REGISTRATION_BUILDNUM",     application.REGISTRATION_BUILDNUM,     isMandatory || canSkipPersonalDetails);
            PrepareParameters(changes, parameters, "REGISTRATION_APARTMENT",    application.REGISTRATION_APARTMENT);
            PrepareParameters(changes, parameters, "CURRENT_COUNTRY_CODE",      application.CURRENT_COUNTRY_CODE,      isMandatory || canSkipPersonalDetails);
            PrepareParameters(changes, parameters, "CURRENT_STATE_CODE",        application.CURRENT_STATE_CODE,        isMandatory || canSkipPersonalDetails);
            PrepareParameters(changes, parameters, "CURRENT_CITY_CODE",         application.CURRENT_CITY_CODE,         isMandatory || canSkipPersonalDetails);
            PrepareParameters(changes, parameters, "CURRENT_STREET",            application.CURRENT_STREET,            isMandatory || canSkipPersonalDetails);
            PrepareParameters(changes, parameters, "CURRENT_BUILDNUM",          application.CURRENT_BUILDNUM,          isMandatory || canSkipPersonalDetails);
            PrepareParameters(changes, parameters, "CURRENT_APARTMENT",         application.CURRENT_APARTMENT);

            if (initial.LOAN_TYPE_STATE == "INSTALLATION_LOAN") // Installation Loan
            {
                this.DeleteApplicationProducts(id);
                this.SaveApplicationProducts(id, application.Products);
                if (application.SHOP_CODE == null && isShopUser && currentUserID.HasValue)
                {
                    application.SHOP_CODE = this.GetShopCode(currentUserID.Value);
                }

                PrepareParameters(changes, parameters, "SHOP_CODE",              application.SHOP_CODE, isMandatory && isShopUser);
                PrepareParameters(changes, parameters, "PRODUCT_NUMBER",         application.PRODUCT_NUMBER);
                PrepareParameters(changes, parameters, "GOODS_RECEIVING_CODE",   application.GOODS_RECEIVING_CODE);
                PrepareParameters(changes, parameters, "GOODS_DELIVERY_ADDRESS", application.GOODS_DELIVERY_ADDRESS);
                PrepareParameters(changes, parameters, "MOBILE_PHONE_2",         application.MOBILE_PHONE_2);
                PrepareParameters(changes, parameters, "SHOP_USER_ID",           currentUserID);
                PrepareParameters(changes, parameters, "COMMUNICATION_TYPE_CODE",application.COMMUNICATION_TYPE_CODE, isMandatory);
            }
            else // General Loan
            {
                PrepareParameters(changes, parameters, "MOBILE_PHONE_1",          application.MOBILE_PHONE_1, isMandatory);
                PrepareParameters(changes, parameters, "COMPANY_NAME",            application.COMPANY_NAME);
                PrepareParameters(changes, parameters, "COMPANY_PHONE",           application.COMPANY_PHONE);
                PrepareParameters(changes, parameters, "POSITION",                application.POSITION);
                PrepareParameters(changes, parameters, "WORKING_EXPERIENCE_CODE", application.WORKING_EXPERIENCE_CODE);
                PrepareParameters(changes, parameters, "MONTHLY_INCOME_CODE",     application.MONTHLY_INCOME_CODE);
                PrepareParameters(changes, parameters, "FAMILY_STATUS_CODE",      application.FAMILY_STATUS_CODE,      isMandatory || canSkipPersonalDetails);
            }

            bool isLoanOverdraft = IsLoanOverdraft(initial.LOAN_TYPE_ID);
            if (isLoanOverdraft)
            {
                PrepareParameters(changes, parameters, "OVERDRAFT_TEMPLATE_CODE", application.OVERDRAFT_TEMPLATE_CODE, isMandatory);
            }
            else
            {
                PrepareParameters(changes, parameters, "LOAN_TEMPLATE_CODE", application.LOAN_TEMPLATE_CODE);
            }

            parameters.Add("OPERATION_DETAILS", GenerateOperationDetailsString(changes));
            parameters.Add("IS_SUBMIT", application.SUBMIT);
            await ExecuteAsync(parameters, "Common.sp_SaveCompletedApplication");
        }

        public async Task<MainApplication> GetMainApplication(Guid id, int customerUserID, bool isShopUser)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("ID", id);
            MainApplication application = await GetSingleAsync<MainApplication>(parameters, "Common.sp_GetMainApplication");

            // if the customer is AmeriaBank client and their mobile phone or email are missing in AS system,
            // that data should be taken from APPLICATION_USER table which is populated once the customer
            // has registered to the system.
            if (!isShopUser && application != null &&
                (string.IsNullOrEmpty(application.MOBILE_PHONE_1) || string.IsNullOrEmpty(application.EMAIL)))
            {
                DynamicParameters dynamicParams = new DynamicParameters();
                dynamicParams.Add("CUSTOMER_USER_ID", customerUserID);
                CustomerMissingData customerMissingData = await GetSingleAsync<CustomerMissingData>(dynamicParams, "GL.sp_GetCustomerMissingData");

                application.MOBILE_PHONE_1 = customerMissingData.MOBILE_PHONE_1;
                application.EMAIL = customerMissingData.EMAIL;
            }

            if (application != null)
            {
                this.ApplyMappingSingle(application);
                if (application.LOAN_TYPE_STATE == "INSTALLATION_LOAN")
                {
                    application.Products = await GetApplicationProducts(id);
                }
            }

            return application;
        }

        public async Task SubmitMainApplication(Guid id)
        {
            string statusState = await GetApplicationStatusState(id);
            if (statusState == "PRE_APPROVAL_SUCCESS")
            {
                await this.ChangeApplicationStatus(id, 10);
            }
        }

        public async Task<IEnumerable<ScoringResults>> GetInstallationTemplateResults(string shopCode,
                                                                                      string productCategoryCode)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("SHOP_CODE", shopCode);
            parameters.Add("PRODUCT_CATEGORY_CODE", productCategoryCode);
            return await GetListAsync<ScoringResults>(parameters, "IL.sp_GetApplicationTemplateResult");
        }

        public async Task<ScoringResults> GetInstallationApplicationScoringResult(Guid id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("APPLICATION_ID", id);
            return await GetSingleAsync<ScoringResults>(parameters, "IL.sp_GetApplicationScoringResult");
        }

        public async Task<IEnumerable<ScoringResults>> GetGeneralApplicationScoringResult(Guid id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("APPLICATION_ID", id);
            return await GetListAsync<ScoringResults>(parameters, "GL.sp_GetApplicationScoringResultByType");
        }

        #endregion

        #region Agreed Application

        /// <summary>
        /// Create agreed application after the customer has been authenticated
        /// </summary>
        public async Task CreateAgreedApplication(Guid applicationID, AgreedApplication application)
        {
            Dictionary<string, string> changes = new Dictionary<string, string>();
            DynamicParameters parameters = new DynamicParameters();
            if (application.SUBMIT && !application.AGREED_WITH_TERMS)
            {
                throw new ApplicationException("ERR-0036", "Պայմաններին և կանոններին Ձեր համաձայնությունը պարտադիր է։");
            }

            PrepareParameters(changes, parameters, "APPLICATION_ID", applicationID, application.SUBMIT);
            bool isCardAccount = IsCardAccount(application.LOAN_TYPE_ID);
            if (isCardAccount)
            {
                if (application.IS_NEW_CARD)
                {
                    PrepareParameters(changes, parameters, "IS_NEW_CARD",           application.IS_NEW_CARD,           application.SUBMIT);
                    PrepareParameters(changes, parameters, "CREDIT_CARD_TYPE_CODE", application.CREDIT_CARD_TYPE_CODE, application.SUBMIT);
                    PrepareParameters(changes, parameters, "IS_CARD_DELIVERY",      application.IS_CARD_DELIVERY,      application.SUBMIT);
                    if (application.IS_CARD_DELIVERY)
                    {
                        PrepareParameters(changes, parameters, "CARD_DELIVERY_ADDRESS", application.CARD_DELIVERY_ADDRESS, application.SUBMIT);
                    }
                    else
                    {
                        PrepareParameters(changes, parameters, "BANK_BRANCH_CODE", application.BANK_BRANCH_CODE, application.SUBMIT);
                    }
                }
                else
                    PrepareParameters(changes, parameters, "EXISTING_CARD_CODE", application.EXISTING_CARD_CODE, application.SUBMIT);
            }

            PrepareParameters(changes, parameters, "IS_ARBITRAGE_CHECKED", application.IS_ARBITRAGE_CHECKED, application.SUBMIT);
            PrepareParameters(changes, parameters, "ACTUAL_INTEREST", application.ACTUAL_INTEREST, application.SUBMIT);
            parameters.Add("OPERATION_DETAILS", GenerateOperationDetailsString(changes));
            parameters.Add("IS_SUBMIT", application.SUBMIT);
            await ExecuteAsync(parameters, "GL.sp_SaveAgreedApplication");
        }

        public async Task<AgreedApplication> GetAgreedApplication(Guid id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("ID", id);
            AgreedApplication application = await GetSingleAsync<AgreedApplication>(parameters, "GL.sp_GetAgreedApplication");
            decimal.TryParse(GetSetting("AGREEMENT_LIMIT"), out decimal agreementLimit);
            if (application.CURRENCY_CODE != "AMD")
            {
                decimal cbRate = GetCBRate(application.CURRENCY_CODE, GetSetting("BANK_SERVER_DATABASE"));
                if (cbRate > 0)
                {
                    agreementLimit /= cbRate;
                }
            }
            application.IsAgreementNeeded = (application.FINAL_AMOUNT <= agreementLimit);
            ApplyMappingSingle(application);
            return application;
        }

        #endregion

        #region Common Operations

        public async Task DeleteApplication(Guid id)
        {
            Dictionary<string, string> changes = new Dictionary<string, string>();
            DynamicParameters parameters = new DynamicParameters();
            PrepareParameters(changes, parameters, "ID", id, true);
            parameters.Add("OPERATION_DETAILS", GenerateOperationDetailsString(changes));
            await ExecuteAsync(parameters, "Common.sp_DeleteApplication");
        }

        public async Task<IEnumerable<Application>> GetApplications(int id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("CUSTOMER_USER_ID", id);
            IEnumerable<Application> applications = await GetListAsync<Application>(parameters, "Common.sp_GetApplications");
            this.ApplyMappingList(applications);
            return applications;
        }

        public async Task<IEnumerable<ShopApplication>> GetManagerApplications(int id, DateTime fromDate, DateTime toDate, string name)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("SHOP_USER_ID", id);
            parameters.Add("FROM_DATE", fromDate);
            parameters.Add("TO_DATE", toDate);
            parameters.Add("NAME", name);
            IEnumerable<ShopApplication> applications = await GetListAsync<ShopApplication>(parameters, "IL.sp_GetManagerApplications");
            this.ApplyMappingList(applications);
            return applications;
        }

        public async Task<IEnumerable<ShopApplication>> GetOperatorApplications(int id, DateTime fromDate, DateTime toDate, string name)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("SHOP_USER_ID", id);
            parameters.Add("FROM_DATE", fromDate);
            parameters.Add("TO_DATE", toDate);
            parameters.Add("NAME", name);
            IEnumerable<ShopApplication> applications = await GetListAsync<ShopApplication>(parameters, "IL.sp_GetOperatorApplications");
            this.ApplyMappingList(applications);
            return applications;
        }

        public async Task CancelApplicationByCustomer(Guid id)
        {
            string statusState = await GetApplicationStatusState(id);
            if (statusState == "PENDING_PRE_APPROVAL" ||
                statusState == "PRE_APPROVAL_SUCCESS" ||
                statusState == "APPROVAL_SUCCESS" ||
                statusState == "AGREED")
            {
                await this.ChangeApplicationStatus(id, 16);
            }
        }

        public async Task RejectApplicationByShopUser(Guid id)
        {
            string statusState = await GetApplicationStatusState(id);
            if (statusState == "PENDING_PRE_APPROVAL" ||
                statusState == "PRE_APPROVAL_SUCCESS" ||
                statusState == "AGREED")
            {
                await this.ChangeApplicationStatus(id, 17);
            }
        }
        
        public async Task CompleteApplicationByShopUser(Guid id)
        {
            string statusState = await GetApplicationStatusState(id);
            if (statusState == "DELIVERING")
            {
                await this.ChangeApplicationStatus(id, 21);
            }
        }

        public string GetHeadShopCode(int currentUserID)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("ID", currentUserID);
            return GetScalarValue<string>(parameters, "select IL.f_GetShopUserHeadShopCode(@ID)");
        }

        public Shop GetShopUserHeadShop(int currentUserID)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("ID", currentUserID);
            return GetSingle<Shop>(parameters, "IL.sp_GetShopUserHeadShop");
        }

        public string GetShopCode(int currentUserID)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("ID", currentUserID);
            return GetSingle<string>(parameters, "IL.sp_GetShopCode");
        }

        private bool IsLoanOverdraft(string code)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("CODE", code);
            return GetSingle<bool>(parameters, "Common.sp_GetLoanOverdraft");
        }

        private bool IsCardAccount(string code)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("CODE", code);
            return GetSingle<bool>(parameters, "Common.sp_GetCardAccount");
        }

        #endregion

        #region Refinancing Loans

        public void UpdateRefinancingLoans(IEnumerable<RefinancingLoan> loans)
        {
            DynamicParameters parameters = new DynamicParameters();
            foreach (RefinancingLoan loan in loans)
            {
                parameters.Add("APPLICATION_ID", loan.APPLICATION_ID);
                parameters.Add("ROW_ID", loan.ROW_ID);
                parameters.Add("LOAN_CODE", loan.LOAN_CODE);
                Execute(parameters, "GL.sp_UpdateRefinancingLoans");
            }
        }

        public async Task<IEnumerable<RefinancingLoan>> GetRefinancingLoans(Guid applicationID)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("APPLICATION_ID", applicationID);
            IEnumerable<RefinancingLoan> loans = await GetListAsync<RefinancingLoan>(parameters, "GL.sp_GetRefinancingLoans");
            return loans;
        }

        #endregion

        #region Application Products

        private void SaveApplicationProducts(Guid applicationID, IEnumerable<ApplicationProduct> products)
        {
            DynamicParameters parameters = new DynamicParameters();
            foreach (ApplicationProduct product in products)
            {
                parameters.Add("APPLICATION_ID", applicationID);
                parameters.Add("DESCRIPTION", product.DESCRIPTION);
                parameters.Add("QUANTITY", product.QUANTITY);
                parameters.Add("PRICE", product.PRICE);
                parameters.Add("PRODUCT_CATEGORY_CODE", product.PRODUCT_CATEGORY_CODE);
                Execute(parameters, "IL.sp_SaveApplicationProduct");
            }
        }

        private void DeleteApplicationProducts(Guid applicationID)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("APPLICATION_ID", applicationID);
            Execute(parameters, "IL.sp_DeleteApplicationProducts");
        }

        private async Task<IEnumerable<ApplicationProduct>> GetApplicationProducts(Guid applicationID)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("APPLICATION_ID", applicationID);
            IEnumerable<ApplicationProduct> products = await GetListAsync<ApplicationProduct>(parameters, "IL.sp_GetApplicationProducts");
            return products;
        }

        #endregion

        #region Application Scan

        public void CreateApplicationScan(Guid applicationId, byte documentType, string fileName, byte[] content)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("APPLICATION_ID", applicationId);
            parameters.Add("APPLICATION_SCAN_TYPE_CODE", documentType);
            parameters.Add("FILE_NAME", fileName);
            parameters.Add("CONTENT", content);
            Execute(parameters, "Common.sp_CreateApplicationScan");
        }

        public void DeleteApplicationScan(Guid applicationId, string documentTypeCode)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("APPLICATION_ID", applicationId);
            parameters.Add("APPLICATION_SCAN_TYPE_CODE", documentTypeCode);
            Execute(parameters, "Common.sp_DeleteApplicationScan");
        }

        public void UpdateApplicationScan(Guid applicationId, string documentTypeCode, string fileName, byte[] content)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("APPLICATION_ID", applicationId);
            parameters.Add("APPLICATION_SCAN_TYPE_CODE", documentTypeCode);
            parameters.Add("CONTENTS", content);
            Execute(parameters, "Common.sp_UpdateApplicationScan");
        }

        public void SaveApplicationScan(Guid applicationId, string documentTypeCode, string fileName, byte[] content)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("APPLICATION_ID", applicationId);
            parameters.Add("APPLICATION_SCAN_TYPE_CODE", documentTypeCode);
            parameters.Add("FILE_NAME", fileName);
            parameters.Add("CONTENT", content);
            Execute(parameters, "Common.sp_SaveApplicationScan");
        }

        public IEnumerable<ApplicationScan> GetApplicationScan(Guid applicationId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("APPLICATION_ID", applicationId);
            IEnumerable<ApplicationScan> applicationScans = GetList<ApplicationScan>(parameters, "Common.sp_GetApplicationScan");
            return applicationScans;
        }

        public byte[] GetApplicationScanContent(Guid applicationId, string documentTypeCode)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("APPLICATION_ID", applicationId);
            parameters.Add("APPLICATION_SCAN_TYPE_CODE", documentTypeCode);
            ApplicationScanContent content = GetSingle<ApplicationScanContent>(parameters, "Common.sp_GetApplicationScanContent");
            byte[] result = null;
            if (content != null)
            {
                result = content.CONTENT;
            }
            return result;
        }

        #endregion

        #region Application Print

        public byte[] GetApplicationPrint(Guid applicationId, byte applicationPrintTypeId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("APPLICATION_ID", applicationId);
            parameters.Add("APPLICATION_PRINT_TYPE_ID", applicationPrintTypeId);
            return GetSingle<byte[]>(parameters, string.Format("{0}dbo.am0sp_GetApplicationPrint", GetSetting("BANK_SERVER_DATABASE")));
        }

        public PersonalSheet GetApplicationForPersonalSheet(Guid applicationId)
        {
            PersonalSheet sheet;
            DateTime currentDate = DateTime.Now;
            currentDate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day);

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("ID", applicationId);
            sheet = GetSingle<PersonalSheet>(parameters, "Common.sp_GetApplicationForPersonalSheet");
            if (sheet != null)
            {
                LoanParameters loanParameters = this.GetLoanParameters(sheet.LOAN_TYPE_ID);

                string bankDB = this.GetSetting("BANK_SERVER_DATABASE");

                string subsystemCode = loanParameters.IS_OVERDRAFT ? "C3" : "C1";

                if (sheet.REPAY_DAY == 0)
                {
                    sheet.REPAY_DAY = this.GetRepayDate(subsystemCode, sheet.TEMPLATE_CODE, currentDate, bankDB);
                }

                if (sheet.LOAN_TYPE_ID == "00")
                {
                    if (sheet.REPAY_DAY < loanParameters.REPAYMENT_DAY_FROM
                        || sheet.REPAY_DAY > loanParameters.REPAYMENT_DAY_TO)
                    {
                        sheet.REPAY_DAY = loanParameters.REPAYMENT_DAY_FROM;
                        sheet.REPAYMENT_BEGIN_DATE = new DateTime(currentDate.Year, currentDate.Month, sheet.REPAY_DAY).AddMonths(2);
                    }
                }
                else if (loanParameters.IS_REPAY_START_DAY)
                {
                    sheet.REPAYMENT_BEGIN_DATE = new DateTime(currentDate.Year, currentDate.Month, 1).AddMonths(1);
                }
                else
                {
                    int givenDay = currentDate.Day;
                    if ((givenDay < loanParameters.REPAYMENT_DAY_FROM || givenDay > loanParameters.REPAYMENT_DAY_TO)
                        && givenDay <= loanParameters.REPAY_TRANSITION_DAY)
                    {
                        sheet.REPAYMENT_BEGIN_DATE = new DateTime(currentDate.Year, currentDate.Month, sheet.REPAY_DAY).AddMonths(2);
                    }
                    else if (loanParameters.IS_REPAY_NEXT_MONTH)
                    {
                        sheet.REPAYMENT_BEGIN_DATE = new DateTime(currentDate.Year, currentDate.Month, sheet.REPAY_DAY).AddMonths(1);
                    }
                    else
                    {
                        sheet.REPAYMENT_BEGIN_DATE = currentDate;
                    }
                }

                if (currentDate < sheet.REPAYMENT_BEGIN_DATE && this.IsHoliday(sheet.REPAYMENT_BEGIN_DATE, bankDB))
                {
                    sheet.REPAYMENT_BEGIN_DATE = this.GetNextWorkDay(sheet.REPAYMENT_BEGIN_DATE, bankDB);
                }

                DateTime queryLimitDate;
                if (sheet.LOAN_DURATION == "0")
                {
                    sheet.REPAYMENT_END_DATE = new DateTime(2049, 12, 31);
                    queryLimitDate = sheet.REPAYMENT_END_DATE;
                }
                else
                {
                    sheet.REPAYMENT_END_DATE = this.GetPassHolidayDate(subsystemCode, sheet.TEMPLATE_CODE, currentDate.AddMonths(int.Parse(sheet.LOAN_DURATION)), bankDB);
                    queryLimitDate = new DateTime(2065, 1, 1);
                }

                DateTime? scheduleLastDate = this.GetLastRepaymentDate(subsystemCode, sheet.TEMPLATE_CODE, currentDate, sheet.REPAYMENT_BEGIN_DATE, queryLimitDate, int.Parse(sheet.LOAN_DURATION), sheet.FINAL_AMOUNT, sheet.LOAN_INTEREST, sheet.REPAY_DAY, bankDB);
                if (scheduleLastDate.HasValue)
                {
                    sheet.REPAYMENT_END_DATE = scheduleLastDate.Value;
                }

                decimal? actualInterest = this.GetActualInterest(subsystemCode, sheet.TEMPLATE_CODE, sheet.LOAN_TYPE_ID, currentDate, sheet.REPAYMENT_BEGIN_DATE, sheet.REPAYMENT_END_DATE, byte.Parse(sheet.LOAN_DURATION), sheet.FINAL_AMOUNT, sheet.CURRENCY_CODE, sheet.CREDIT_CARD_TYPE_CODE, sheet.LOAN_INTEREST, sheet.REPAY_DAY, bankDB);
                if (actualInterest.HasValue)
                {
                    sheet.LOAN_INTEREST_2 = actualInterest.Value;
                }

                sheet.MONTH_DURATION = 12 * (sheet.REPAYMENT_END_DATE.Year - currentDate.Year) + sheet.REPAYMENT_END_DATE.Month - currentDate.Month;

                int durationYear;
                if (sheet.LOAN_DURATION == "0")
                {
                    durationYear = 1;
                }
                else
                {
                    if (sheet.MONTH_DURATION % 12 == 0)
                        durationYear = sheet.MONTH_DURATION / 12;
                    else
                        durationYear = (sheet.MONTH_DURATION / 12) + 1;
                }

                if (sheet.IS_OVERDRAFT)
                {
                    sheet.TOTAL_INTEREST_AMOUNT = sheet.FINAL_AMOUNT * sheet.LOAN_INTEREST * durationYear / 100;
                }
                else
                {
                    decimal? interestAmount = this.GetInterestAmount(subsystemCode, sheet.TEMPLATE_CODE, currentDate, sheet.REPAYMENT_BEGIN_DATE, sheet.REPAYMENT_END_DATE, byte.Parse(sheet.LOAN_DURATION), sheet.FINAL_AMOUNT, sheet.LOAN_INTEREST, sheet.REPAY_DAY, bankDB);
                    if (interestAmount.HasValue)
                    {
                        sheet.TOTAL_INTEREST_AMOUNT = interestAmount.Value;
                    }
                }

                decimal cbRate = this.GetCBRate(sheet.CURRENCY_CODE, bankDB);
                sheet.OTHER_PAYMENTS_CASH_OUT_FEE *= cbRate;
                sheet.OTHER_PAYMENTS = sheet.OTHER_PAYMENTS_LOAN_SERVICE_FEE + (sheet.OTHER_PAYMENTS_CARD_SERVICE_FEE + sheet.OTHER_PAYMENTS_OTHER_FEE) * durationYear + sheet.OTHER_PAYMENTS_CASH_OUT_FEE;
                sheet.TOTAL_REPAYMENT = (sheet.FINAL_AMOUNT + sheet.TOTAL_INTEREST_AMOUNT) * cbRate + sheet.OTHER_PAYMENTS;
            }
            return sheet;
        }

        private byte GetRepayDate(string subsystemCode, string templateCode, DateTime date, string bankDB)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("SubsystemCode", subsystemCode);
            parameters.Add("TemplateCode", templateCode);
            parameters.Add("Date", date);
            parameters.Add("PeriodMonthFrom", dbType: DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("PeriodMonthTo", dbType: DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("Interest", dbType: DbType.Decimal, direction: ParameterDirection.Output);
            parameters.Add("RepayDay", dbType: DbType.Byte, direction: ParameterDirection.Output);
            Execute(parameters, string.Format("{0}dbo.am0sp_GetTemplateData", bankDB));
            return parameters.Get<byte>("RepayDay");
        }

        private LoanParameters GetLoanParameters(string loanType)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("LOAN_TYPE_CODE", loanType);
            return GetSingle<LoanParameters>(parameters, "Common.sp_GetLoanParameters");
        }

        private decimal? GetActualInterest(string subsystemCode,
                                           string templateCode,
                                           string loanType,
                                           DateTime dateAgreementFrom,
                                           DateTime dateFirstRepayment,
                                           DateTime dateAgreementTo,
                                           byte period,
                                           decimal amount,
                                           string currency,
                                           string cardType,
                                           decimal interest,
                                           byte repayDay,
                                           string bankDB)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("TERM_MONTH", period);
            parameters.Add("AGREEMENT_FROM", dateAgreementFrom);
            parameters.Add("FIRST_REPAYMENT", dateFirstRepayment);
            parameters.Add("AGREEMENT_TO", dateAgreementTo);
            parameters.Add("TEMPLATE_CODE", templateCode);
            parameters.Add("SUBSYSTEM_CODE", subsystemCode);
            parameters.Add("AMOUNT", amount);
            parameters.Add("CURRENCY", currency);
            parameters.Add("CARD_TYPE", cardType);
            parameters.Add("INTEREST", interest);
            parameters.Add("REPAY_DAY", repayDay);
            parameters.Add("LOAN_TYPE", loanType);
            return GetSingle<decimal?>(parameters, string.Format("{0}dbo.am0sp_GetActualInterest", bankDB));
        }

        private decimal? GetInterestAmount(string subsystemCode,
                                           string templateCode,
                                           DateTime dateAgreementFrom,
                                           DateTime dateFirstRepayment,
                                           DateTime dateAgreementTo,
                                           byte period,
                                           decimal amount,
                                           decimal interest,
                                           byte repayDay,
                                           string bankDB)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("TERM_MONTH", period);
            parameters.Add("AGREEMENT_FROM", dateAgreementFrom);
            parameters.Add("FIRST_REPAYMENT", dateFirstRepayment);
            parameters.Add("AGREEMENT_TO", dateAgreementTo);
            parameters.Add("TEMPLATE_CODE", templateCode);
            parameters.Add("SUBSYSTEM_CODE", subsystemCode);
            parameters.Add("AMOUNT", amount);
            parameters.Add("INTEREST", interest);
            parameters.Add("REPAY_DAY", repayDay);
            return GetSingle<decimal?>(parameters, string.Format("{0}dbo.am0sp_GetInterestAmount", bankDB));
        }

        private bool IsHoliday(DateTime date, string bankDB)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Date", date);
            return GetSingle<bool>(parameters, string.Format("{0}dbo.am0sp_IsHoliday", bankDB));
        }

        private DateTime GetPassHolidayDate(string subsystemCode, string templateCode, DateTime date, string bankDB)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("SubsystemCode", subsystemCode);
            parameters.Add("TemplateCode", templateCode);
            parameters.Add("Date", date);
            return GetSingle<DateTime>(parameters, string.Format("{0}dbo.am0sp_GetPassHolidayDate", bankDB));
        }

        private DateTime? GetLastRepaymentDate(string subsystemCode,
                                               string templateCode,
                                               DateTime dateAgreementFrom,
                                               DateTime dateFirstRepayment,
                                               DateTime dateAgreementTo,
                                               int loanDuration,
                                               decimal amount,
                                               decimal interest,
                                               byte repayDay,
                                               string bankDB)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("SubsystemCode", subsystemCode);
            parameters.Add("TemplateCode", templateCode);
            parameters.Add("DateAgreementFrom", dateAgreementFrom);
            parameters.Add("DateFirstRepayment", dateFirstRepayment);
            parameters.Add("DateAgreementTo", dateAgreementTo);
            parameters.Add("LoanDuration", loanDuration);
            parameters.Add("Amount", amount);
            parameters.Add("Interest", interest);
            parameters.Add("RepayDay", repayDay);
            return GetSingle<DateTime?>(parameters, string.Format("{0}dbo.am0sp_GetLastRepaymentDate", bankDB));
        }

        public DateTime GetNextWorkDay(DateTime date, string bankDB)
        {
            DateTime result = date.AddDays(1);
            if (IsHoliday(result, bankDB))
                result = GetNextWorkDay(result, bankDB);
            return result;
        }

        private decimal GetCBRate(string currency, string bankDB)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("CURRENCY", currency);
            return GetSingle<decimal>(parameters, string.Format("{0}dbo.am0sp_GetCBRate", bankDB));
        }

        #endregion

        #region Credit Card Authorization

        public async Task<ClientDataForCardValidation> GetClientDataForCardValidation(Guid id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("APPLICATION_ID", id);
            return await GetSingleAsync<ClientDataForCardValidation>(parameters, "Common.sp_GetClientDataForCardValidation");
        }

        public async Task<ClientCard> GetClientCardData(string clientCode, string cardNumber, DateTime expiryDate)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("CLICODE", clientCode);
            parameters.Add("CARDNUM", cardNumber);
            parameters.Add("EXPIRY", expiryDate);
            return await GetSingleAsync<ClientCard>(parameters, string.Format("{0}dbo.am0sp_GetClientCardData", GetSetting("BANK_SERVER_DATABASE")));
        }

        public async Task<Client> GetClientData(string clientCode)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("CLICODE", clientCode);
            return await GetSingleAsync<Client>(parameters, string.Format("{0}dbo.am0sp_GetClientData", GetSetting("BANK_SERVER_DATABASE")));
        }

        /// <summary>
        /// Returns list of active client cards
        /// </summary>
        public async Task<IEnumerable<ActiveClientCard>> GetActiveClientCards(string clientCode, string loanType, string currency)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("CLICODE", clientCode);
            parameters.Add("LOANTYPE", loanType);
            parameters.Add("CURRENCY", currency);
            return await GetListAsync<ActiveClientCard>(parameters, string.Format("{0}dbo.am0sp_GetActiveClientCards", GetSetting("BANK_SERVER_DATABASE")));
        }

        /// <summary>
        /// Returns list of credit card types
        /// </summary>
        public async Task<IEnumerable<DirectoryEntity>> GetCreditCardTypes(string languageCode, string loanType, string currencyCode)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("LANGUAGE_CODE", languageCode);
            parameters.Add("LOAN_TYPE_ID", loanType);
            parameters.Add("CURRENCY_CODE", currencyCode);
            return await GetListAsync<DirectoryEntity>(parameters, "GL.sp_GetCreditCardTypes");
        }

        /// <summary>
        /// If already exists smsHash for the given application ID,
        /// deletes it and inserts the new smsHash.
        /// </summary>
        private async Task SaveCreditCardAuthorization(Guid applicationID, string smsHash)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("APPLICATION_ID", applicationID);
            parameters.Add("SMS_HASH", smsHash);
            await ExecuteAsync(parameters, "GL.sp_SaveCreditCardAuthorization");
        }

        /// <summary>
        /// Returns application ID, smsHash, smsSentDate and tryCount
        /// for the given application ID
        /// </summary>
        private async Task<CreditCardAuthorization> GetCreditCardAuthorization(Guid applicationID)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("APPLICATION_ID", applicationID);
            CreditCardAuthorization authorization = await GetSingleAsync<CreditCardAuthorization>(parameters, "GL.sp_GetCreditCardAuthorization");
            return authorization;
        }

        /// <summary>
        /// Sets a new Credit Card Authorization tryCount for the given application ID.
        /// </summary>
        private async Task SetTryCreditCardAuthorization(Guid applicationID, int tryCount)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("APPLICATION_ID", applicationID);
            parameters.Add("TRY_COUNT", tryCount);
            await ExecuteAsync(parameters, "GL.sp_SetTryCreditCardAuthorization");
        }

        /// <summary>
        /// If exists, deletes the record for the given application ID
        /// </summary>
        private async Task DeleteCreditCardAuthorization(Guid applicationID)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("APPLICATION_ID", applicationID);
            await ExecuteAsync(parameters, "GL.sp_DeleteCreditCardAuthorization");
        }

        public async Task SubmitAuthorizedApplication(Guid id)
        {
            string statusState = await GetApplicationStatusState(id);
            if (statusState == "PHONE_VERIFICATION_PENDING")
            {
                await this.ChangeApplicationStatus(id, 13);
            }
        }

        public async Task SubmitNotAuthorizedApplication(Guid id)
        {
            string statusState = await GetApplicationStatusState(id);
            if (statusState == "PRE_APPROVAL_SUCCESS" ||
                statusState == "PHONE_VERIFICATION_PENDING")
            {
                await this.ChangeApplicationStatus(id, 12);
            }
        }

        public async Task SubmitCardAuthorizedApplication(Guid id)
        {
            string statusState = await GetApplicationStatusState(id);
            if (statusState == "PENDING_APPROVAL")
            {
                await this.ChangeApplicationStatus(id, 19);
            }
        }

        /// <summary>
        /// Checks whether the correct smsCode is inserted
        /// by the applicant with the given application ID 
        /// </summary>
        public async Task CheckCreditCardAuthorization(Guid applicationID, string smsCode)
        {
            CreditCardAuthorization authorization = await GetCreditCardAuthorization(applicationID);
            if (authorization != null)
            {
                if (authorization.SMS_HASH == Crypto.HashString(smsCode)) // correct sms code inserted
                {
                    int authTerm = int.Parse(GetSetting("CREDIT_CARD_AUTH_TERM"));
                    if (GetServerDate() <= authorization.SMS_SENT_DATE.AddSeconds(authTerm))
                    {
                        // sms code authentication succeeded, delete the code
                        await DeleteCreditCardAuthorization(applicationID);
                    }
                    else
                    {
                        throw new ApplicationException("ERR-0005", "SMS-ի ակտիվ ժամկետը սպառվել է");
                    }
                }
                else // wrong sms code inserted
                {
                    int tryCount = int.Parse(GetSetting("CREDIT_CARD_TRY_COUNT"));
                    authorization.TRY_COUNT++;
                    if (authorization.TRY_COUNT < tryCount) // still attempt is left
                    {
                        await SetTryCreditCardAuthorization(applicationID, authorization.TRY_COUNT);
                        throw new ApplicationException("ERR-0007", "Մուտքագրված նույնականացման կոդը սխալ է: Կարող եք ստանալ նոր նույնականացման կոդ:");
                    }
                    else // no attempt is left
                    {
                        await DeleteCreditCardAuthorization(applicationID);
                        await SubmitNotAuthorizedApplication(applicationID);
                        throw new ApplicationException("ERR-0006", "Հնարավոր փորձերի քանակը սպառվեց։ Անհրաժեշտ է գնալ բանկ։");
                    }
                }
            }
        }

        /// <summary>
        /// Generates a sms code and sends it to the applicant with the given ID
        /// in order to validate their credit card
        /// </summary>
        public async Task<string> GenerateSMSCode(Guid applicationID)
        {
            CreditCardAuthorization authorization = await GetCreditCardAuthorization(applicationID);
            if (authorization != null)
            {
                int smsCount = int.Parse(GetSetting("CREDIT_CARD_SMS_COUNT"));
                if (authorization.SMS_COUNT >= smsCount) // SMS count limit is reached
                {
                    await DeleteCreditCardAuthorization(applicationID);
                    await SubmitNotAuthorizedApplication(applicationID);
                    throw new ApplicationException("ERR-0008", "SMS ուղարկելու քանակը սպառվեց։ Անհրաժեշտ է գնալ բանկ։");
                }
            }
            string smsCode = GetAuthorizationCode();
            await SaveCreditCardAuthorization(applicationID, Crypto.HashString(smsCode));
            return smsCode;
        }

        /// <summary>
        /// Log application 3rd step.
        /// </summary>
        public async Task LogSMSCreditCardStep(Guid applicationID, string cardNumber, string mobilePhone) // here
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("APPLICATION_ID", applicationID);
            parameters.Add("CARD_NUMBER", cardNumber);
            parameters.Add("MOBILE_PHONE", mobilePhone);
            await ExecuteAsync(parameters, "GL.sp_LogSMSCreditCardStep");
        }

        #endregion

        public async Task<ApplicationInformation> GetApplicationInformation(Guid applicationID)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("APPLICATION_ID", applicationID);
            ApplicationInformation application = await GetSingleAsync<ApplicationInformation>(parameters, "GL.sp_GetApplicationInformation");
            this.ApplyMappingSingle(application);
            return application;
        }

        /// <summary>
        /// Apply mapping for a single Application entity
        /// </summary>
        private void ApplyMappingSingle(Application application)
        {
            if (application != null)
            {
                application.STATUS_STATE = this.MapApplicationStatus(application.STATUS_ID);
                application.LOAN_TYPE_STATE = this.MapLoanTypeStatus(application.LOAN_TYPE_ID);
            }
        }

        /// <summary>
        /// Apply mapping for a list of Application entities 
        /// </summary>
        private void ApplyMappingList(IEnumerable<Application> applications)
        {
            foreach(Application application in applications)
            {
                this.ApplyMappingSingle(application);
            }
        }

        /// <summary>
        /// Changes application status
        /// </summary>
        private async Task ChangeApplicationStatus(Guid id, byte status)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("APPLICATION_ID", id);
            parameters.Add("STATUS", status);
            await ExecuteAsync(parameters, "Common.sp_ChangeApplicationStatus");
        }

        private async Task<string> GetApplicationStatusState(Guid id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("ID", id);
            int statusID = await GetSingleAsync<int>(parameters, "Common.sp_GetApplicationStatusID");
            string statusState = this.MapApplicationStatus(statusID);
            return statusState;
        }

        public async Task<ApplicationCountSetting> GetApplicationCountSetting(string socialCardNumber, Guid? applicationID)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("SOCIAL_CARD_NUMBER", socialCardNumber);
            parameters.Add("APPLICATION_ID", applicationID);
            return await GetSingleAsync<ApplicationCountSetting>(parameters, "GL.sp_GetApplicationCountSetting");
        }

        public async Task<bool> DoesClientWorksAtBank(string socialCardNumber)
        {
            string bankDB = GetSetting("BANK_SERVER_DATABASE");
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("SocialCardCode", socialCardNumber);
            return await GetSingleAsync<bool>(parameters, string.Format("{0}dbo.am0sp_DoesClientWorksAtBank", bankDB));
        }

        public bool IsClientStudent(string socialCardNumber)
        {
            string bankDB = GetSetting("BANK_SERVER_DATABASE");
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("SocialCardCode", socialCardNumber);
            return GetSingle<bool>(parameters, string.Format("{0}dbo.am0sp_IsClientStudent", bankDB));
        }

        public async Task<IEnumerable<ScoringResults>> GetInstallationTemplatesForPartner(string partnerCode,
                                                                                          string productCategoryCode)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("PARTNER_COMPANY_CODE", partnerCode);
            parameters.Add("PRODUCT_CATEGORY_CODE", productCategoryCode);
            return await GetListAsync<ScoringResults>(parameters, "IL.sp_GetInstallationTemplatesForPartner");
        }

        #region Mobile Phone Authorization

        /// <summary>
        /// If already exists smsHash for the given application ID,
        /// deletes it and inserts the new smsHash.
        /// </summary>
        private async Task SaveMobilePhoneAuthorization(Guid applicationID, string smsHash)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("APPLICATION_ID", applicationID);
            parameters.Add("SMS_HASH", smsHash);
            await ExecuteAsync(parameters, "IL.sp_SaveMobilePhoneAuthorization");
        }

        /// <summary>
        /// Returns application ID, smsHash, smsSentDate and tryCount
        /// for the given application ID
        /// </summary>
        private async Task<MobilePhoneAuthorization> GetMobilePhoneAuthorization(Guid applicationID)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("APPLICATION_ID", applicationID);
            MobilePhoneAuthorization authorization = await GetSingleAsync<MobilePhoneAuthorization>(parameters, "IL.sp_GetMobilePhoneAuthorization");
            return authorization;
        }

        /// <summary>
        /// Sets a new Mobile Phone Authorization tryCount for the given application ID.
        /// </summary>
        private async Task SetTryMobilePhoneAuthorization(Guid applicationID, int tryCount)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("APPLICATION_ID", applicationID);
            parameters.Add("SMS_COUNT", tryCount);
            await ExecuteAsync(parameters, "IL.sp_SetTryMobilePhoneAuthorization");
        }

        /// <summary>
        /// If exists, deletes the record for the given application ID
        /// </summary>
        private async Task DeleteMobilePhoneAuthorization(Guid applicationID)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("APPLICATION_ID", applicationID);
            await ExecuteAsync(parameters, "IL.sp_DeleteMobilePhoneAuthorization");
        }

        /// <summary>
        /// Generates a sms code and sends it to the applicant with the given ID
        /// in order to validate their mobile phone
        /// </summary>
        public async Task<string> GenerateMobilePhoneAuthorizationCode(Guid applicationID)
        {
            MobilePhoneAuthorization authorization = await GetMobilePhoneAuthorization(applicationID);
            if (authorization != null)
            {
                int smsCount = int.Parse(GetSetting("MOBILE_PHONE_SMS_COUNT"));
                if (authorization.SMS_COUNT >= smsCount) // SMS count limit is reached
                {
                    throw new ApplicationException("ERR-0008", "SMS ուղարկելու քանակը սպառվել է");
                }
            }
            string smsCode = GetAuthorizationCode();
            await SaveMobilePhoneAuthorization(applicationID, Crypto.HashString(smsCode));
            return smsCode;
        }

        /// <summary>
        /// Checks whether the correct smsCode is inserted
        /// by the applicant with the given application ID 
        /// </summary>
        public async Task CheckMobilePhoneAuthorization(Guid applicationID, string smsCode)
        {
            MobilePhoneAuthorization authorization = await GetMobilePhoneAuthorization(applicationID);
            if (authorization != null)
            {
                if (authorization.SMS_HASH == Crypto.HashString(smsCode)) // correct sms code inserted
                {
                    int authTerm = int.Parse(GetSetting("MOBILE_PHONE_AUTH_TERM"));
                    if (GetServerDate() <= authorization.SMS_SENT_DATE.AddSeconds(authTerm))
                    {
                        // sms code authentication succeeded, delete the code
                        await DeleteMobilePhoneAuthorization(applicationID);
                    }
                    else
                    {
                        throw new ApplicationException("ERR-0005", "SMS-ի ակտիվ ժամկետը սպառվել է");
                    }
                }
                else // wrong sms code inserted
                {
                    int tryCount = int.Parse(GetSetting("MOBILE_PHONE_SMS_COUNT"));
                    authorization.SMS_COUNT++;
                    if (authorization.SMS_COUNT < tryCount) // still attempt is left
                    {
                        await SetTryMobilePhoneAuthorization(applicationID, authorization.SMS_COUNT);
                        throw new ApplicationException("ERR-0007", "Մուտքագրված SMS կոդը սխալ է: Կարող եք ստանալ նոր SMS կոդ:");
                    }
                    else // no attempt is left
                    {
                        throw new ApplicationException("ERR-0008", "SMS ուղարկելու քանակը սպառվել է");
                    }
                }
            }
            else
                throw new ApplicationException("ERR-0007", "Մուտքագրված SMS կոդը սխալ է: Կարող եք ստանալ նոր SMS կոդ:");
        }

        #endregion

        public ADriveFile GetADriveFile(Guid applicationId)
        {
            string adriveDB = GetSetting("EVENTSTORE_SERVER_DATABASE");
            if (string.IsNullOrWhiteSpace(adriveDB))
                return null;
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("APPLICATION_ID", applicationId);
            return GetSingle<ADriveFile>(parameters, string.Format("{0}dbo.olsp_GetApplicationPrint", adriveDB));
        }

        public decimal GetMaxApprovedAmount(Guid applicationId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("APPLICATION_ID", applicationId);
            return GetSingle<decimal>(parameters, "Common.sp_GetMaxApprovedAmount");
        }
    }
}
