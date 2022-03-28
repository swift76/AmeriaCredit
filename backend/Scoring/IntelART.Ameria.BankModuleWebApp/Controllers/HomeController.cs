using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using IntelART.Ameria.BankModuleWebApp.Models;
using Microsoft.Extensions.Configuration;

namespace IntelART.Ameria.BankModuleWebApp.Controllers
{
    [Authorize(Roles = "BankUser,BankPowerUser")]
    public class HomeController : Controller
    {
        private string identityServerUrl;

        public HomeController(IConfigurationRoot configuration)
        {
            this.identityServerUrl = configuration.GetSection("Authentication")["Authority"];
        }

        public async Task<IActionResult> Index()
        {
            return View(this.CreateViewModel<ViewModelBase>());
        }

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

        private T CreateViewModel<T>()
            where T : ViewModelBase, new()
        {
            T result = new T();

            ClaimsPrincipal principal = HttpContext.User;

            if (principal != null)
            {
                result.CurrentUser = new CurrentUserModel();
                result.CurrentUser.Username = principal.Identity.Name;
                result.CurrentUser.Roles = principal.FindAll("role").Select(claim => claim.Value);
            }

            return result;
        }
    }
}
