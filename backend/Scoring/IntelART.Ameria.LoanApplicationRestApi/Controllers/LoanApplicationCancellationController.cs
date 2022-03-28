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
    /// to cancel the loan applications by the customers
    /// </summary>
    [Authorize(Roles = "Customer")]
    [Route("/Applications/Cancelled")]
    public class LoanApplicationCancellationController : RepositoryControllerBase<ApplicationRepository>
    {
        public LoanApplicationCancellationController(IConfigurationRoot configuration)
            : base(configuration, (connectionString) => new ApplicationRepository(connectionString))
        {
        }

        /// <summary>
        /// Implements PUT /Applications/Cancelled/{id}
        /// Cancels the application with the given id by a customer
        /// </summary>
        [HttpPut("{id}")]
        public async Task Put(Guid id)
        {
            await Repository.CancelApplicationByCustomer(id);
        }
    }
}
