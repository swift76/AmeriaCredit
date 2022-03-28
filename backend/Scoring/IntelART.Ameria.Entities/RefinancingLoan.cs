using System;

namespace IntelART.Ameria.Entities
{
    public class RefinancingLoan
    {
        public Guid APPLICATION_ID       { get; set; }
        public int ROW_ID                { get; set; }
        public string ORIGINAL_BANK_NAME { get; set; }
        public string LOAN_TYPE          { get; set; }
        public string CURRENCY           { get; set; }
        public decimal INITIAL_INTEREST  { get; set; }
        public decimal INITIAL_AMOUNT    { get; set; }
        public decimal CURRENT_BALANCE   { get; set; }
        public DateTime DRAWDOWN_DATE    { get; set; }
        public DateTime MATURITY_DATE    { get; set; }
        public string LOAN_CODE          { get; set; }
    }
}
