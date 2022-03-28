using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authorization;
using IntelART.Ameria.ShopModuleWebApp.Models;
using System.Net.Http;
using Newtonsoft.Json;
using IntelART.Utilities;

namespace IntelART.Ameria.ShopModuleWebApp.Controllers
{
    public class AccountController : Controller
    {
        private string identityServerUrl;

        public AccountController(IConfigurationRoot configuration)
        {
            this.identityServerUrl = configuration.GetSection("Authentication")["Authority"];
        }

        [HttpGet]
        public async Task<IActionResult> Login([FromQuery] string returnUrl)
        {
            LoginModel model = new LoginModel();
            model.ReturnUrl = returnUrl;
            return View(model);
        }

        [HttpPost]
        ////[ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([FromForm] LoginModel model)
        {
            IActionResult result;

            var disco = await DiscoveryClient.GetAsync(this.identityServerUrl);
            if (disco.IsError)
            {
                throw new Exception(disco.Error);
            }

            var client = new TokenClient(
                disco.TokenEndpoint,
                "shopApplication2",
                "secret");

            string username = model.Username;
            string password = model.Password;

            TokenResponse tokenResponse = await client.RequestResourceOwnerPasswordAsync(username, password, "openid profile loanApplicationApi offline_access");

            if (!tokenResponse.IsError
                && tokenResponse.AccessToken != null)
            {
                JwtSecurityToken token = new JwtSecurityToken(tokenResponse.AccessToken);
                AuthenticationProperties props = new AuthenticationProperties();
                props.Items[".Token.access_token"] = tokenResponse.AccessToken;
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(new ClaimsIdentity(token.Claims, CookieAuthenticationDefaults.AuthenticationScheme)),
                    props);

                // if (string.IsNullOrEmpty(model.ReturnUrl))
                // {
                //     result = RedirectToAction("", "");
                // }
                // else
                // {
                //     result = Redirect(model.ReturnUrl);
                // }
                return Ok(tokenResponse.Json);
            }
            else
            {
                return BadRequest(tokenResponse.Json);
            }
            return result;
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok(true);
        }

        [Authorize]
        [HttpGet()]
        public async Task<IActionResult> ChangePassword()
        {
            if (HttpContext.User != null
                && HttpContext.User.Identity != null
                && HttpContext.User.Identity.IsAuthenticated)
            {
                string returnUrl = HttpContext.Request.Headers["referer"];
                ChangePasswordResponse result = new ChangePasswordResponse()
                { 
                    UserName = HttpContext.User.Identity.Name,
                    ReturnUrl = returnUrl
                };
                return Ok(result);
            }
            else
            {
                return Unauthorized();
            }
        }

        [Authorize]
        [HttpPost()]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            IActionResult result;

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(string.Format("{0}://{1}", this.Request.Scheme, this.Request.Host));

            AuthenticateResult info = await this.HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            string accessToken;
            if (info != null
                && info.Properties != null
                && info.Properties.Items != null
                && info.Properties.Items.TryGetValue(".Token.access_token", out accessToken))
            {
                client.SetBearerToken(accessToken);
            }
            string sss = JsonConvert.SerializeObject(new { oldPassword = model.OldPassword, newPassword = model.NewPassword, newPasswordConfirmation = model.ConfirmNewPassword }, Formatting.None);
            HttpResponseMessage response = await client.PostAsync("/api/loan/Account/Password/", new StringContent(sss, System.Text.Encoding.UTF8));

            if (response.IsSuccessStatusCode)
            {
                // TODO: Tigran: Should logout here. Also, the redirect URL should be the logout
                // endpoint of the clinet application, so that the user will be logged out on the
                // client application scope as well.
                result = Redirect(model.ReturnUrl);
            }
            else
            {
                bool isUnknownError = true;
                if (response.Content != null)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    ErrorInfo exception = JsonConvert.DeserializeObject<ErrorInfo>(content);
                    if (exception != null)
                    {
                        isUnknownError = false;
                        ModelState.AddModelError("", exception.Message);
                    }
                }
                if (isUnknownError)
                {
                    ModelState.AddModelError("", "Համակարգային սխալ գաղտնաբառի փոփոխման ժամանակ");
                }
                result = View(model);
            }

            return result;
        }
    }

    public class ChangePasswordResponse
    {
        public string UserName { get; set; }
        public string ReturnUrl { get; set; }
    }
}
