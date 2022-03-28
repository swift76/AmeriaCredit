using System;
using System.Collections.Generic;

namespace IntelART.Ameria.CLRServices
{
    public class NORQData
    {
        public NORQQueryResult Main { get; set; }
        public NORQQueryResult Coborrower { get; set; }
    }

    public class NORQQueryResult
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public DateTime BirthDate { get; set; }
        public bool Gender { get; set; }
        public string District { get; set; }
        public string Community { get; set; }
        public string Street { get; set; }
        public string Building { get; set; }
        public string Apartment { get; set; }
        public DocumentData NonBiometricPassport { get; set; }
        public DocumentData BiometricPassport { get; set; }
        public DocumentData IDCard { get; set; }
        public string SocialCardNumber { get; set; }
        public decimal Salary { get; set; }
        public decimal SocialPayment { get; set; }
        public bool IsDead { get; set; }
        public List<WorkData> Employment { get; set; }
        public string XML { get; set; }

        public NORQQueryResult()
        {
            Employment = new List<WorkData>();
            NonBiometricPassport = new DocumentData();
            BiometricPassport = new DocumentData();
            IDCard = new DocumentData();
        }
    }

    public class DocumentData
    {
        public string Number { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string IssuedBy { get; set; }

        public bool IsValid()
        {
            return (!string.IsNullOrEmpty(Number) && ExpiryDate >= DateTime.Now.Date);
        }
    }

    public class WorkData
    {
        public string OrganizationName { get; set; }
        public string RegistryCode { get; set; }
        public string TaxCode { get; set; }
        public string OrganizationAddress { get; set; }
        public string Position { get; set; }
        public DateTime AgreementStartDate { get; set; }
        public DateTime AgreementEndDate { get; set; }
        public decimal Salary { get; set; }
        public decimal SocialPayment { get; set; }
    }
}
