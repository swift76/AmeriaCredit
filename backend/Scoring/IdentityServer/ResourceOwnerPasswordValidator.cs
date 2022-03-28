using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using IntelART.IdentityManagement;

namespace IntelART.IdentityServer
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private IUserStore userStore;

        public ResourceOwnerPasswordValidator(IUserStore userStore)
        {
            this.userStore = userStore;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            if (!this.userStore.ValidatePassword(context.UserName, context.Password))
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.UnauthorizedClient, "Նման տվյալներով օգտատեր առկա չէ։ Խնդրում ենք փորձել նորից կամ գրանցել օգտատեր։");
            }
            else
            {
                UserInfo user = this.userStore.GetUserByUsername(context.UserName);
                context.Result = new GrantValidationResult(user.Id.ToString(), "custom");
            }
        }
    }
}
