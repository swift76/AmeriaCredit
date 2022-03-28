using System;

namespace IntelART.Ameria.Entities
{
    public class PersonalSheet
    {
        public Guid ID { get; set; }
        public DateTime DATE { get; set; }
        public string LOAN_TYPE { get; set; }
        public string LOAN_AMOUNT { get; set; }
        public string LOAN_DURATION { get; set; }
        public decimal LOAN_INTEREST { get; set; }
        public string CUSTOMER_NAME { get; set; }
        public string CUSTOMER_ADDRESS { get; set; }
        public string CUSTOMER_PHONE { get; set; }
        public string CUSTOMER_EMAIL { get; set; }
        public decimal OTHER_PAYMENTS_LOAN_SERVICE_FEE { get; set; }
        public decimal OTHER_PAYMENTS_CARD_SERVICE_FEE { get; set; }
        public decimal OTHER_PAYMENTS_CASH_OUT_FEE { get; set; }
        public decimal OTHER_PAYMENTS_OTHER_FEE { get; set; }
        public decimal OTHER_PAYMENTS_PROVISION_FEE { get; set; }
        public decimal OTHER_PAYMENTS { get; set; }
        public DateTime SIGNATURE_DATE { get; set; }
        public DateTime SIGNATURE_DATE1 { get; set; }
        public DateTime SIGNATURE_DATE2 { get; set; }

        public bool IS_OVERDRAFT { get; set; }
        public string TEMPLATE_CODE { get; set; }
        public decimal FINAL_AMOUNT { get; set; }
        public string CURRENCY_CODE { get; set; }
        public byte REPAY_DAY { get; set; }
        public string LOAN_TYPE_ID { get; set; }
        public string CREDIT_CARD_TYPE_CODE { get; set; }
        public string FORM_CAPTION { get; set; }

        public decimal LOAN_INTEREST_2 { get; set; }
        public decimal TOTAL_REPAYMENT { get; set; }
        public decimal TOTAL_INTEREST_AMOUNT { get; set; }

        public DateTime REPAYMENT_BEGIN_DATE { get; set; }
        public DateTime REPAYMENT_END_DATE { get; set; }
        public int MONTH_DURATION { get; set; }
    }
}
