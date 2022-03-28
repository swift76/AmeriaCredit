using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Mvc;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using IntelART.IdentityManagement;

namespace IntelART.IdentityServer.Authentication
{
    [SecurityHeaders]
    public class AuthenticationController : Controller
    {
        private readonly IUserStore userStore;
        private readonly IIdentityServerInteractionService interactionService;
        private readonly IClientStore clientStore;

        public AuthenticationController(
            IIdentityServerInteractionService interaction,
            IClientStore clientStore,
            IHttpContextAccessor httpContextAccessor,
            IEventService events,
            IUserStore users)
        {
            this.userStore = users;
            this.interactionService = interaction;
            this.clientStore = clientStore;
        }

        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl)
        {
            AuthorizationRequest request = await this.interactionService.GetAuthorizationContextAsync(returnUrl);
            Client client = await this.clientStore.FindClientByIdAsync(request.ClientId);

            LoginModel model = new LoginModel()
            {
                ReturnUrl = returnUrl,
                RememberLogin = false,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                if (this.userStore.ValidatePassword(model.Username, model.Password))
                {
                    AuthenticationProperties props = null;

                    if (model.RememberLogin)
                    {
                        props = new AuthenticationProperties
                        {
                            IsPersistent = true,
                            ExpiresUtc = DateTimeOffset.UtcNow.Add(TimeSpan.FromDays(30))
                        };
                    };

                    // issue authentication cookie with subject ID and username
                    UserInfo user = this.userStore.GetUserByUsername(model.Username);
                    await HttpContext.Authentication.SignInAsync(user.Id.ToString(), user.Username, props);

                    // make sure the returnUrl is still valid, and if yes - redirect back to authorize endpoint or a local page
                    if (this.interactionService.IsValidReturnUrl(model.ReturnUrl) || Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }

                    return Redirect("~/");
                }

                ModelState.AddModelError("", "Նման տվյալներով օգտատեր առկա չէ։ Խնդրում ենք փորձել նորից կամ գրանցել օգտատեր։");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Logout(string logoutId)
        {
            LogoutRequest request = await this.interactionService.GetLogoutContextAsync(logoutId);

            LogoutModel model = new LogoutModel() { PostLogoutRedirectUri = request.PostLogoutRedirectUri };

            if (model.PostLogoutRedirectUri == null)
            {
                model.PostLogoutRedirectUri = HttpContext.Request.Headers["referer"];
            }

            await HttpContext.Authentication.SignOutAsync();

            return View(model);
        }
    }
}
