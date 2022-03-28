using IntelART.IdentityManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IntelART.Ameria.LoanApplicationRestApi.Controllers
{
    [Route("[controller]")]
    ////[Authorize(Roles = "BankUser,BankPowerUser")]
    public class AccountController : ControllerBase
    {
        private IMembershipProvider membershipProvider;

        public AccountController(IMembershipProvider membershipProvider)
        {
            this.membershipProvider = membershipProvider;
        }

        private async Task ChangePasswordImpl(string oldPassword, string newPassword, string confirmNewPassword)
        {
            Claim subjectClaim = HttpContext.User.FindFirst("sub");
            if (subjectClaim != null)
            {
                string userId = subjectClaim.Value;
                if (newPassword == confirmNewPassword)
                {
                    if (this.membershipProvider.ValidatePassword(HttpContext.User.Identity.Name, oldPassword))
                    {
                        try
                        {
                            await this.membershipProvider.ChangeUserPassword(userId, oldPassword, newPassword);
                        }
                        catch (Exception e)
                        {
                            throw new Exception("Համակարգային սխալ գաղտնաբառը փոխելիս", e);
                        }
                    }
                    else
                    {
                        throw new Exception("Ընթացիկ գաղտնաբառը սխալ է մուտքագրված");
                    }
                }
                else
                {
                    throw new Exception("Նոր գաղտնաբառը և նոր գաղտնաբառի հաստատումը իրար չեն համապատասխանում");
                }
            }
            else
            {
                throw new Exception("Անհնար է որոշել ընթացիկ օգտագործողին");
            }
        }

        [Authorize]
        [HttpPost("Password")]
        public async Task ChangePassword([FromBody] PasswordChangeRequest changePassword)
        {
            await this.ChangePasswordImpl(changePassword.oldPassword, changePassword.newPassword, changePassword.newPasswordConfirmation);
        }

        public class PasswordChangeRequest
        {
            public string oldPassword { get; set; }
            public string newPassword { get; set; }
            public string newPasswordConfirmation { get; set; }
        }
    }
}
