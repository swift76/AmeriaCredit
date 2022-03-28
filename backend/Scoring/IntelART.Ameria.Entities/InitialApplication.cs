using System;

namespace IntelART.Ameria.Entities
{
    /// <summary>
    /// The preapproval application for the loan
    /// It contain all the info from the applicatin metadata, thus inheriting it
    /// </summary>
    public class InitialApplication : Application
    {
        public decimal? INITIAL_AMOUNT { get; set; }
        public string CURRENCY_CODE { get; set; }
        public string FIRST_NAME { get; set; }
        public string LAST_NAME { get; set; }
        public string PATRONYMIC_NAME { get; set; }
        public DateTime? BIRTH_DATE { get; set; }
        public string SOCIAL_CARD_NUMBER { get; set; }
        public string DOCUMENT_TYPE_CODE { get; set; }
        public string DOCUMENT_NUMBER { get; set; }
        public DateTime? DOCUMENT_GIVEN_DATE { get; set; }
        public DateTime? DOCUMENT_EXPIRY_DATE { get; set; }
        public string DOCUMENT_GIVEN_BY { get; set; }
        public string ORGANIZATION_ACTIVITY_CODE { get; set; }
        public string MOBILE_PHONE_AUTHORIZATION_CODE { get; set; }
        public bool AGREED_WITH_TERMS { get; set; }
        public string CLIENT_CODE { get; set; }
        public bool IS_DATA_COMPLETE { get; set; }
        public bool SUBMIT { get; set; } // Submit or Save
        public string REFUSAL_REASON { get; set; }
        public string MANUAL_REASON { get; set; }
        public bool IS_REFINANCING { get; set; }
        public string PARTNER_COMPANY_CODE { get; set; }

        public string UNIVERSITY_CODE { get; set; }
        public string UNIVERSITY_FACULTY { get; set; }
        public string UNIVERSITY_YEAR { get; set; }
    }
}
