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
    [Route("/Applications/{id}/Agreed")]
    public class LoanAgreedApplicationController : RepositoryControllerBase<ApplicationRepository>
    {
        public LoanAgreedApplicationController(IConfigurationRoot configuration)
            : base(configuration, (connectionString) => new ApplicationRepository(connectionString))
        {
        }

        /// <summary>
        /// Implements GET /Applications/{id}/Agreed
        /// Returns agreed application with the given id
        /// </summary>
        [HttpGet]
        public async Task<AgreedApplication> Get(Guid id)
        {
            AgreedApplication application = await Repository.GetAgreedApplication(id);
            return application;
        }

        /// <summary>
        /// Implements POST /Applications/{id}/Agreed
        /// Creates main application with the given id
        /// </summary>
        [HttpPost]
        public async Task Post(Guid id, [FromBody]AgreedApplication application)
        {
            await Repository.CreateAgreedApplication(id, application);
        }

        [HttpGet("personalSheet")]
        public PersonalSheet GetApplicationForPersonalSheet(Guid id)
        {
            return this.Repository.GetApplicationForPersonalSheet(id);
        }
    }
}
