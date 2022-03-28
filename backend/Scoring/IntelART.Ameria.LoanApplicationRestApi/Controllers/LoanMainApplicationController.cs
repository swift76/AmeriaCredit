using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using IntelART.Ameria.Entities;
using IntelART.Ameria.Repositories;
using System.Threading.Tasks;

namespace IntelART.Ameria.LoanApplicationRestApi.Controllers
{
    /// <summary>
    /// Controller class to implement the API methods required for viewing, creating, and 
    /// managing loan applications
    /// </summary>
    ////[Authorize(Roles ="ShopUser,ShopPowerUser")]
    [Authorize]
    [Route("/Applications/{id}/Main")]
    public class LoanMainApplicationController : RepositoryControllerBase<ApplicationRepository>
    {
        public LoanMainApplicationController(IConfigurationRoot configuration)
            : base(configuration, (connectionString) => new ApplicationRepository(connectionString))
        {
        }

        /// <summary>
        /// Implements GET /Applications/{id}/Main
        /// Returns main application with the given id
        /// </summary>
        [HttpGet]
        public async Task<MainApplication> Get(Guid id)
        {
            MainApplication application = await Repository.GetMainApplication(id, this.CurrentUserID, this.IsShopUser);
            return application;
        }

        /// <summary>
        /// Implements POST /Applications/{id}/Main
        /// Creates main application with the given id
        /// </summary>
        [HttpPost]
        public async Task Post(Guid id, [FromBody]MainApplication application)
        {
            int? currentUserID = null;
            bool isShopUser;
            InitialApplication initial = await Repository.GetInitialApplication(id);
            if (string.IsNullOrEmpty(initial.PARTNER_COMPANY_CODE))
            {
                currentUserID = this.CurrentUserID;
                isShopUser = this.IsShopUser;
            }
            else
                isShopUser = false;
            await Repository.CreateMainApplication(id, application, initial, currentUserID, isShopUser);
        }
    }
}
