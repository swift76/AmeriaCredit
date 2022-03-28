using System;
using System.ComponentModel.DataAnnotations;

namespace IntelART.Ameria.CustomerModuleWebApp.Controllers.Authentication
{
    public class VerificationModel
    {
        public Guid RegistrationProcessId { get; set; }
        public String Phone { get; set; }
        [Required(ErrorMessage = "Նույնականացման կոդը պետք է մուտքագրված լինի")]
        [RegularExpression("^[a-zA-Z0-9]{6}$", ErrorMessage = "Նույնականացման կոդը պետք է պարունակի վեց նիշ")]
        public string VerificationCode { get; set; }

        public bool IsActive { get; set; }
    }
}
