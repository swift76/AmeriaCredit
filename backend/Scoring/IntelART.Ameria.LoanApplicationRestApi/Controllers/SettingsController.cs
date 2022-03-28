using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using IntelART.Ameria.Repositories;

namespace IntelART.Ameria.LoanApplicationRestApi.Controllers
{
    /// <summary>
    /// Controller class to implement the API methods required for getting the system settings
    /// </summary>
    [Authorize]
    [Route("/Settings")]
    public class SettingsController : RepositoryControllerBase<ApplicationRepository>
    {
        public SettingsController(IConfigurationRoot configuration)
            : base(configuration, (connectionString) => new ApplicationRepository(connectionString))
        {
        }

        [HttpGet("RedirectToOnboarding")]
        public bool GetRedirectToOnboarding()
        {
            bool result = Repository.GetSetting("REDIRECT_TO_ONBOARDING") == "1";
            return result;
        }
    }
}
