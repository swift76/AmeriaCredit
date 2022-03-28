using System;

namespace IntelART.Ameria.Entities
{
    public class CustomerUser : ApplicationUser
    {
        public string FIRST_NAME_EN { get; set; }
        public string LAST_NAME_EN { get; set; }
        public string PATRONYMIC_NAME { get; set; }
        public DateTime? BIRTH_DATE { get; set; }
        public string BIRTH_PLACE_CODE { get; set; }
        public string CITIZENSHIP_CODE { get; set; }
        public string MOBILE_PHONE { get; set; }
        public string FIXED_PHONE { get; set; }
        public string SOCIAL_CARD_NUMBER { get; set; }
        public string DOCUMENT_TYPE_CODE { get; set; }
        public string DOCUMENT_NUMBER { get; set; }
        public DateTime? DOCUMENT_GIVEN_DATE { get; set; }
        public DateTime? DOCUMENT_EXPIRY_DATE { get; set; }
        public string DOCUMENT_GIVEN_BY { get; set; }
        public string REGISTRATION_COUNTRY_CODE { get; set; }
        public string REGISTRATION_STATE_CODE { get; set; }
        public string REGISTRATION_CITY_CODE { get; set; }
        public string REGISTRATION_STREET { get; set; }
        public string REGISTRATION_BUILDNUM { get; set; }
        public string REGISTRATION_APARTMENT { get; set; }
        public string CURRENT_COUNTRY_CODE { get; set; }
        public string CURRENT_STATE_CODE { get; set; }
        public string CURRENT_CITY_CODE { get; set; }
        public string CURRENT_STREET { get; set; }
        public string CURRENT_BUILDNUM { get; set; }
        public string CURRENT_APARTMENT { get; set; }
        public string COMPANY_NAME { get; set; }
        public string COMPANY_PHONE { get; set; }
        public string ORGANIZATION_ACTIVITY_CODE { get; set; }
        public string WORKING_EXPERIENCE_CODE { get; set; }
        public string POSITION { get; set; }
        public string MONTHLY_INCOME_CODE { get; set; }
        public string TOTAL_EXPERIENCE_CODE { get; set; }
        public string FAMILY_STATUS_CODE { get; set; }
        public string CLIENT_CODE { get; set; }
        public Guid? ONBOARDING_ID { get; set; }
        public bool IS_STUDENT { get; set; }
        public bool? IS_EMAIL_VERIFIED { get; set; }
    }
}
