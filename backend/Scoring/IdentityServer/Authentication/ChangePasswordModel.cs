using System.ComponentModel.DataAnnotations;

namespace IntelART.IdentityServer.Authentication
{
    public class ChangePasswordModel
    {
        [Required]
        public string NewPassword { get; set; }
        [Required]
        public string ConfirmNewPassword { get; set; }
        [Required]
        public string OldPassword { get; set; }
        public string Username { get; set; }
        public string ReturnUrl { get; set; }
    }
}
