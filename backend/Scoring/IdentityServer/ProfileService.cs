using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IntelART.IdentityManagement;
using System.Collections.Generic;

namespace IntelART.IdentityServer
{
    public class ProfileService : IProfileService
    {
        private IUserStore userstore;

        public ProfileService(IUserStore userstore)
        {
            this.userstore = userstore;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            string sid = context.Subject.FindFirst("sub").Value;
            UserInfo user = this.userstore.GetUserById(sid);
            IEnumerable<string> roles = await this.userstore.GetUserRolesById(sid);

            context.IssuedClaims.Add(new Claim("name", user.Username));
            context.IssuedClaims.Add(new Claim(ClaimsIdentity.DefaultNameClaimType, user.Username));

            if (roles != null)
            {
                foreach (string role in roles)
                {
                    context.IssuedClaims.Add(new Claim("role", role));
                    context.IssuedClaims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, role));
                }
            }
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            return Task.FromResult(0);
        }
    }
}
