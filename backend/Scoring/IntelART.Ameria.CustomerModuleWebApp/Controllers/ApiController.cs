using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace IntelART.Ameria.CustomerModuleWebApp.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class ApiController : Controller
    {
        [HttpGet]
        public async Task<string> Values()
        {
            AuthenticateInfo info = await HttpContext.Authentication.GetAuthenticateInfoAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            string accessToken = info.Properties.Items["access_token"];
            HttpClient client = new HttpClient();
            client.SetBearerToken(accessToken);
            HttpResponseMessage response = await client.GetAsync("http://localhost:5005/partners");

            HttpContext.Response.StatusCode = (int)response.StatusCode;

            return response.ToString();
        }
    }
}
