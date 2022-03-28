using System;

namespace IntelART.Ameria.Entities
{
    public class CustomerUserRegistrationPreVerification : ApplicationUser
    {
        public Guid? PROCESS_ID { get; set; }
        public string FIRST_NAME_EN { get; set; }
        public string LAST_NAME_EN { get; set; }
        public string SOCIAL_CARD_NUMBER { get; set; }
        public string MOBILE_PHONE { get; set; }
        public string VERIFICATION_CODE { get; set; }
        public Guid? ONBOARDING_ID { get; set; }
        public bool IS_STUDENT { get; set; }
        public int TRY_COUNT { get; set; }
        public int SMS_COUNT { get; set; }
    }
}
