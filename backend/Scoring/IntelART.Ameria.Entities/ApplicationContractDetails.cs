using System;

namespace IntelART.Ameria.Entities
{
    /// <summary>
    /// The preapproval application for the loan
    /// It contain all the info from the applicatin metadata, thus inheriting it
    /// </summary>
    public class ApplicationContractDetails
    {
        public DateTime CREATION_DATE { get; set; }
        public string CLIENT_CODE { get; set; }
        public string FIRST_NAME { get; set; }
        public string LAST_NAME { get; set; }
        public string PATRONYMIC_NAME { get; set; }
        public string FIRST_NAME_EN { get; set; }
        public string LAST_NAME_EN { get; set; }
        public string DOCUMENT_NUMBER { get; set; }
        public DateTime DOCUMENT_GIVEN_DATE { get; set; }
        public DateTime DOCUMENT_EXPIRY_DATE { get; set; }
        public string DOCUMENT_GIVEN_BY { get; set; }
        public string SOCIAL_CARD_NUMBER { get; set; }
        public DateTime BIRTH_DATE { get; set; }
        public string CITIZENSHIP_COUNTRY_NAME { get; set; }
        public string BIRTH_PLACE_NAME { get; set; }
        public string FAMILY_STATUS { get; set; }
        public string REGISTRATION_COUNTRY_NAME { get; set; }
        public string REGISTRATION_CITY_NAME { get; set; }
        public string REGISTRATION_STATE_NAME { get; set; }
        public string REGISTRATION_STREET { get; set; }
        public string REGISTRATION_BUILDNUM { get; set; }
        public string REGISTRATION_APARTMENT { get; set; }
        public string CURRENT_COUNTRY_NAME { get; set; }
        public string CURRENT_STATE_NAME { get; set; }
        public string CURRENT_CITY_NAME { get; set; }
        public string CURRENT_STREET { get; set; }
        public string CURRENT_BUILDNUM { get; set; }
        public string CURRENT_APARTMENT { get; set; }
        public string FIXED_PHONE { get; set; }
        public string MOBILE_PHONE_1 { get; set; }
        public string MOBILE_PHONE_2 { get; set; }
        public string EMAIL { get; set; }
        public string COMPANY_NAME { get; set; }
        public string ORGANIZATION_ACTIVITY_NAME { get; set; }
        public string COMPANY_PHONE { get; set; }
        public string POSITION { get; set; }
        public string MONTHLY_INCOME_NAME { get; set; }
        public string WORKING_EXPERIENCE_NAME { get; set; }
        public decimal INITIAL_AMOUNT         { get; set; }
        public byte REPAY_DAY { get; set; }
        public decimal FINAL_AMOUNT { get; set; }
        public decimal INTEREST { get; set; }
        public string PERIOD_TYPE_NAME { get; set; }
        public string CURRENCY_NAME           { get; set; }
        public byte IS_NEW_CARD { get; set; }
        public string EXISTING_CARD_CODE { get; set; }
        public string NEW_CARD_TYPE_NAME { get; set; }
        public string CARD_DELIVERY_BANK_BRANCH_NAME { get; set; }
        public string CARD_DELIVERY_ADDRESS { get; set; }
    }
}
