using System;

namespace IntelART.Ameria.CLRServices
{
    public class ScoringResult
    {
        public Guid ID { get; set; }
        public byte Option { get; set; }
        public decimal Amount { get; set; }
        public decimal Coefficient { get; set; }
        public decimal Interest { get; set; }
    }
}
