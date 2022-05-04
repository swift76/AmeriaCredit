namespace IntelART.Ameria.Entities
{
    /// <summary>
    /// The agreed application for the loan
    /// It contain all the info from the applicatin metadata, thus inheriting it
    /// </summary>
    public class AgreedApplication : Application
    {
        public string EXISTING_CARD_CODE { get; set; }
        public bool IS_NEW_CARD { get; set; }
        public string CREDIT_CARD_TYPE_CODE { get; set; }
        public bool IS_CARD_DELIVERY { get; set; }
        public string CARD_DELIVERY_ADDRESS { get; set; }
        public string BANK_BRANCH_CODE { get; set; }
        public bool AGREED_WITH_TERMS { get; set; }
        public bool IS_ARBITRAGE_CHECKED { get; set; }
        public decimal ACTUAL_INTEREST { get; set; }
        public bool SUBMIT { get; set; } // Submit or Save
        public decimal? FINAL_AMOUNT { get; set; }
        public string CURRENCY_CODE { get; set; }
        public bool IsAgreementNeeded { get; set; }
    }
}
