using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using IntelART.Utilities;
using IntelART.Ameria.Repositories;
using Microsoft.Extensions.Configuration;
using IntelART.Ameria.Entities;

namespace IntelART.Ameria.CustomerRestApi.Controllers
{
    /// <summary>
    /// A controller class that exposes Customers entities.
    /// </summary>
    [Route("[controller]")]
    [Authorize]
    public class ProfileController : Controller
    {
        private string ConnectionString;
        private CustomerUserRepository repository;
        private ApplicationRepository applicationRepository;

        private int CurrentUserID
        {
            get
            {
                return int.Parse(HttpContext.User.FindFirst("sub").Value);
            }
        }

        public ProfileController(IConfigurationRoot Configuration)
        {
            this.ConnectionString = Configuration.GetSection("ConnectionStrings")["ScoringDB"];
            this.repository = new CustomerUserRepository(this.ConnectionString);
            this.applicationRepository = new ApplicationRepository(this.ConnectionString);
        }

        /// <summary>
        /// Get the current user ID and lookup the
        /// CustomerUser object with that ID
        /// </summary>
        [HttpGet]
        public CustomerUser Get()
        {
            CustomerUser customerUser = this.repository.GetCustomerUser(this.CurrentUserID);
            customerUser.IS_STUDENT = applicationRepository.IsClientStudent(customerUser.SOCIAL_CARD_NUMBER);
            return customerUser;
        }

        /// <summary>
        /// Modify customer user
        /// </summary>
        [HttpPost]
        public void Post([FromBody]CustomerUser customerUser)
        {
            if (customerUser != null)
            {
                customerUser.HASH = "";
                if (!string.IsNullOrEmpty(customerUser.PASSWORD)) // password is modified
                {
                    ValidationManager.ValidatePasswordChange(customerUser.LOGIN, string.Empty, customerUser.PASSWORD, customerUser.PASSWORD);
                }
                this.repository.ModifyCustomerUser(customerUser, this.CurrentUserID);
            }
        }

        /// <summary>
        /// Changes Customer user password
        /// </summary>
        [HttpPut("login")]
        public void ChangeCustomerUserPassword(string login,
                                              [FromBody]string oldPassword,
                                              [FromBody]string newPassword,
                                              [FromBody]string newPasswordRepeat)
        {
            ValidationManager.ValidatePasswordChange(login, oldPassword, newPassword, newPasswordRepeat);

            if (repository.AuthenticateCustomerUser(login, Crypto.HashString(oldPassword)) == null)
                throw new Exception("Հին գաղտնաբառը սխալ է");

            repository.ChangeCustomerUserPassword(login, Crypto.HashString(newPassword));
        }
    }
}
