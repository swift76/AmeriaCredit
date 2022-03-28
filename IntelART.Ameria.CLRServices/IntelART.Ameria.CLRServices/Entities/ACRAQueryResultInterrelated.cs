using System;

namespace IntelART.Ameria.CLRServices
{
    public class ACRAQueryResultInterrelated
    {
        public DateTime FROM_DATE { get; set; }
        public DateTime TO_DATE { get; set; }
        public string CUR { get; set; }
        public decimal CONTRACT_AMOUNT { get; set; }
        public decimal DUE_AMOUNT { get; set; }
        public decimal OVERDUE_AMOUNT { get; set; }
        public decimal OUTSTANDING_PERCENT { get; set; }
        public string RISK { get; set; }
    }
}
