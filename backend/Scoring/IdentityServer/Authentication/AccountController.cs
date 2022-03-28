using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IntelART.Communication;
using IntelART.IdentityManagement;

namespace IntelART.IdentityServer.Authentication
{
    public class AccountController : Controller
    {
        private IEmailSender emailSender;
        private IMembershipProvider membershipProvider;

        public AccountController(IEmailSender emailSender, IMembershipProvider membershipProvider)
        {
            this.emailSender = emailSender;
            this.membershipProvider = membershipProvider;
        }

        [HttpPost]
        ////[Authorize]
        public async Task<IActionResult> ResetPassword(string username, string returnUrl)
        {
            if (!string.IsNullOrWhiteSpace(username))
            {
                UserInfo user = this.membershipProvider.GetUserByUsername(username);
                if (user == null)
                {
                    throw new ApplicationException("ERR-0002", string.Format("Unknown user '{0}'.", username));
                }
                if (string.IsNullOrWhiteSpace(user.Email))
                {
                    throw new ApplicationException("ERR-0003", string.Format("User '{0}' does not have a valid email.", username));
                }

                string token = Guid.NewGuid().ToString("N");
                await this.membershipProvider.ChangeUserPassword(user.Id.ToString(), "", token);

                string baseUrl = string.Format("{0}://{1}{2}", HttpContext.Request.Scheme, HttpContext.Request.Host, Url.Action("CreatePassword", "Account"));
                string url = string.Format("{0}?username={1}&token={2}&returnUrl={3}", baseUrl, username, token, returnUrl);

                string bodyMessage = string.Format("Սեղմեք ստորև բերված հղման վրա՝ գրանցումն ավարտելու համար։<br /><a href=\"{0}\">{0}</a>", url);

                await this.emailSender.SendAsync(new EmailAddress(user.FullName, user.Email), "Գրանցում", bodyMessage);
            }
            return NoContent();
        }

        [HttpGet()]
        public async Task<IActionResult> CreatePassword(string returnUrl, string username, string token)
        {
            CreatePasswordModel model = new CreatePasswordModel()
            {
                ReturnUrl = returnUrl,
                Username = username,
                PasswordChangeRequestToken = token
            };
            return View(model);
        }

        [HttpPost()]
        public async Task<IActionResult> CreatePassword(CreatePasswordModel model)
        {
            if (string.IsNullOrWhiteSpace(model.NewPassword)
                || model.NewPassword != model.ConfirmNewPassword)
            {
                return View(model);
            }
            if (this.membershipProvider.ValidatePassword(model.Username, model.PasswordChangeRequestToken))
            {
                UserInfo user = this.membershipProvider.GetUserByUsername(model.Username);
                if (user == null)
                {
                    throw new ApplicationException("ERR-0002", string.Format("Unknown user '{0}'.", model.Username));
                }
                await this.membershipProvider.ChangeUserPassword(user.Id.ToString(), model.PasswordChangeRequestToken, model.NewPassword);
            }
            return Redirect(model.ReturnUrl);
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
                ChangePasswordModel model = new ChangePasswordModel();
                model.Username = HttpContext.User.Identity.Name;
                if (!string.IsNullOrWhiteSpace(returnUrl))
                {
                    model.ReturnUrl = returnUrl;
                }
                return View(model);
            }
            else
            {
                return Unauthorized();
            }
        }

        private class ChangePasswordResult
        {
            public bool IsSuccess { get; private set; }
            public string Message { get; private set; }

            public ChangePasswordResult(bool isSuccess, string message)
            {
                this.IsSuccess = isSuccess;
                this.Message = message;
            }
        }

        private async Task<ChangePasswordResult> ChangePasswordImpl(ChangePasswordModel model)
        {
            bool success = false;
            string message = null;

            Claim subjectClaim = HttpContext.User.FindFirst("sub");
            if (subjectClaim != null)
            {
                string userId = subjectClaim.Value;
                if (model != null
                    && model.NewPassword == model.ConfirmNewPassword)
                {
                    if (this.membershipProvider.ValidatePassword(HttpContext.User.Identity.Name, model.OldPassword))
                    {
                        try
                        {
                            await this.membershipProvider.ChangeUserPassword(userId, model.OldPassword, model.NewPassword);
                            success = true;
                        }
                        catch (Exception e)
                        {
                            // TODO: Tigran: Log exception message here
                            message = "Համակարգային սխալ գաղտնաբառը փոխելիս";
                        }
                    }
                    else
                    {
                        message = "Ընթացիկ գաղտնաբառը սխալ է մուտքագրված";
                    }
                }
                else
                {
                    message = "Նոր գաղտնաբառը և նոր գաղտնաբառի հաստատումը իրար չեն համապատասխանում";
                }
            }
            else
            {
                message = "Անհնար է որոշել ընթացիկ օգտագործողին";
            }

            return new ChangePasswordResult(success, message);
        }

        [Authorize]
        [HttpPost()]
        public async Task<IActionResult> ChangePasswordRpc(ChangePasswordModel model)
        {
            IActionResult result;

            ChangePasswordResult r = await this.ChangePasswordImpl(model);

            if(r.IsSuccess)
            {
                result = Ok();
            }
            else
            {
                result = BadRequest(r.Message);
            }

            return result;
        }

        [Authorize]
        [HttpPost()]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            IActionResult result;

            ChangePasswordResult r = await this.ChangePasswordImpl(model);

            if (r.IsSuccess)
            {
                // TODO: Tigran: Should logout here. Also, the redirect URL should be the logout
                // endpoint of the clinet application, so that the user will be logged out on the
                // client application scope as well.
                result = Redirect(model.ReturnUrl);
            }
            else
            {
                if (r.Message != null)
                {
                    ModelState.AddModelError("", r.Message);
                }
                model.Username = HttpContext.User.Identity.Name;
                result = View(model);
            }

            return result;
        }
    }
}
