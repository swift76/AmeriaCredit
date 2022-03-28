using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using IntelART.Ameria.Entities;
using IntelART.Ameria.Repositories;

namespace IntelART.Ameria.LoanApplicationRestApi.Controllers
{
    /// <summary>
    /// Controller class to implement the API methods required for getting the  
    /// loan options available as a result of the successful scoring
    /// </summary>
    ////[Authorize(Roles ="ShopUser,ShopPowerUser")]
    [Authorize]
    [Route("/Applications")]
    public class LoanApplicationScoringResultsController : RepositoryControllerBase<ApplicationRepository>
    {
        public LoanApplicationScoringResultsController(IConfigurationRoot configuration)
            : base(configuration, (connectionString) => new ApplicationRepository(connectionString))
        {
        }

        /// <summary>
        /// Implements GET /Applications/{id}/InstallationScoringResult
        /// Returns scoring results for the main installation loan application with the given id
        /// </summary>
        [HttpGet("{id}/InstallationScoringResult")]
        public async Task<ScoringResults> GetInstallationScoringResult(Guid id)
        {
            ScoringResults results = await Repository.GetInstallationApplicationScoringResult(id);
            return results;
        }

        /// <summary>
        /// Implements GET /Applications/InstallationTemplateResults?productCategoryCode={productCategoryCode}
        /// Returns installation template data for the main application with the product category
        /// </summary>
        [HttpGet("InstallationTemplateResults")]
        public async Task<IEnumerable<ScoringResults>> Get([FromQuery]string productCategoryCode)
        {
            string shopCode = null;
            if (this.IsShopUser)
            {
                shopCode = Repository.GetHeadShopCode(this.CurrentUserID);
            }

            IEnumerable<ScoringResults> results = await Repository.GetInstallationTemplateResults(shopCode, productCategoryCode);
            return results;
        }

        /// <summary>
        /// Implements GET /Applications/{id}/GeneralScoringResults
        /// Returns scoring results for the main general loan application with the given id
        /// </summary>
        [HttpGet("{id}/GeneralScoringResults")]
        public async Task<IEnumerable<ScoringResults>> Get(Guid id)
        {
            IEnumerable<ScoringResults> results = await Repository.GetGeneralApplicationScoringResult(id);
            return results;
        }

        /// <summary>
        /// Implements GET /Applications/InstallationTemplatesForPartner?partnerCode={partnerCode}&productCategoryCode={productCategoryCode}
        /// Returns installation template data for the main application of partner with the product category
        /// </summary>
        [HttpGet("InstallationTemplatesForPartner")]
        public async Task<IEnumerable<ScoringResults>> Get([FromQuery]string partnerCode, [FromQuery]string productCategoryCode)
        {
            IEnumerable<ScoringResults> results = await Repository.GetInstallationTemplatesForPartner(partnerCode, productCategoryCode);
            return results;
        }
    }
}
