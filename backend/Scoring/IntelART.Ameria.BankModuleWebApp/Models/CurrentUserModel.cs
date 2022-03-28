using System.Collections.Generic;

namespace IntelART.Ameria.BankModuleWebApp.Models
{
    public class CurrentUserModel
    {
        public string Username { get; set; }
        public string FullName { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}
