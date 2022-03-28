using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Transactions;
using System.Xml;

namespace IntelART.Ameria.CLRServices
{
    public class NORQQuery
    {
        public NORQQueryResult GetNORQResult(DataHelper dataAccess, ServiceConfig config, string socialCardNumber, int queryTimeout)
        {
            XmlDocument document;
            NORQQueryResult result = new NORQQueryResult();
            result.XML = dataAccess.GetCachedNORQResponse(socialCardNumber);
            if (string.IsNullOrEmpty(result.XML))
            {
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("partner_password", config.UserPassword);
                parameters.Add("partner_username", config.UserName);
                parameters.Add("soccard", socialCardNumber);
                document = ServiceHelper.GetServiceResult(config.URL, "http://tempuri.org/IsrvNorq/f_GetUserDataXML", queryTimeout, "f_GetUserDataXML", parameters, "Request", "http://schemas.datacontract.org/2004/07/Norq.DataContract");
                XmlNode node = document.SelectSingleNode("/*[local-name()='Envelope']/*[local-name()='Body']/*[local-name()='f_GetUserDataXMLResponse']/*[local-name()='f_GetUserDataXMLResult']");
                if (node != null)
                {
                    result.XML = ServiceHelper.DecodeResponseXML(node.InnerXml);
                }
            }

            document = new XmlDocument();
            document.LoadXml(result.XML);

            result.FirstName = FormatName(ServiceHelper.GetNodeValue(document, "/PersonDataResponse/privatedata/Firstname"));
            result.LastName = FormatName(ServiceHelper.GetNodeValue(document, "/PersonDataResponse/privatedata/Lastname"));

            if (result.FirstName != string.Empty && result.LastName != string.Empty)
            {
                DateTime dateCurrent = dataAccess.GetServerDate();

                result.MiddleName = FormatName(ServiceHelper.GetNodeValue(document, "/PersonDataResponse/privatedata/Middlename"));
                result.Gender = (ServiceHelper.GetNodeValue(document, "/PersonDataResponse/privatedata/Gender") == "2");
                result.BirthDate = ServiceHelper.GetNORQDateValue(ServiceHelper.GetNodeValue(document, "/PersonDataResponse/privatedata/Birthdate"), dateCurrent);

                result.District = ServiceHelper.GetNodeValue(document, "/PersonDataResponse/privatedata/Region").ToUpper();
                result.Community = ServiceHelper.GetNodeValue(document, "/PersonDataResponse/privatedata/Community").ToUpper();
                result.Street = ServiceHelper.GetNodeValue(document, "/PersonDataResponse/privatedata/Street").ToUpper();
                result.Building = ServiceHelper.GetNodeValue(document, "/PersonDataResponse/privatedata/Building").ToUpper();
                result.Apartment = ServiceHelper.GetNodeValue(document, "/PersonDataResponse/privatedata/Apartment").ToUpper();

                result.NonBiometricPassport = GetDocumentData(document, "Passport", dateCurrent);
                result.BiometricPassport = GetDocumentData(document, "Biometric", dateCurrent);
                result.IDCard = GetDocumentData(document, "IdCard", dateCurrent);

                result.SocialCardNumber = ServiceHelper.GetNodeValue(document, "/PersonDataResponse/privatedata/Soccard");
                result.IsDead = (ServiceHelper.GetNodeValue(document, "/PersonDataResponse/privatedata/IsDead").ToUpper() == "TRUE");

                XmlNodeList listWork = document.SelectNodes("/PersonDataResponse/workdata/WorkData");
                foreach (XmlNode node in listWork)
                    result.Employment.Add(new WorkData()
                    {
                        OrganizationName = node.SelectSingleNode("OrganizationName").InnerXml,
                        RegistryCode = node.SelectSingleNode("RegistryCode").InnerXml,
                        TaxCode = Regex.Replace(node.SelectSingleNode("TaxCode").InnerXml, @"\s+", ""),
                        OrganizationAddress = node.SelectSingleNode("OrganizationAddress").InnerXml,
                        Position = node.SelectSingleNode("Position").InnerXml,
                        AgreementStartDate = ServiceHelper.GetNORQDateValue(node.SelectSingleNode("AgreementStartDate").InnerXml, DateTime.MinValue),
                        AgreementEndDate = ServiceHelper.GetNORQDateValue(node.SelectSingleNode("AgreementEndDate").InnerXml, DateTime.MaxValue),
                        Salary = decimal.Parse(node.SelectSingleNode("Salary").InnerXml),
                        SocialPayment = decimal.Parse(node.SelectSingleNode("SocialPayment").InnerXml)
                    });
            }
            else
                result.SocialCardNumber = socialCardNumber;
            return result;
        }

        private DocumentData GetDocumentData(XmlDocument document, string documentName, DateTime dateCurrent)
        {
            DocumentData result = new DocumentData();
            if (ServiceHelper.GetNodeValue(document, string.Format("/PersonDataResponse/privatedata/{0}valid", documentName.Substring(0, 1).ToUpper() + documentName.Substring(1).ToLower())).ToUpper() == "TRUE")
            {
                result.Number = ServiceHelper.GetNodeValue(document, string.Format("/PersonDataResponse/privatedata/{0}", documentName));
                result.IssueDate = ServiceHelper.GetNORQDateValue(ServiceHelper.GetNodeValue(document, string.Format("/PersonDataResponse/privatedata/{0}Date", documentName)), dateCurrent);
                result.ExpiryDate = ServiceHelper.GetNORQDateValue(ServiceHelper.GetNodeValue(document, string.Format("/PersonDataResponse/privatedata/{0}Vdate", documentName)), dateCurrent);
                result.IssuedBy = ServiceHelper.GetNodeValue(document, string.Format("/PersonDataResponse/privatedata/{0}Where", documentName));
            }
            return result;
        }

        private bool Validate(DataHelper dataAccess
            , Guid entityID
            , string entityFirstName
            , string entityLastName
            , DateTime entityBirthDate
            , ref NORQQueryResult response)
        {
            bool result;
            if (response.FirstName == string.Empty || response.LastName == string.Empty)
            {
                dataAccess.AutomaticallyRefuseApplication(entityID, "Սխալ փաստաթղթի տվյալներ");
                result = false;
            }
            else if ((FormatName(entityFirstName) != response.FirstName)
                || (FormatName(entityLastName) != response.LastName)
                || response.BirthDate != entityBirthDate)
            {
                dataAccess.AutomaticallyRefuseApplication(entityID, "Տվյալների անհամապատասխանություն");
                result = false;
            }
            else
            {
                response.Salary = 0;
                response.SocialPayment = 0;
                DateTime dateCurrent = dataAccess.GetServerDate();
                foreach (WorkData work in response.Employment)
                    if (work.AgreementEndDate >= dateCurrent)
                    {
                        response.Salary += work.Salary;
                        response.SocialPayment += work.SocialPayment;
                    }
                result = true;
            }
            return result;
        }

        public void GetResponse(DataHelper dataAccess, ServiceConfig config, NORQEntity entity, int queryTimeout)
        {
            dataAccess.SaveNORQTryCount(entity.ID);
            bool isValid;
            NORQQueryResult resultMain = GetNORQResult(dataAccess, config, entity.SocialCardNumber, queryTimeout);

            if (Validate(dataAccess, entity.ID, entity.FirstName, entity.LastName, entity.BirthDate, ref resultMain))
            {
                NORQQueryResult resultCoborrower;
                if (string.IsNullOrEmpty(entity.CoborrowerSocialCardNumber))
                {
                    resultCoborrower = null;
                    isValid = true;
                }
                else
                {
                    resultCoborrower = GetNORQResult(dataAccess, config, entity.CoborrowerSocialCardNumber, queryTimeout);
                    isValid = Validate(dataAccess, entity.ID, entity.CoborrowerFirstName, entity.CoborrowerLastName, entity.CoborrowerBirthDate.Value, ref resultCoborrower);
                }

                if (isValid)
                {
                    using (TransactionScope transScope = new TransactionScope())
                    {
                        if (dataAccess.LockApplicationByID(entity.ID, 1))
                            dataAccess.SaveNORQQueryResult(entity.ID, new NORQData()
                                {
                                    Main = resultMain,
                                    Coborrower = resultCoborrower
                                });
                        transScope.Complete();
                    }
                }
            }
        }

        private string FormatName(string source)
        {
            string result = source.Trim().ToUpper();
            result = result.Replace("և", "ԵՎ");
            result = result.Replace("ԵՒ", "ԵՎ");
            return result;
        }
    }
}
