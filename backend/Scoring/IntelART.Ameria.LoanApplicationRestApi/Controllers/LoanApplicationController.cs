using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using IntelART.Ameria.Entities;
using IntelART.Ameria.Repositories;
using System.Threading.Tasks;
using IntelART.Communication;

namespace IntelART.Ameria.LoanApplicationRestApi.Controllers
{
    /// <summary>
    /// Controller class to implement the API methods required for viewing, creating, and 
    /// managing loan applications
    /// </summary>
    ////[Authorize(Roles ="ShopUser,ShopPowerUser")]
    [Authorize]
    [Route("/Applications")]
    public class LoanApplicationController : RepositoryControllerBase<ApplicationRepository>
    {
        private ISmsSender smsSender;

        public LoanApplicationController(IConfigurationRoot configuration, ISmsSender smsSender)
            : base(configuration, (connectionString) => new ApplicationRepository(connectionString))
        {
            this.smsSender = smsSender;
        }

        /// <summary>
        /// Implements GET /Applications?fromDate={fromDate}&toDate={toDate}&name={name}
        /// Returns all applications accessible to the current user
        /// </summary>
        [HttpGet]
        public async Task<IEnumerable<Application>> Get([FromQuery]DateTime fromDate,
                                                        [FromQuery]DateTime toDate,
                                                        [FromQuery]string name)
        {
            IEnumerable<Application> applications;
            fromDate = fromDate.Date;
            toDate = toDate.Date.AddDays(1);
            if (this.IsShopPowerUser) // shop manager
            {
                applications = await Repository.GetManagerApplications(this.CurrentUserID, fromDate, toDate, name);
            }
            else if (this.IsShopUser) // shop user
            {
                applications = await Repository.GetOperatorApplications(this.CurrentUserID, fromDate, toDate, name);
            }
            else // customer user
            {
                applications = await Repository.GetApplications(this.CurrentUserID);
            }

            return applications;
        }

        /// <summary>
        /// Implements GET /Applications/{id}
        /// Returns initial application with the given id
        /// </summary>
        [HttpGet("{id}")]
        public async Task<InitialApplication> Get(Guid id)
        {
            InitialApplication application = await Repository.GetInitialApplication(id);
            return application;
        }

        /// <summary>
        /// Implements POST /Applications
        /// Creates a new initial application
        /// </summary>
        [HttpPost]
        public async Task<Guid> Post([FromBody]InitialApplication application)
        {
            if (application.SUBMIT)
            {
                if (!application.AGREED_WITH_TERMS)
                {
                    throw new ApplicationException("ERR-0020", "User must agree with terms and conditions before submitting an application.");
                }

                if (application.LOAN_TYPE_ID == "00")
                {
                    if (Repository.GetSetting("MOBILE_PHONE_SMS_CHECK") == "1")
                    {
                        if (string.IsNullOrEmpty(application.MOBILE_PHONE_AUTHORIZATION_CODE))
                        {
                            throw new ApplicationException("ERR-0202", "Mobile phone authorization code is empty");
                        }
                        await Repository.CheckMobilePhoneAuthorization(application.ID.Value, application.MOBILE_PHONE_AUTHORIZATION_CODE);
                    }
                }
                else
                {
                    ApplicationCountSetting setting = await Repository.GetApplicationCountSetting(application.SOCIAL_CARD_NUMBER, application.ID);
                    if (setting.APPLICATION_COUNT > setting.REPEAT_COUNT)
                    {
                        throw new ApplicationException("ERR-0200", "Application count overflow");
                    }
                }
                if (await Repository.DoesClientWorksAtBank(application.SOCIAL_CARD_NUMBER))
                {
                    throw new ApplicationException("ERR-0201", "Բանկի աշխատակիցներին արգելվում է օգտվել առցանց վարկավորման համակարգերից");
                }
            }

            int? currentUserID = null;
            if (string.IsNullOrEmpty(application.PARTNER_COMPANY_CODE))
                currentUserID = this.CurrentUserID;

            return await Repository.CreateInitialApplication(application, currentUserID, this.IsShopUser);
        }

        /// <summary>
        /// Implements DELETE /Applications/{id}
        /// Deletes an application with the given id
        /// </summary>
        [HttpDelete("{id}")]
        public async Task Delete(Guid id)
        {
            await Repository.DeleteApplication(id);
        }

        /// <summary>
        /// Implements GET /Applications/ApplicationInformation/{id}
        /// Returns information of the application with the given id
        /// </summary>
        [HttpGet("ApplicationInformation/{id}")]
        public async Task<ApplicationInformation> ApplicationInformation(Guid id)
        {
            ApplicationInformation application = await Repository.GetApplicationInformation(id);
            return application;
        }

        /// <summary>
        /// Implements POST /Applications/MobilePhoneAuthorization/{id}
        /// Generates SMS code and sends it to the user for the given application ID
        /// </summary>
        [HttpPost("MobilePhoneAuthorization/{id}")]
        public async Task SendSMSCode(Guid id)
        {
            InitialApplication application = await Repository.GetInitialApplication(id);
            string code = await Repository.GenerateMobilePhoneAuthorizationCode(id);
            await smsSender.SendAsync(string.Format("374{0}", application.MOBILE_PHONE_1), code);
        }

        [HttpGet("UserRole")]
        public byte GetUserRole()
        {
            byte result;
            if (this.IsShopPowerUser) // shop manager
            {
                result = 3;
            }
            else if (this.IsShopUser) // shop user
            {
                result = 2;
            }
            else // customer user
            {
                result = 1;
            }
            return result;
        }
    }
}
