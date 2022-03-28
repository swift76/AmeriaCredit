using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using IntelART.Ameria.Entities;
using IntelART.Ameria.Repositories;
using System.Collections.Generic;

namespace IntelART.Ameria.LoanApplicationRestApi.Controllers
{
    /// <summary>
    /// Controller class to implement the API methods required for getting the  
    /// parent shops for installation loan applications
    /// </summary>
    ////[Authorize(Roles ="ShopUser,ShopPowerUser")]
    [Authorize]
    [Route("[controller]")]
    public class ParentShopsController : RepositoryControllerBase<ApplicationParameterRepository>
    {
        public ParentShopsController(IConfigurationRoot configuration)
            : base(configuration, (connectionString) => new ApplicationParameterRepository(connectionString))
        {
        }

        /// <summary>
        /// Implements GET /ParentShops
        /// Returns parent shopss for installation loan application.
        /// </summary>
        [HttpGet]
        public async Task<IEnumerable<Shop>> Get()
        {
            IEnumerable<Shop> parentShops = await Repository.GetParentShops();
            return parentShops;
        }
    }
}
