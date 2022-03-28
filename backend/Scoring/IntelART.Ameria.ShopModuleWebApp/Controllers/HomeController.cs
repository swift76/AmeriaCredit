using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;

namespace IntelART.Ameria.BankModuleWebApp.Controllers
{
    [Authorize(Roles = "ShopUser,ShopPowerUser")]
    public class HomeController : Controller
    {
        private string identityServerUrl;

        public HomeController(IConfigurationRoot configuration)
        {
            this.identityServerUrl = configuration.GetSection("Authentication")["Authority"];
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        // [Route("/NewApplication")]
        // public async Task<IActionResult> NewApplication([FromQuery] string applicationId)
        // {
        //     return View((object)applicationId);
        // }

        [Route("/ChangePassword")]
        public async Task<IActionResult> ChangePassword()
        {
            // TODO: Tigran: This needs to be moved out to the configuration.
            // Would be better to have an AccountService which will expose methods
            // like ResetPassword, ChangePassword, ActivateAccount, DeactivateAccount,
            // and others related to the account management.
            // The service then can be configured in the Startup class.
            return Redirect(string.Format("{0}Account/ChangePassword", this.identityServerUrl));
        }

        [Route("/Logout")]
        public IActionResult Logout()
        {
            return new SignOutResult(new[] { "oidc", "Cookies" });
        }
    }
}
