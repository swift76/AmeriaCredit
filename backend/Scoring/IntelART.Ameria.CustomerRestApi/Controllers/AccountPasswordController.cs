using System;
using Microsoft.AspNetCore.Mvc;
using IntelART.Utilities;
using IntelART.Ameria.Repositories;
using Microsoft.Extensions.Configuration;
using IntelART.Ameria.Entities;
using IntelART.Communication;
using System.Threading.Tasks;

namespace IntelART.Ameria.CustomerRestApi.Controllers
{
    /// <summary>
    /// A controller class that exposes functionality for customer account management.
    /// </summary>
    [Route("Account/{username}/PasswordManagerProcess")]
    public class AccountPasswordController : Controller
    {
        private ISmsSender smsSender;
        private CustomerUserRepository repository;

        public AccountPasswordController(IConfigurationRoot Configuration, ISmsSender smsSender)
        {
            this.smsSender = smsSender;
            string connectionString = Configuration.GetSection("ConnectionStrings")["ScoringDB"];
            this.repository = new CustomerUserRepository(connectionString);
        }

        /// <summary>
        /// Creates new password reset process
        /// </summary>
        [HttpPut("{processId}")]
        public async Task Put(string username, Guid processId)
        {
            if (processId != null)
            {
                if (this.repository.CheckCustomerUserExistenceByPhone(username))
                {
                    string smsCode = repository.GetAuthorizationCode6A();
                    this.repository.StartCustomerUserPasswordResetProcess(username, processId, Crypto.HashString(smsCode));
                    await smsSender.SendAsync(string.Format("374{0}", username), smsCode);
                }
            }
        }

        /// <summary>
        /// Updates the password within the context of the given password update process.
        /// /Account/{username}/PasswordManagerProcess/{processId}
        /// </summary>
        [HttpPost("{processId}")]
        public void Post(string username, Guid processId, [FromBody]CustomerUserPasswordResetData data)
        {
            if (data == null)
            {
                throw new ApplicationException("E-5111", "Գաղտնաբառի վերականգնման թերի հարցում");
            }
            this.repository.ResetCustomerUserPassword(username, Crypto.HashString(data.SmsCode), processId, Crypto.HashString(data.NewPassword));
        }
    }
}
