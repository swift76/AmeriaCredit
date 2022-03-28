using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using IntelART.Ameria.Entities;
using IntelART.Ameria.Repositories;

namespace IntelART.Ameria.LoanApplicationRestApi.Controllers
{
    /// <summary>
    /// Controller class to implement the API methods required for getting 
    /// the loan repayment schedule based on the loan amount, interest,
    /// and the loan duration
    /// </summary>
    ////[Authorize(Roles ="ShopUser,ShopPowerUser")]
    [Authorize]
    [Route("[controller]")]
    public class RepaymentScheduleController : RepositoryControllerBase<ApplicationParameterRepository>
    {
        public RepaymentScheduleController(IConfigurationRoot configuration)
            : base(configuration, (connectionString) => new ApplicationParameterRepository(connectionString))
        {
        }

        /// <summary>
        /// Implements GET /RepaymentSchedule?interest={interest}&amount={amount}&duration={duration}&serviceInterest={serviceInterest}serviceAmount={serviceAmount}
        /// Returns the loan repayment schedule based on the loan amount, the interest rate,
        /// and the loan duration in months
        /// </summary>
        [HttpGet]
        public async Task<RepaymentSchedule> Get([FromQuery]decimal interest, [FromQuery]decimal amount, [FromQuery]byte duration, [FromQuery]decimal serviceInterest, [FromQuery]decimal serviceAmount)
        {
            RepaymentSchedule repaymentSchedule = await Repository.GetLoanMonthlyPaymentAmount(interest, amount, duration, serviceInterest, serviceAmount);
            return repaymentSchedule;
        }
    }
}
