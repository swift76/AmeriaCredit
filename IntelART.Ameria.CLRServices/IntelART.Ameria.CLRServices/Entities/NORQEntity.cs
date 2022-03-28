using System;

namespace IntelART.Ameria.CLRServices
{
    public class NORQEntity
    {
        public Guid ID { get; set; }
        public string SocialCardNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }

        public string CoborrowerSocialCardNumber { get; set; }
        public string CoborrowerFirstName { get; set; }
        public string CoborrowerLastName { get; set; }
        public DateTime? CoborrowerBirthDate { get; set; }
    }
}
