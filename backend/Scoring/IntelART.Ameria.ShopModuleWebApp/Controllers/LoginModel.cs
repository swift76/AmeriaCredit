using System.ComponentModel.DataAnnotations;

namespace IntelART.Ameria.ShopModuleWebApp.Controllers
{
    public class LoginModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        public string ReturnUrl { get; set; }
    }
}
