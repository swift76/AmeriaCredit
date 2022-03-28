using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using IntelART.Ameria.Repositories;

namespace IntelART.Ameria.LoanApplicationRestApi.Controllers
{
    /// <summary>
    /// Controller class to implement the API methods required
    /// to complete the loan applications by a shop-user
    /// </summary>
    [Authorize(Roles ="ShopUser,ShopPowerUser")]
    [Route("/Applications/Completed")]
    public class LoanApplicationCompletionController : RepositoryControllerBase<ApplicationRepository>
    {
        public LoanApplicationCompletionController(IConfigurationRoot configuration)
            : base(configuration, (connectionString) => new ApplicationRepository(connectionString))
        {
        }

        /// <summary>
        /// Implements PUT /Applications/Completed/{id}
        /// Completes the application with the given id by a shop-user
        /// </summary>
        [HttpPut("{id}")]
        public async Task Put(Guid id)
        {
            await Repository.CompleteApplicationByShopUser(id);
        }
    }
}
