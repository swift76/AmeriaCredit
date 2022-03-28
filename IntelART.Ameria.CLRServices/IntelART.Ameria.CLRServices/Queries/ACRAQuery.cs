using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;
using System.Xml;

namespace IntelART.Ameria.CLRServices
{
    public class ACRAQuery
    {
        public ACRAQueryResult GetACRAResult(DataHelper dataAccess, ServiceConfig config, string sessionID, string serviceType, string socialCard, string firstName, string lastName, DateTime birthDate, string passport, string idCard, int importID, int queryTimeout)
        {
            XmlDocument document;
            ACRAQueryResult result = new ACRAQueryResult();
            if (serviceType == "acra_monitoring")
                result.XML = dataAccess.GetCachedACRAClientResponse(socialCard);
            else
                result.XML = dataAccess.GetCachedACRAResponse(socialCard);
            
            result.FicoScore = string.Empty;

            if (string.IsNullOrEmpty(result.XML))
            {
                bool isMonitoring;
                string requestTarget;
                string usageRange;
                if (importID > 0)
                {
                    isMonitoring = false;
                    requestTarget = "11";
                    usageRange = "54";
                }
                else
                {
                    isMonitoring = (serviceType == "acra_monitoring") || (dataAccess.GetSettingValue("IS_MONITORING") == "1");
                    if (isMonitoring)
                    {
                        requestTarget = "2";
                        usageRange = "21";
                    }
                    else
                    {
                        requestTarget = "1";
                        usageRange = "1";
                    }
                }

                StringBuilder requestPerson = new StringBuilder();
                requestPerson.Append(@"<a:PersonParticipientRequest>");
                requestPerson.AppendFormat(@"<a:DateofBirth>{0}</a:DateofBirth>", birthDate.ToString("dd-MM-yyyy"));
                requestPerson.AppendFormat(@"<a:FirstName>{0}</a:FirstName>", firstName);
                requestPerson.AppendFormat(@"<a:IdCardNumber>{0}</a:IdCardNumber>", idCard);
                requestPerson.Append(@"<a:KindBorrower>1</a:KindBorrower>");
                requestPerson.AppendFormat(@"<a:LastName>{0}</a:LastName>", lastName);
                requestPerson.AppendFormat(@"<a:PassportNumber>{0}</a:PassportNumber>", passport);
                requestPerson.AppendFormat(@"<a:RequestTarget>{0}</a:RequestTarget>", requestTarget);
                requestPerson.AppendFormat(@"<a:SocCardNumber>{0}</a:SocCardNumber>", socialCard);
                requestPerson.AppendFormat(@"<a:UsageRange>{0}</a:UsageRange>", usageRange);
                requestPerson.Append(@"<a:id>1</a:id>");
                requestPerson.Append(@"</a:PersonParticipientRequest>");

                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("AppNumber", ServiceHelper.GenerateUniqueID(15));
                parameters.Add("DateTime", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                parameters.Add("ReportType", "02");
                parameters.Add("ReqID", ServiceHelper.GenerateUniqueID(13));
                parameters.Add("SID", sessionID);
                parameters.Add("participient", requestPerson.ToString());
                Dictionary<string, string> parentParameters = new Dictionary<string, string>();
                parentParameters.Add("service_type", isMonitoring ? "acra_monitoring" : serviceType);

                document = ServiceHelper.GetServiceResult(config.URL, "http://tempuri.org/IsrvACRA/f_AcraPersonLoanXML", queryTimeout, "f_AcraPersonLoanXML", parameters, "personrequest", "http://schemas.datacontract.org/2004/07/ACRA.business.person", parentParameters);
                XmlNode node = document.SelectSingleNode("/*[local-name()='Envelope']/*[local-name()='Body']/*[local-name()='f_AcraPersonLoanXMLResponse']/*[local-name()='f_AcraPersonLoanXMLResult']");
                if (node != null)
                {
                    result.XML = ServiceHelper.DecodeResponseXML(node.InnerXml);
                }
            }

            if (result.XML.Substring(0, 7).ToLower() == "<error>")
                return null;
            else
            {
                document = ServiceHelper.CheckACRAResponse(result.XML);

                string presence = ServiceHelper.GetNodeValue(document, "/ROWDATA[@*]/PARTICIPIENT[@*]/ThePresenceData");

                if (presence == "2")
                    result.IsBlocked = true;
                else
                {
                    result.IsBlocked = false;
                    if (presence == "1")
                    {
                        DateTime dateCurrent = dataAccess.GetServerDate();
                        int currentYear = dateCurrent.Year;
                        int currentMonth = dateCurrent.Month;
                        ParseLoanGuarantee(dataAccess, document, false, currentYear, currentMonth, result.Details);
                        ParseLoanGuarantee(dataAccess, document, true, currentYear, currentMonth, result.Details);
                        ParseInterrelated(document, result.Interrelated);
                    }
                    ParseQuery(dataAccess, document, result.Queries);
                    result.FicoScore = ServiceHelper.GetNodeValue(document, "/ROWDATA[@*]/PARTICIPIENT[@*]/Score/FICOScore");
                }

                return result;
            }
        }

        private bool Validate(DataHelper dataAccess, Guid entityID, ref ACRAQueryResult response)
        {
            bool result;
            if (response == null)
                result = false;
            else if (response.IsBlocked)
            {
                dataAccess.AutomaticallyRefuseApplication(entityID, "Վարկային զեկույցը արգելափակված է");
                result = false;
            }
            else
                result = true;
            return result;
        }

        public void GetResponse(DataHelper dataAccess, ServiceConfig config, string sessionID, ACRAEntity entity, int queryTimeout)
        {
            dataAccess.SaveACRATryCount(entity.ID);
            bool isValid;
            ACRAQueryResult resultMain = GetACRAResult(dataAccess, config, sessionID, "acra_service", entity.SocialCardNumber, entity.FirstName, entity.LastName, entity.BirthDate, entity.PassportNumber, entity.IDCardNumber, entity.ImportID, queryTimeout);

            if (Validate(dataAccess, entity.ID, ref resultMain))
            {
                ACRAQueryResult resultCoborrower;
                if (string.IsNullOrEmpty(entity.CoborrowerSocialCardNumber))
                {
                    resultCoborrower = null;
                    isValid = true;
                }
                else
                {
                    resultCoborrower = GetACRAResult(dataAccess, config, sessionID, "acra_service", entity.CoborrowerSocialCardNumber, entity.CoborrowerFirstName, entity.CoborrowerLastName, entity.CoborrowerBirthDate, entity.CoborrowerPassportNumber, entity.CoborrowerIDCardNumber, entity.ImportID, queryTimeout);
                    isValid = Validate(dataAccess, entity.ID, ref resultCoborrower);
                }

                if (isValid)
                {
                    using (TransactionScope transScope = new TransactionScope())
                    {
                        if (dataAccess.LockApplicationByID(entity.ID, 2))
                            dataAccess.SaveACRAQueryResult(entity.ID, new ACRAData()
                        {
                            Main = resultMain,
                            Coborrower = resultCoborrower
                        });
                        transScope.Complete();
                    }
                }
            }
        }

        private static void ParseQuery(DataHelper dataAccess, XmlDocument document, List<ACRAQueryResultQueries> queries)
        {
            XmlNodeList list = document.SelectNodes("/ROWDATA[@*]/PARTICIPIENT[@*]/Requests/Request");
            foreach (XmlNode node in list)
                queries.Add(new ACRAQueryResultQueries()
                {
                    DATE = ServiceHelper.GetACRADateValue(ServiceHelper.RetrieveValue(node.SelectSingleNode("DateTime").InnerXml)),
                    BANK_NAME = ServiceHelper.RetrieveValue(node.SelectSingleNode("BankName").InnerXml)
                });
        }

        private static void ParseLoanGuarantee(DataHelper dataAccess, XmlDocument document, bool isGuarantee, int currentYear, int currentMonth, List<ACRAQueryResultDetails> details)
        {
            string prefixLG = isGuarantee ? "Guarantee" : "Loan";
            XmlNodeList list = document.SelectNodes(string.Format("/ROWDATA[@*]/PARTICIPIENT[@*]/{0}s/{0}", prefixLG));
            foreach (XmlNode node in list)
            {
                string status = ServiceHelper.RetrieveValue(node.SelectSingleNode("CreditStatus").InnerXml);
                DateTime dateFrom = ServiceHelper.GetACRADateValue(ServiceHelper.RetrieveValue(node.SelectSingleNode("CreditStart").InnerXml), DateTime.MinValue);
                DateTime dateTo = ServiceHelper.GetACRADateValue(ServiceHelper.RetrieveValue(node.SelectSingleNode("CloseDate").InnerXml), DateTime.MaxValue);
                string type = ServiceHelper.RetrieveValue(node.SelectSingleNode("LiabilityKind").InnerXml);
                string cur = ServiceHelper.RetrieveValue(node.SelectSingleNode("Currency").InnerXml);
                decimal amount = decimal.Parse(ServiceHelper.RetrieveValue(node.SelectSingleNode("Amount").InnerXml));
                decimal balance = decimal.Parse(ServiceHelper.RetrieveValue(node.SelectSingleNode("Balance").InnerXml));
                DateTime? datePastDue = ServiceHelper.GetACRANullableDateValue(ServiceHelper.RetrieveValue(node.SelectSingleNode("OutstandingDate").InnerXml));
                string risk = ServiceHelper.RetrieveValue(node.SelectSingleNode(string.Format("The{0}Class", prefixLG)).InnerXml);
                DateTime? dateClassification = ServiceHelper.GetACRANullableDateValue(ServiceHelper.RetrieveValue(node.SelectSingleNode("LastClassificationDate").InnerXml));
                decimal interestRate = decimal.Parse(ServiceHelper.RetrieveValue(node.SelectSingleNode("Interest").InnerXml));
                string pledge = ServiceHelper.RetrieveValue(node.SelectSingleNode("PledgeSubject").InnerXml);
                decimal pledge_amount = ServiceHelper.RetrieveOptionalAmount(node, "CollateralAmount");
                decimal outstanding_amount = ServiceHelper.RetrieveOptionalAmount(node, "AmountOverdue");
                decimal outstanding_percent = ServiceHelper.RetrieveOptionalAmount(node, "OutstandingPercent");
                string bankName = ServiceHelper.RetrieveValue(node.SelectSingleNode("SourceName").InnerXml).ToUpper();
                string loanID = ServiceHelper.RetrieveValue(node.SelectSingleNode("CreditID").InnerXml);

                int dueDays1 = 0, dueDays2 = 0, dueDays3 = 0, dueDays4 = 0;

                DateTime? dateLastPayment = ServiceHelper.GetACRANullableDateValue(ServiceHelper.RetrieveValue(node.SelectSingleNode(string.Format("{0}LastPaymentDate", prefixLG)).InnerXml));

                if (node.SelectSingleNode("OutstandingDaysCount") != null)
                {
                    dueDays1 += GetDueDaysByYear(node, currentYear, currentMonth, 1, dateLastPayment, dateTo);
                    dueDays2 = dueDays1 + GetDueDaysByYear(node, currentYear, currentMonth, 2, dateLastPayment, dateTo);
                    dueDays3 = dueDays2 + GetDueDaysByYear(node, currentYear, currentMonth, 3, dateLastPayment, dateTo);
                    dueDays4 = dueDays3 + GetDueDaysByYear(node, currentYear, currentMonth, 4, dateLastPayment, dateTo);
                }

                details.Add(new ACRAQueryResultDetails()
                {
                    STATUS = status,
                    FROM_DATE = dateFrom,
                    TO_DATE = dateTo,
                    TYPE = type,
                    CUR = cur,
                    CONTRACT_AMOUNT = amount,
                    DEBT = balance,
                    PAST_DUE_DATE = datePastDue,
                    RISK = risk,
                    CLASSIFICATION_DATE = dateClassification,
                    INTEREST_RATE = interestRate,
                    PLEDGE = pledge,
                    PLEDGE_AMOUNT = pledge_amount,
                    OUTSTANDING_AMOUNT = outstanding_amount,
                    OUTSTANDING_PERCENT = outstanding_percent,
                    BANK_NAME = bankName,
                    IS_GUARANTEE = isGuarantee,
                    DUE_DAYS_1 = dueDays1,
                    DUE_DAYS_2 = dueDays2,
                    DUE_DAYS_3 = dueDays3,
                    DUE_DAYS_4 = dueDays4,
                    LOAN_ID = loanID
                });
            }
        }

        private static void ParseInterrelated(XmlDocument document, List<ACRAQueryResultInterrelated> interrelated)
        {
            XmlNodeList list = document.SelectNodes("/ROWDATA[@*]/PARTICIPIENT[@*]/InterrelatedData/Interrelated/InterrelatedLoans/InterrelatedLoan");
            foreach (XmlNode node in list)
            {
                DateTime dateFrom = ServiceHelper.GetACRADateValue(ServiceHelper.RetrieveValue(node.SelectSingleNode("CreditStart").InnerXml), DateTime.MinValue);
                DateTime dateTo = ServiceHelper.GetACRADateValue(ServiceHelper.RetrieveValue(node.SelectSingleNode("LastInstallment").InnerXml), DateTime.MaxValue);
                string cur = ServiceHelper.RetrieveValue(node.SelectSingleNode("Currency").InnerXml);
                decimal amount = decimal.Parse(ServiceHelper.RetrieveValue(node.SelectSingleNode("ContractAmount").InnerXml));
                decimal due_amount = ServiceHelper.RetrieveOptionalAmount(node, "AmountDue");
                decimal overdue_amount = ServiceHelper.RetrieveOptionalAmount(node, "AmountOverdue");
                decimal outstanding_percent = ServiceHelper.RetrieveOptionalAmount(node, "OutstandingPercent");
                string risk = ServiceHelper.RetrieveValue(node.SelectSingleNode("CreditClassification").InnerXml);

                interrelated.Add(new ACRAQueryResultInterrelated()
                {
                    FROM_DATE = dateFrom,
                    TO_DATE = dateTo,
                    CUR = cur,
                    RISK = risk,
                    CONTRACT_AMOUNT = amount,
                    DUE_AMOUNT = due_amount,
                    OVERDUE_AMOUNT = overdue_amount,
                    OUTSTANDING_PERCENT = outstanding_percent
                });
            }
        }

        private static int GetDueDaysByYear(XmlNode node, int currentYear, int currentMonth, int shift,DateTime? dateLastPayment, DateTime dateTo)
        {
            int result = 0;

            XmlNodeList listDueYear = node.SelectNodes("OutstandingDaysCount/Year[@*]");
            foreach (XmlNode nodeDueYear in listDueYear)
            {
                XmlAttributeCollection colYear = nodeDueYear.Attributes;
                foreach (XmlAttribute attrYear in colYear)
                {
                    if (attrYear.Name.ToLower() == "name")
                    {
                        int valueYear = int.Parse(attrYear.Value);
                        if ((valueYear == (currentYear - shift)) || (valueYear == (currentYear - shift + 1)))
                        {
                            XmlNodeList listDueMonth = nodeDueYear.SelectNodes("Month");
                            foreach (XmlNode nodeDueMonth in listDueMonth)
                            {
                                XmlAttributeCollection colMonth = nodeDueMonth.Attributes;
                                foreach (XmlAttribute attrMonth in colMonth)
                                {
                                    if (attrYear.Name.ToLower() == "name")
                                    {
                                        int valueMonth = int.Parse(attrMonth.Value);
                                        if (IsDateInYear(currentYear, currentMonth, shift, valueYear, valueMonth))
                                        {
                                            int days = 0;
                                            if (int.TryParse(ServiceHelper.RetrieveValue(nodeDueMonth.InnerXml), out days))
                                                result += days;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (dateLastPayment.HasValue)
            {
                int overdueDays = (dateTo - dateLastPayment.Value).Days;
                if (overdueDays < 0)
                {
                    if (IsDateInYear(currentYear, currentMonth, shift, dateLastPayment.Value.Year, dateLastPayment.Value.Month))
                        result -= overdueDays;
                }
            }

            return result;
        }

        private static bool IsDateInYear(int currentYear, int currentMonth, int shift, int valueYear, int valueMonth)
        {
            return ((valueYear == (currentYear - shift) && valueMonth > currentMonth) || valueYear == (currentYear - shift + 1) && valueMonth <= currentMonth);
        }
    }
}
