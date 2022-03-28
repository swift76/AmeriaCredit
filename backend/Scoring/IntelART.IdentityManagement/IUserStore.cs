using System.Collections.Generic;
using System.Threading.Tasks;

namespace IntelART.IdentityManagement
{
    public interface IUserStore
    {
        UserInfo GetUserByUsername(string username);
        UserInfo GetUserById(string id);
        bool ValidatePassword(string username, string password);

        Task<IEnumerable<string>> GetUserRolesById(string id);
    }
}
