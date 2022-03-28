using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using IntelART.Ameria.Repositories;
using IntelART.Ameria.Entities;
using IntelART.Communication;

namespace IntelART.Ameria.CustomerRestApi.Controllers
{
    /// <summary>
    /// A controller class that exposes functionality for customer account management.
    /// </summary>
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private ISmsSender smsSender;
        private IEmailSender emailSender;
        private CustomerUserRepository repository;
        
        public AccountController(IConfigurationRoot Configuration, ISmsSender smsSender, IEmailSender emailSender)
        {
            this.smsSender = smsSender;
            this.emailSender = emailSender;
            this.repository = new CustomerUserRepository(Configuration.GetSection("ConnectionStrings")["ScoringDB"]);
        }

        /// <summary>
        /// Creates new customer user
        /// </summary>
        [HttpPost]
        public async Task Post([FromBody]CustomerUserRegistrationPreVerification registrationProcess)
        {
            if (registrationProcess != null && !registrationProcess.ID.HasValue) // create customer user
            {
                if (registrationProcess.ONBOARDING_ID.HasValue)
                {
                    CustomerUser customerUser = new CustomerUser();
                    customerUser.FIRST_NAME_EN = registrationProcess.FIRST_NAME_EN;
                    customerUser.LAST_NAME_EN = registrationProcess.LAST_NAME_EN;
                    customerUser.SOCIAL_CARD_NUMBER = registrationProcess.SOCIAL_CARD_NUMBER;
                    customerUser.MOBILE_PHONE = registrationProcess.MOBILE_PHONE;
                    customerUser.EMAIL = registrationProcess.EMAIL;
                    customerUser.HASH = registrationProcess.HASH;
                    customerUser.ONBOARDING_ID = registrationProcess.ONBOARDING_ID;
                    customerUser.IS_STUDENT = registrationProcess.IS_STUDENT;
                    customerUser.IS_EMAIL_VERIFIED = true;

                    this.repository.CreateCustomerUser(customerUser);
                }
                else
                {
                    string smsCode = repository.GetAuthorizationCode();
                    registrationProcess.VERIFICATION_CODE = smsCode;
                    this.repository.StartRegistrationProcess(registrationProcess);
                    string phone = string.Format("374{0}", registrationProcess.MOBILE_PHONE.Trim());
                    await smsSender.SendAsync(phone, smsCode);
                }
            }
        }

        /// <summary>
        /// Resends SMS code to a newly created customer user
        /// /Account/{registrationProcessId}
        /// </summary>
        [HttpPost("{registrationProcessId}")]
        public async Task Post(Guid registrationProcessId)
        {
            CustomerUserRegistrationPreVerification registrationProcess = this.repository.GetRegistrationProcess(registrationProcessId);
            if (registrationProcess == null)
            {
                throw new ApplicationException("E-5002", "Անհայտ գրանցում");
            }

            int tryCount = int.Parse(repository.GetSetting("AUTHORIZATION_CODE_TRY_COUNT"));
            if (registrationProcess.SMS_COUNT >= tryCount)
            {
                throw new ApplicationException("E-5008", "SMS ուղարկելու քանակը սպառվեց");
            }

            string smsCode = repository.GetAuthorizationCode();
            repository.UpdateRegistrationProcess(registrationProcessId, smsCode);
            string phone = string.Format("374{0}", registrationProcess.MOBILE_PHONE.Trim());
            await smsSender.SendAsync(phone, smsCode);
        }

        /// <summary>
        /// Set the SMS verification code for the given registration process ID.
        /// /Account/{registrationProcessId}/Verification/{verificationCode}
        /// </summary>
        [HttpPut("{registrationProcessId}/Verification/{verificationCode}")]
        public void Put(Guid registrationProcessId, string verificationCode)
        {
            CustomerUserRegistrationPreVerification registrationProcess = this.repository.GetRegistrationProcess(registrationProcessId);
            if (registrationProcess == null)
            {
                throw new ApplicationException("E-5002", "Անհայտ գրանցում");
            }

            int tryCount = int.Parse(repository.GetSetting("AUTHORIZATION_CODE_TRY_COUNT"));
            if (registrationProcess.VERIFICATION_CODE != verificationCode)
            {
                if (registrationProcess.TRY_COUNT < tryCount)
                {
                    repository.SetTryCustomerUserRegistrationProcess(registrationProcessId);
                    throw new ApplicationException("E-5003", "Սխալ կոդ");
                }
                else
                {
                    throw new ApplicationException("E-5006", "Հնարավոր փորձերի քանակը սպառվեց");
                }
            }

            bool verifyEmail = (repository.GetSetting("EMAIL_VERIFICATION") == "1");

            CustomerUser customerUser = new CustomerUser();
            customerUser.FIRST_NAME_EN = registrationProcess.FIRST_NAME_EN;
            customerUser.LAST_NAME_EN = registrationProcess.LAST_NAME_EN;
            customerUser.SOCIAL_CARD_NUMBER = registrationProcess.SOCIAL_CARD_NUMBER;
            customerUser.MOBILE_PHONE = registrationProcess.MOBILE_PHONE;
            customerUser.EMAIL = registrationProcess.EMAIL;
            customerUser.HASH = registrationProcess.HASH;
            customerUser.ONBOARDING_ID = registrationProcess.ONBOARDING_ID;
            customerUser.IS_STUDENT = registrationProcess.IS_STUDENT;
            customerUser.IS_EMAIL_VERIFIED = !verifyEmail;

            if (customerUser != null && !customerUser.ID.HasValue) // create customer user
            {
                int userId = this.repository.CreateCustomerUser(customerUser);

                if (verifyEmail)
                {
                    Guid id = Guid.NewGuid();
                    this.repository.StartCustomerUserEmailVerificationProcess(id, userId, customerUser.EMAIL);
                    string url = string.Format("{0}://{1}{2}/{3}", HttpContext.Request.Scheme, HttpContext.Request.Host, Url.Action("ValidateEmail", "Account"), id.ToString("N"));
                    string bodyMessage = string.Format("Սեղմեք ստորև բերված հղման վրա՝ գրանցումն ավարտելու համար։<br /><a href=\"{0}\">{0}</a>", url);
                    this.emailSender.SendAsync(new EmailAddress(string.Format("{0} {1}", customerUser.FIRST_NAME_EN, customerUser.LAST_NAME_EN), customerUser.EMAIL), "Գրանցում", bodyMessage);
                }
            }
        }

        [HttpGet("ValidateEmail/{registrationProcessId}")]
        public void Put(string registrationProcessId)
        {
            repository.UpdateCustomerUserEmailVerificationProcess(new Guid(registrationProcessId));
        }

        [HttpGet("{restrictedPasswords}")]
        public async Task<IEnumerable<string>> GetRestrictedPasswords()
        {
            IEnumerable<string> restrictedPasswords = await repository.GetRestrictedPasswords();
            return restrictedPasswords;
        }
    }
}
