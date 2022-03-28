using System.Collections.Generic;

namespace IntelART.Ameria.Entities
{
    /// <summary>
    /// The main application for the loan
    /// It contain all the info from the applicatin metadata, thus inheriting it
    /// </summary>
    public class MainApplication : Application
    {
        public byte SOURCE_ID                  { get; set; }
        public byte? REPAY_DAY                 { get; set; }
        public decimal? FINAL_AMOUNT           { get; set; }
        public decimal? INTEREST               { get; set; }
        public string FIRST_NAME_EN            { get; set; }
        public string LAST_NAME_EN             { get; set; }
        public string MOBILE_PHONE_2           { get; set; }
        public string FIXED_PHONE              { get; set; }
        public string EMAIL                    { get; set; }
        public string SHOP_CODE                { get; set; }
        public string PRODUCT_NUMBER           { get; set; }
        public string GOODS_RECEIVING_CODE     { get; set; }
        public string GOODS_DELIVERY_ADDRESS   { get; set; }
        public IEnumerable<ApplicationProduct> Products { get; set; }
        public string PERIOD_TYPE_CODE          { get; set; }
        public string BIRTH_PLACE_CODE          { get; set; }
        public string CITIZENSHIP_CODE          { get; set; }
        public string REGISTRATION_COUNTRY_CODE { get; set; }
        public string REGISTRATION_STATE_CODE   { get; set; }
        public string REGISTRATION_CITY_CODE    { get; set; }
        public string REGISTRATION_STREET       { get; set; }
        public string REGISTRATION_BUILDNUM     { get; set; }
        public string REGISTRATION_APARTMENT    { get; set; }
        public string CURRENT_COUNTRY_CODE      { get; set; }
        public string CURRENT_STATE_CODE        { get; set; }
        public string CURRENT_CITY_CODE         { get; set; }
        public string CURRENT_STREET            { get; set; }
        public string CURRENT_BUILDNUM          { get; set; }
        public string CURRENT_APARTMENT         { get; set; }
        public string COMMUNICATION_TYPE_CODE   { get; set; }
        public string COMPANY_NAME              { get; set; }
        public string COMPANY_PHONE             { get; set; }
        public string POSITION                  { get; set; }
        public string MONTHLY_INCOME_CODE       { get; set; }
        public string WORKING_EXPERIENCE_CODE   { get; set; }
        public string FAMILY_STATUS_CODE        { get; set; }
        public string OVERDRAFT_TEMPLATE_CODE   { get; set; }
        public bool AGREED_WITH_TERMS           { get; set; }
        public bool SUBMIT                      { get; set; } // Submit or Save

        public MainApplication()
        {
            this.Products = new List<ApplicationProduct>();
        }
    }
}
