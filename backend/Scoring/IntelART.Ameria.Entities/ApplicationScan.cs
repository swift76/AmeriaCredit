using System;

namespace IntelART.Ameria.Entities
{
    public class ApplicationScan
    {
        public Guid APPLICATION_ID { get; set; }
        public string APPLICATION_SCAN_TYPE_CODE { get; set; }
        public string FILE_NAME { get; set; }
        public DateTime ATTACHMENT_TIME { get; set; }
    }
}
