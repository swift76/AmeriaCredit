namespace IntelART.Ameria.Entities
{
    public class LoanParameters
    {
        public byte REPAYMENT_DAY_FROM { get; set; }
        public byte REPAYMENT_DAY_TO   { get; set; }
        public bool IS_OVERDRAFT       { get; set; }
        public bool IS_REPAY_DAY_FIXED { get; set; }
        public bool IS_CARD_ACCOUNT    { get; set; }
        public bool IS_REPAY_START_DAY { get; set; }
        public bool IS_REPAY_NEXT_MONTH { get; set; }
        public byte REPAY_TRANSITION_DAY { get; set; }
    }
}
