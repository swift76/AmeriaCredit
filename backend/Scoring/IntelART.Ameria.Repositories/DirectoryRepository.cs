using Dapper;
using System.Collections.Generic;
using IntelART.Ameria.Entities;
using System.Threading.Tasks;

namespace IntelART.Ameria.Repositories
{
    public class DirectoryRepository : BaseRepository
    {
        public DirectoryRepository(string connectionString) : base(connectionString)
        {
        }

        /// <summary>
        /// Returns list of passport types
        /// </summary>
        public async Task<IEnumerable<DirectoryEntity>> GetDocumentTypes(string languageCode)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("LANGUAGE_CODE", languageCode);
            return await GetListAsync<DirectoryEntity>(parameters, "Common.sp_GetDocumentTypes");
        }

        /// <summary>
        /// Returns list of countries
        /// </summary>
        public async Task<IEnumerable<DirectoryEntity>> GetCountries(string languageCode)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("LANGUAGE_CODE", languageCode);
            return await GetListAsync<DirectoryEntity>(parameters, "Common.sp_GetCountries");
        }

        /// <summary>
        /// Returns list of loan terms
        /// </summary>
        public IEnumerable<DirectoryEntity> GetLoanTerms(string language, bool isOverdraft, int termFrom, int termTo)
        {
            List<DirectoryEntity> loanTerms = new List<DirectoryEntity>();
            int termInterval = 6;
            for (int i = termFrom; i <= termTo; i += termInterval)
            {
                loanTerms.Add(new DirectoryEntity { CODE = i.ToString(), NAME = string.Format("{0} ամիս", i.ToString()) });
            }

            if (isOverdraft) // in case of overdraft, the loan can be termless
            {
                loanTerms.Add(new DirectoryEntity { CODE = "0", NAME = "Անժամկետ" });
            }
            return loanTerms;
        }

        /// <summary>
        /// Returns list of states
        /// </summary>
        public async Task<IEnumerable<DirectoryEntity>> GetStates(string languageCode)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("LANGUAGE_CODE", languageCode);
            return await GetListAsync<DirectoryEntity>(parameters, "Common.sp_GetStates");
        }

        /// <summary>
        /// Returns list of states
        /// </summary>
        public async Task<IEnumerable<DirectoryEntity>> GetCities(string languageCode, string stateCode)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("LANGUAGE_CODE", languageCode);
            parameters.Add("STATE_CODE", stateCode);
            return await GetListAsync<DirectoryEntity>(parameters, "Common.sp_GetCities");
        }

        /// <summary>
        /// Returns list of communication types
        /// </summary>
        public async Task<IEnumerable<DirectoryEntity>> GetCommunicationTypes(string languageCode)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("LANGUAGE_CODE", languageCode);
            return await GetListAsync<DirectoryEntity>(parameters, "Common.sp_GetCommunicationTypes");
        }

        /// <summary>
        /// Returns list of organization activities
        /// </summary>
        public async Task<IEnumerable<DirectoryEntity>> GetOrganizationActivities(string languageCode)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("LANGUAGE_CODE", languageCode);
            return await GetListAsync<DirectoryEntity>(parameters, "Common.sp_GetOrganizationActivities");
        }

        /// <summary>
        /// Returns list of monthly net salaries
        /// </summary>
        public async Task<IEnumerable<DirectoryEntity>> GetMonthlyNetSalaries(string languageCode)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("LANGUAGE_CODE", languageCode);
            return await GetListAsync<DirectoryEntity>(parameters, "Common.sp_GetMonthlyNetSalaries");
        }

        /// <summary>
        /// Returns list of loan types
        /// </summary>
        public async Task<IEnumerable<LoanType>> GetLoanTypes(string languageCode, bool isStudent)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("LANGUAGE_CODE", languageCode);
            parameters.Add("IS_STUDENT", isStudent);
            IEnumerable<LoanType> loanTypes = await GetListAsync<LoanType>(parameters, "Common.sp_GetLoanTypes");
            foreach (LoanType loanType in loanTypes)
            {
                loanType.STATE = this.MapLoanTypeStatus(loanType.CODE);
            }
            return loanTypes;
        }

        /// <summary>
        /// Returns list of working experiences
        /// </summary>
        public async Task<IEnumerable<DirectoryEntity>> GetWorkingExperiences(string languageCode)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("LANGUAGE_CODE", languageCode);
            return await GetListAsync<DirectoryEntity>(parameters, "GL.sp_GetWorkingExperiences");
        }

        /// <summary>
        /// Returns list of family statuses
        /// </summary>
        public async Task<IEnumerable<DirectoryEntity>> GetFamilyStatuses(string languageCode)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("LANGUAGE_CODE", languageCode);
            return await GetListAsync<DirectoryEntity>(parameters, "GL.sp_GetFamilyStatuses");
        }

        /// <summary>
        /// Returns list of loan currencies
        /// </summary>
        public async Task<IEnumerable<DirectoryEntity>> GetLoanCurrencies(string loanType, string languageCode)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("LOAN_TYPE_CODE", loanType);
            parameters.Add("LANGUAGE_CODE", languageCode);
            return await GetListAsync<DirectoryEntity>(parameters, "Common.sp_GetLoanCurrencies");
        }

        /// <summary>
        /// Returns list of goods receiving options
        /// </summary>
        public async Task<IEnumerable<DirectoryEntity>> GetGoodsReceivingOptions(string languageCode)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("LANGUAGE_CODE", languageCode);
            return await GetListAsync<DirectoryEntity>(parameters, "IL.sp_GetGoodsReceivingOptions");
        }

        /// <summary>
        /// Returns list of product categories
        /// </summary>
        public async Task<IEnumerable<DirectoryEntity>> GetProductCategories(string languageCode, int shopUserID)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("LANGUAGE_CODE", languageCode);
            parameters.Add("SHOP_USER_ID", shopUserID);
            return await GetListAsync<DirectoryEntity>(parameters, "IL.sp_GetProductCategories");
        }

        /// <summary>
        /// Returns list of branches of the bank
        /// </summary>
        public async Task<IEnumerable<DirectoryEntity>> GetBankBranches(string languageCode)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("LANGUAGE_CODE", languageCode);
            return await GetListAsync<DirectoryEntity>(parameters, "GL.sp_GetBankBranches");
        }

        /// <summary>
        /// Returns list of countries
        /// </summary>
        public async Task<IEnumerable<DirectoryEntity>> GetAddressCountries(string languageCode)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("LANGUAGE_CODE", languageCode);
            return await GetListAsync<DirectoryEntity>(parameters, "Common.sp_GetAddressCountries");
        }

        /// <summary>
        /// Returns list of product categories for partner companies
        /// </summary>
        public async Task<IEnumerable<DirectoryEntity>> GetProductCategoriesForPartner(string languageCode, string partnerCode)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("LANGUAGE_CODE", languageCode);
            parameters.Add("PARTNER_COMPANY_CODE", partnerCode);
            return await GetListAsync<DirectoryEntity>(parameters, "IL.sp_GetProductCategoriesForPartner");
        }

        /// <summary>
        /// Returns list of universities
        /// </summary>
        public async Task<IEnumerable<DirectoryEntity>> GetUniversities(string languageCode)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("LANGUAGE_CODE", languageCode);
            return await GetListAsync<DirectoryEntity>(parameters, "Common.sp_GetUniversities");
        }
    }
}
