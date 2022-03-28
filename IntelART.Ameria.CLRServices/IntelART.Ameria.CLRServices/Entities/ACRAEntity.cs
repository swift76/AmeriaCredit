using System;

namespace IntelART.Ameria.CLRServices
{
    public class ACRAEntity
    {
        public Guid ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string PassportNumber { get; set; }
        public string SocialCardNumber { get; set; }
        public string IDCardNumber { get; set; }
        public int ImportID { get; set; }

        public string CoborrowerFirstName { get; set; }
        public string CoborrowerLastName { get; set; }
        public DateTime CoborrowerBirthDate { get; set; }
        public string CoborrowerPassportNumber { get; set; }
        public string CoborrowerSocialCardNumber { get; set; }
        public string CoborrowerIDCardNumber { get; set; }
    }
}
