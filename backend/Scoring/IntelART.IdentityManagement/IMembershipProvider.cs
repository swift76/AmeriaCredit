using System.Threading.Tasks;

namespace IntelART.IdentityManagement
{
    public interface IMembershipProvider : IUserStore
    {
        //Task AddUser(ApplicationUser user);
        //Task<IEnumerable<ApplicationUser>> GetAllUsers();
        //Task<IEnumerable<ApplicationUser>> GetAllUsers(int from, int count);
        //Task<IEnumerable<ApplicationUser>> SearchUsers(string criteria);
        //Task DeleteUser(string id);
        //Task UpdateUserInfo(ApplicationUser user);
        Task ChangeUserPassword(string id, string oldPassword, string newPassword);
        //Task ActivateUser(string id);
        //Task DeactivateUser(string id);
    }
}
