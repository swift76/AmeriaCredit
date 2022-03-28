using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using IntelART.Ameria.Entities;
using IntelART.Ameria.Repositories;
using System.Linq;

namespace IntelART.Ameria.LoanApplicationRestApi.Controllers
{
    /// <summary>
    /// Controller class to implement the API methods required for retrieving
    /// different lookup sets
    /// </summary>
    ////[Authorize(Roles ="ShopUser,ShopPowerUser")]
    [Authorize]
    [Route("/Directory")]
    public class DirectoryController : RepositoryControllerBase<DirectoryRepository>
    {
        public DirectoryController(IConfigurationRoot Configuration)
            : base(Configuration, (connectionString)=>new DirectoryRepository(connectionString))
        {
        }

        /// <summary>
        /// Gets the list of countries
        /// </summary>
        [HttpGet("Countries")]
        public async Task<IEnumerable<DirectoryEntity>> GetCountries()
        {
            IEnumerable<DirectoryEntity> countries = await Repository.GetCountries(this.languageCode);
            return countries;
        }

        /// <summary>
        /// Gets the list of countries
        /// </summary>
        [HttpGet("AddressCountries")]
        public async Task<IEnumerable<DirectoryEntity>> GetAddressCountries()
        {
            IEnumerable<DirectoryEntity> countries = await Repository.GetAddressCountries(this.languageCode);
            return countries;
        }

        /// <summary>
        /// Gets the list of states
        /// </summary>
        [HttpGet("States")]
        public async Task<IEnumerable<DirectoryEntity>> GetStates()
        {
            IEnumerable<DirectoryEntity> states = await Repository.GetStates(this.languageCode);
            return states;
        }

        /// <summary>
        /// Gets the list of cities for a given state
        /// </summary>
        /// <param name="stateCode">The code of the state</param>
        /// <returns></returns>
        [HttpGet("States/{stateCode}/Cities")]
        public async Task<IEnumerable<DirectoryEntity>> GetCitiesForState(string stateCode)
        {
            IEnumerable<DirectoryEntity> cities = await Repository.GetCities(this.languageCode, stateCode);
            return cities;
        }

        /// <summary>
        /// Gets the list of possible employer industries
        /// </summary>
        [HttpGet("Industries")]
        public async Task<IEnumerable<DirectoryEntity>> GetIndustries()
        {
            IEnumerable<DirectoryEntity> industries = await Repository.GetOrganizationActivities(this.languageCode);
            return industries;
        }

        /// <summary>
        /// Gets the list of possible values for the working experience durations
        /// </summary>
        [HttpGet("WorkExperienceDurations")]
        public async Task<IEnumerable<DirectoryEntity>> GetWorkExperienceDurations()
        {
            IEnumerable<DirectoryEntity> workingExperienceDurations = await Repository.GetWorkingExperiences(this.languageCode);
            return workingExperienceDurations;
        }

        /// <summary>
        /// Gets the list of possible values for the monthly net income ranges
        /// </summary>
        [HttpGet("MonthlyNetIncomeRanges")]
        public async Task<IEnumerable<DirectoryEntity>> GetMonthlyNetIncomeRanges()
        {
            IEnumerable<DirectoryEntity> monthlyNetIncomeRanges = await Repository.GetMonthlyNetSalaries(this.languageCode);
            return monthlyNetIncomeRanges;
        }

        /// <summary>
        /// Gets the list of possible marital statuses
        /// </summary>
        [HttpGet("MaritalStatuses")]
        public async Task<IEnumerable<DirectoryEntity>> GetMaritalStatuses()
        {
            IEnumerable<DirectoryEntity> maritalStatuses = await Repository.GetFamilyStatuses(this.languageCode);
            return maritalStatuses;
        }

        /// <summary>
        /// Gets the list of possible document types
        /// </summary>
        [HttpGet("IdDocumentTypes")]
        public async Task<IEnumerable<DirectoryEntity>> GetIdDocumentTypes()
        {
            IEnumerable<DirectoryEntity> documentTypes = await Repository.GetDocumentTypes(this.languageCode);
            return documentTypes;
        }

        /// <summary>
        /// Gets the list of possible loan types
        /// </summary>
        [HttpGet("LoanTypes/{isStudent}")]
        public async Task<IEnumerable<LoanType>> GetLoanTypes(bool isStudent)
        {
            IEnumerable<LoanType> loanTypes = await Repository.GetLoanTypes(this.languageCode, isStudent);
            return loanTypes;
        }

        /// <summary>
        /// Gets the list of possible currency types
        /// </summary>
        [HttpGet("Currencies/{loanType}")]
        public async Task<IEnumerable<DirectoryEntity>> GetCurrencies(string loanType)
        {
            IEnumerable<DirectoryEntity> currencies = Enumerable.Empty<DirectoryEntity>();
            if (loanType != "null")
            {
                currencies = await Repository.GetLoanCurrencies(loanType, this.languageCode);
            }
            return currencies;
        }

        /// <summary>
        /// Gets the list of possible communication types
        /// </summary>
        [HttpGet("CommunicationTypes")]
        public async Task<IEnumerable<DirectoryEntity>> GetCommunicationTypes()
        {
            IEnumerable<DirectoryEntity> communicationTypes = await Repository.GetCommunicationTypes(this.languageCode);
            return communicationTypes;
        }

        /// <summary>
        /// Gets the list of possible product categories
        /// </summary>
        [HttpGet("ProductCategories")]
        public async Task<IEnumerable<DirectoryEntity>> GetProductCategories()
        {
            IEnumerable<DirectoryEntity> productCategories = await Repository.GetProductCategories(this.languageCode, this.CurrentUserID);
            return productCategories;
        }

        /// <summary>
        /// Gets the list of possible options to receive the products
        /// </summary>
        [HttpGet("ProductReceivingOptions")]
        public async Task<IEnumerable<DirectoryEntity>> GetProductReceivingOptions()
        {
            IEnumerable<DirectoryEntity> productReceivingOptions = await Repository.GetGoodsReceivingOptions(this.languageCode);
            return productReceivingOptions;
        }

        /// <summary>
        /// Implements GET /LoanTerms?isOverdraft={isOverdraft}&termFrom={termFrom}&termTo={termTo}
        /// Gets the list of loan terms
        /// </summary>
        [HttpGet("LoanTerms/{isOverdraft}/{termFrom}/{termTo}")]
        public IEnumerable<DirectoryEntity> GetLoanTerms(bool isOverdraft, int termFrom, int termTo)
        {
            IEnumerable<DirectoryEntity> loanTerms = Repository.GetLoanTerms(this.languageCode, isOverdraft, termFrom, termTo);
            return loanTerms;
        }

        /// <summary>
        /// Gets the list of possible options to receive branches of the bank
        /// </summary>
        [HttpGet("BankBranches")]
        public async Task<IEnumerable<DirectoryEntity>> GetBankBranches()
        {
            IEnumerable<DirectoryEntity> bankBranches = await Repository.GetBankBranches(this.languageCode);
            return bankBranches;
        }

        /// <summary>
        /// Gets the list of possible product categories for partner companies
        /// </summary>
        [HttpGet("ProductCategoriesForPartner/{partnerCode}")]
        public async Task<IEnumerable<DirectoryEntity>> GetProductCategoriesForPartner(string partnerCode)
        {
            IEnumerable<DirectoryEntity> productCategories = await Repository.GetProductCategoriesForPartner(this.languageCode, partnerCode);
            return productCategories;
        }

        /// <summary>
        /// Gets the list of universities
        /// </summary>
        [HttpGet("Universities")]
        public async Task<IEnumerable<DirectoryEntity>> GetUniversities()
        {
            IEnumerable<DirectoryEntity> universities = await Repository.GetUniversities(this.languageCode);
            return universities;
        }
    }
}
