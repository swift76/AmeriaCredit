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
    /// loan limits available as a result of the successful scoring
    /// </summary>
    ////[Authorize(Roles ="ShopUser,ShopPowerUser")]
    [Authorize]
    [Route("[controller]")]
    public class LoanLimitsController : RepositoryControllerBase<ApplicationParameterRepository>
    {
        public LoanLimitsController(IConfigurationRoot configuration)
            : base(configuration, (connectionString) => new ApplicationParameterRepository(connectionString))
        {
        }

        /// <summary>
        /// Implements GET /LoanLimits?loanTypeCode={loanTypeCode}&currency={currency}
        /// Returns lower and upper loan limits for the given loan type and currency.
        /// </summary>
        [HttpGet]
        public async Task<LoanLimits> Get([FromQuery]string loanTypeCode, [FromQuery]string currency)
        {
            LoanLimits loanLimits = await Repository.GetLoanLimits(loanTypeCode, currency);
            return loanLimits;
        }
    }
}
