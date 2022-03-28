using System;

namespace IntelART.Ameria.Entities
{
    public class ApplicationForOnboarding
    {
        public string FIRST_NAME_EN { get; set; }
        public string LAST_NAME_EN { get; set; }
        public string EMAIL { get; set; }
        public string PHONE { get; set; }
        public string FIRST_NAME_AM { get; set; }
        public string LAST_NAME_AM { get; set; }
        public string PATRONYMIC_NAME_AM { get; set; }
        public DateTime BIRTH_DATE { get; set; }
        public bool GENDER { get; set; }
        public string DISTRICT { get; set; }
        public string COMMUNITY { get; set; }
        public string STREET { get; set; }
        public string BUILDING { get; set; }
        public string APARTMENT { get; set; }
        public decimal SALARY { get; set; }
        public string NON_BIOMETRIC_PASSPORT_NUMBER { get; set; }
        public DateTime NON_BIOMETRIC_PASSPORT_ISSUE_DATE { get; set; }
        public DateTime NON_BIOMETRIC_PASSPORT_EXPIRY_DATE { get; set; }
        public string NON_BIOMETRIC_PASSPORT_ISSUED_BY { get; set; }
        public string BIOMETRIC_PASSPORT_NUMBER { get; set; }
        public DateTime BIOMETRIC_PASSPORT_ISSUE_DATE { get; set; }
        public DateTime BIOMETRIC_PASSPORT_EXPIRY_DATE { get; set; }
        public string BIOMETRIC_PASSPORT_ISSUED_BY { get; set; }
        public string ID_CARD_NUMBER { get; set; }
        public DateTime ID_CARD_ISSUE_DATE { get; set; }
        public DateTime ID_CARD_EXPIRY_DATE { get; set; }
        public string ID_CARD_ISSUED_BY { get; set; }
        public string SOCIAL_CARD_NUMBER { get; set; }
        public string DOCUMENT_TYPE_CODE { get; set; }
        public string DOCUMENT_NUMBER { get; set; }
        public bool IS_STUDENT { get; set; }
   }
}
