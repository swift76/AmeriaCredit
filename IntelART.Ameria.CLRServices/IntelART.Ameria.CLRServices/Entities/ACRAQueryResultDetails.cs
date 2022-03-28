using System;

namespace IntelART.Ameria.CLRServices
{
    public class ACRAQueryResultDetails
    {
        public string STATUS { get; set; }
        public DateTime FROM_DATE { get; set; }
        public DateTime TO_DATE { get; set; }
        public string TYPE { get; set; }
        public string CUR { get; set; }
        public decimal CONTRACT_AMOUNT { get; set; }
        public decimal DEBT { get; set; }
        public DateTime? PAST_DUE_DATE { get; set; }
        public string RISK { get; set; }
        public DateTime? CLASSIFICATION_DATE { get; set; }
        public decimal INTEREST_RATE { get; set; }
        public string PLEDGE { get; set; }
        public decimal PLEDGE_AMOUNT { get; set; }
        public decimal OUTSTANDING_AMOUNT { get; set; }
        public decimal OUTSTANDING_PERCENT { get; set; }
        public string BANK_NAME { get; set; }
        public bool IS_GUARANTEE { get; set; }
        public int DUE_DAYS_1 { get; set; }
        public int DUE_DAYS_2 { get; set; }
        public int DUE_DAYS_3 { get; set; }
        public int DUE_DAYS_4 { get; set; }
        public string LOAN_ID { get; set; }
    }
}
