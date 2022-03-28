using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IntelART.IdentityManagement;

namespace IntelART.IdentityServer
{
    public class TestUserStore : IUserStore
    {
        private IEnumerable<UserInfo> users;

        public TestUserStore(IEnumerable<UserInfo> users)
        {
            this.users = users;
        }

        public UserInfo GetUserById(string id)
        {
            return this.users.Where(user => user.Id.ToString() == id).FirstOrDefault();
        }

        public UserInfo GetUserByUsername(string username)
        {
            return this.users.Where(user => user.Username == username).FirstOrDefault();
        }

        public async Task<IEnumerable<string>> GetUserRolesById(string id)
        {
            return new[] { "BankUser" };
        }

        public bool ValidatePassword(string username, string password)
        {
            UserInfo user = this.GetUserByUsername(username);

            // TODO: FOr now returning true if the user exists
            // Need to discuss with Arsen why the user model
            // does not have a password property
            return user != null;
            ////return user != null && user.HashedPassword == password;
        }
    }
}
