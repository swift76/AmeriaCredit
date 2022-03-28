using System;

namespace IntelART.Ameria.Entities
{
    public class OnboardingData
    {
        public Guid id { get; set; }
        public string first_name_eng { get; set; }
        public string last_name_eng { get; set; }
        public string mobile_number { get; set; }
        public string email { get; set; }
        public DateTime birth_date { get; set; }
        public string first_name_arm { get; set; }
        public string last_name_arm { get; set; }
        public string middle_name_arm { get; set; }
        //        public string address_region { get; set; }
        //        public string address_community { get; set; }
        //        public string address_street { get; set; }
        //        public string address_building { get; set; }
        //        public string address_apartment { get; set; }
        public byte document_type_id { get; set; }
        public string document_number { get; set; }
        public string soccard_number { get; set; }
        public DateTime? document_issue_date { get; set; }
        public string document_issuer { get; set; }
        //        public decimal? salary { get; set; }
        public bool is_student { get; set; }
    }
}
