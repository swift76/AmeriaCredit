using System;

namespace IntelART.Ameria.Entities
{
    public class CreditCardAuthorization
    {
        public Guid     ID            { get; set; }
        public string   SMS_HASH      { get; set; }
        public DateTime SMS_SENT_DATE { get; set; }
        public int      TRY_COUNT     { get; set; }
        public int      SMS_COUNT     { get; set; }
    }
}
