using System;

namespace IntelART.Ameria.Entities
{
    public class ApplicationUser
    {
        public int? ID { get; set; }

        public string LOGIN { get; set; }

        public string PASSWORD { get; set; }

        public string HASH { get; set; }

        public string FIRST_NAME { get; set; }

        public string LAST_NAME { get; set; }

        public string EMAIL { get; set; }

        public DateTime? CREATE_DATE { get; set; }

        public DateTime? PASSWORD_EXPIRY_DATE { get; set; }

        public DateTime? CLOSE_DATE { get; set; }

        public byte? OBJECT_STATE_ID { get; set; }

        public string OBJECT_STATE_DESCRIPTION { get; set; }
    }
}
