using System.ComponentModel.DataAnnotations;

namespace IntelART.IdentityServer.Authentication
{
    public class CreatePasswordModel
    {
        [Required]
        public string NewPassword { get; set; }
        [Required]
        public string ConfirmNewPassword { get; set; }
        [Required]
        public string PasswordChangeRequestToken { get; set; }
        [Required]
        public string Username { get; set; }
        public string ReturnUrl { get; set; }
    }
}
