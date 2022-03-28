using System;

namespace IntelART.Ameria.Entities
{
    public class OnboardingIntegrationRequest
    {
        public Guid id { get; set; }
        public string first_name_eng { get; set; }
        public string last_name_eng { get; set; }
        public string email { get; set; }
        public string mobile_number { get; set; }
        public DateTime birth_date { get; set; }
        public byte document_type_id { get; set; }
        public string document_number { get; set; }
        public string soccard_number { get; set; }
        public bool is_student { get; set; }
        public string back_url { get; set; }
    }
}
