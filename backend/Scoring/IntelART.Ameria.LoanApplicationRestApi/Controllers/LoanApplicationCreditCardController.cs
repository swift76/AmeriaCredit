using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using IntelART.Ameria.Entities;
using IntelART.Ameria.Repositories;
using IntelART.Communication;

namespace IntelART.Ameria.LoanApplicationRestApi.Controllers
{
    /// <summary>
    /// Controller class to implement the API methods required
    /// for customers' credit card authorization
    /// </summary>
    ////[Authorize(Roles ="Customer")]
    [Authorize]
    [Route("/Applications/{id}")]
    public class LoanApplicationCreditCardController : RepositoryControllerBase<ApplicationRepository>
    {
        private ISmsSender smsSender;
        private CustomerUserRepository customerUserRepository;

        public LoanApplicationCreditCardController(IConfigurationRoot configuration, ISmsSender smsSender)
            : base(configuration, (connectionString) => new ApplicationRepository(connectionString))
        {
            this.smsSender = smsSender;
            customerUserRepository = new CustomerUserRepository(Repository.ConnectionString);
        }

        /// <summary>
        /// Implements POST /Applications/{id}/CreditCardAuthorization
        /// Generates SMS code and sends it to the user for the given application ID
        /// </summary>
        [HttpPost("CreditCardAuthorization")]
        public async Task SendSMSCode(Guid id)
        {
            ClientDataForCardValidation clientData = await Repository.GetClientDataForCardValidation(id);
            Client card = await Repository.GetClientData(clientData.CLIENT_CODE);
            string phone = card.MobilePhone;
            string code = await Repository.GenerateSMSCode(id);
            await smsSender.SendAsync(phone, code);
            await Repository.LogSMSCreditCardStep(id, null, phone);
        }

        /// <summary>
        /// Implements POST /Applications/{id}/CheckCreditCardAuthorization
        /// Checks whether the customer has entered correct SMS Code generated
        /// for the application with a given application ID
        /// </summary>
        [HttpPost("CheckCreditCardAuthorization")]
        public async Task Post(Guid id, [FromBody]string smsCode)
        {
            await Repository.CheckCreditCardAuthorization(id, smsCode);
            await Repository.SubmitAuthorizedApplication(id);
        }

        /// <summary>
        /// Implements POST /Applications/{id}/ValidateCard
        /// Validates entered card number and expiry date for the given application ID
        /// </summary>
        [HttpPost("ValidateCard")]
        public async Task<bool> ValidateCard(Guid id, [FromBody]ClientCard cardNumber)
        {
            bool isValid = false;
            ClientDataForCardValidation clientData = await Repository.GetClientDataForCardValidation(id);
            await Repository.LogSMSCreditCardStep(id, cardNumber.cardNumber, null);
            ClientCard card = await Repository.GetClientCardData(clientData.CLIENT_CODE, cardNumber.cardNumber, cardNumber.expiryDate.Value);
            if (card != null)
            {
                ServiceHelper helper = new ServiceHelper();
                ServiceConfiguration arcaConfiguration = customerUserRepository.GetServiceConfiguration("ArCa");
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("cardNumber", cardNumber.cardNumber);
                parameters.Add("expDate", cardNumber.expiryDate.Value.ToString("yyyyMM"));
                parameters.Add("password", arcaConfiguration.USER_PASSWORD);
                parameters.Add("username", arcaConfiguration.USER_NAME);
                XmlDocument document = helper.GetServiceResult(arcaConfiguration.URL, "http://tempuri.org/IsrvAPIGate/getCardData", 180, "getCardData", parameters, "Request", "http://schemas.datacontract.org/2004/07/APIGate.DataContract");
                XmlNode node = document.SelectSingleNode("/*[local-name()='Envelope']/*[local-name()='Body']/*[local-name()='getCardDataResponse']");
                if (node != null)
                {
                    document = new XmlDocument();
                    document.LoadXml(helper.DecodeResponseXML(node.InnerXml));
                    string operationCode = helper.GetNodeValue(document, "/*[local-name()='getCardDataResult']/*[local-name()='operation_code']");
                    int status = int.Parse(helper.GetNodeValue(document, "/*[local-name()='getCardDataResult']/*[local-name()='hotCardStatus']"));
                    if (operationCode == "0" && status != 1 && status != 3 && status != 6 && status != 7 && status != 8 && status != 9 && status != 10 && status != 12 && status != 19 && status != 20 && status != 21)
                    {
                        isValid = true;
                    }
                }
            }

            if (!isValid)
            {
                await Repository.SubmitNotAuthorizedApplication(id);
                if (card == null) // incorrect card number has been inserted
                {
                    throw new ApplicationException("ERR-0505", "Տվյալների անհամապատասխանություն. խնդրում ենք ճշգրտել։ " +
                        "Հակառակ դեպքում վարկը ստանալու համար խնդրում ենք անձը հաստատող փաստաթղթով մոտենալ «Ամերիաբանկ» ՓԲԸ-ի " +
                        "ցանկացած մասնաճյուղ կամ կապ հաստատել «Ամերիաբանկ» ՓԲԸ-ի  հետ (+37410) 56 11 11 հեռախոսահամարով։ " +
                        "«Ամերիաբանկ» ՓԲԸ-ի սպասարկման ցանցը և աշխատանքի ժամանակացույցը ներկայացված են " +
                        "<a href = \"http://ameriabank.am/Infrastructure.aspx?&lang=33\" target=\"_blank\"><b>հետևյալ հղումով:</b></a>");
                }
                else // correct card number has been inserted but the card has been blocked
                {
                    throw new ApplicationException("ERR-0507", "Վարկը ստանալու համար անհրաժեշտ է լրացուցիչ տեղեկատվություն։ " +
                        "Խնդրում ենք անձը հաստատող փաստաթղթով մոտենալ «Ամերիաբանկ» ՓԲԸ-ի ցանկացած մասնաճյուղ կամ կապ հաստատել " +
                        "«Ամերիաբանկ» ՓԲԸ-ի  հետ (+37410) 56 11 11 հեռախոսահամարով։ «Ամերիաբանկ» ՓԲԸ-ի սպասարկման ցանցը և " +
                        "աշխատանքի ժամանակացույցը ներկայացված են <a href = \"http://ameriabank.am/Infrastructure.aspx?&lang=33\" target=\"_blank\"><b>հետևյալ հղումով:</b></a>");
                }
            }
            else // card verified
            {
                await Repository.SubmitCardAuthorizedApplication(id);
                string phone = card.MobilePhone;
                string code = await Repository.GenerateSMSCode(id);
                await smsSender.SendAsync(phone, code);
                await Repository.LogSMSCreditCardStep(id, null, phone);
            }

            return isValid;
        }

        /// <summary>
        /// Gets the list of active cards
        /// </summary>
        [HttpGet("ActiveCards")]
        public async Task<IEnumerable<ActiveClientCard>> GetActiveCards(Guid id)
        {
            ClientDataForCardValidation clientData = await Repository.GetClientDataForCardValidation(id);
            IEnumerable<ActiveClientCard> cards = await Repository.GetActiveClientCards(clientData.CLIENT_CODE, clientData.LOAN_TYPE_ID, clientData.CURRENCY_CODE);
            return cards;
        }

        /// <summary>
        /// Gets the list of possible options to receive credit card types
        /// </summary>
        [HttpGet("CreditCardTypes")]
        public async Task<IEnumerable<DirectoryEntity>> GetCreditCardTypes(Guid id)
        {
            InitialApplication application = await Repository.GetInitialApplication(id);
            IEnumerable<DirectoryEntity> creditCardTypes = await Repository.GetCreditCardTypes(this.languageCode, application.LOAN_TYPE_ID, application.CURRENCY_CODE);
            return creditCardTypes;
        }
    }
}
