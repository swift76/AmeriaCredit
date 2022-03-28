using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace IntelART.Ameria.BankModuleWebApp.Controllers
{
    [Authorize]
    public class MainController : Controller
    {
        public async Task<IActionResult> Index()
        {
            // Just as an example make a call to the BankRestApi
            // Need to include the access token in each request
            //string accessToken = await HttpContext.Authentication.GetTokenAsync("access_token");
            //HttpClient client = new HttpClient();
            //client.SetBearerToken(accessToken);
            //HttpResponseMessage response = await client.GetAsync("http://localhost:5005/partners");

            //string str = response.ToString();

            return View();
        }
    }
}
