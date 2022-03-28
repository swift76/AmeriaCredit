namespace IntelART.Ameria.Entities
{
    public class ScoringResults
    {
        public decimal AMOUNT { get; set; }
        public decimal INTEREST { get; set; }
        public byte TERM_FROM { get; set; }
        public byte TERM_TO { get; set; }
        public string TEMPLATE_CODE { get; set; }
        public string TEMPLATE_NAME { get; set; }
        public decimal SERVICE_AMOUNT { get; set; }
        public decimal SERVICE_INTEREST { get; set; }
        public decimal PREPAYMENT_AMOUNT { get; set; }
        public decimal PREPAYMENT_INTEREST { get; set; }
    }
}
