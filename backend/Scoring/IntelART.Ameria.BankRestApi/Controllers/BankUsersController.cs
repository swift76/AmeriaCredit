using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using IntelART.Utilities;
using IntelART.Ameria.Entities;
using IntelART.Ameria.Repositories;
using System.Net.Http;
using Microsoft.AspNetCore.Authentication;

namespace IntelART.Ameria.BankRestApi.Controllers
{
    /// <summary>
    /// A controller class that exposes Bank users entities.
    /// </summary>
    [Route("[controller]")]
    [Authorize(Roles ="BankUser,BankPowerUser")]
    public class BankUsersController : Controller
    {
        private string identityServerUrl;
        private string ConnectionString;
        private BankUserRepository repository;
        private int CurrentUserID
        {
            get
            {
                return int.Parse(HttpContext.User.FindFirst("sub").Value);
            }
        }

        public BankUsersController(IConfigurationRoot Configuration)
        {
            this.ConnectionString = Configuration.GetSection("ConnectionStrings")["ScoringDB"];
            this.identityServerUrl = Configuration.GetSection("AutenticationService")["Url"];
            this.repository = new BankUserRepository(this.ConnectionString);
        }

        /// <summary>
        /// Returns the list of all Bank users
        /// </summary>
        /// <returns>Collection containing all Bank users</returns>
        [HttpGet]
        public IEnumerable<BankUser> Get()
        {
            IEnumerable<BankUser> bankUsers = this.repository.GetBankUsers();
            return bankUsers;
        }

        /// <summary>
        /// If ID is not null, modifies Bank user,
        /// else, creates Bank user
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "BankPowerUser")]
        public async void Post([FromBody]BankUser bankUser)
        {
            if (bankUser != null)
            {
                ////bankUser.HASH = Crypto.HashString(bankUser.PASSWORD);
                bankUser.HASH = "";
                if (bankUser.ID.HasValue) // modify bank user
                {
                    if (bankUser.OBJECT_STATE_ID.HasValue && bankUser.OBJECT_STATE_ID.Value == 2)
                    {
                        throw new ApplicationException("ERR-0009", "Bank users, who are locked, cannot be modified.");
                    }

                    if (!string.IsNullOrEmpty(bankUser.PASSWORD)) // password is modified
                    {
                        ValidationManager.ValidatePasswordChange(bankUser.LOGIN, string.Empty, bankUser.PASSWORD, bankUser.PASSWORD);
                    }
                    this.repository.ModifyBankUser(bankUser, this.CurrentUserID);
                }
                else // create bank user
                {
                    this.repository.CreateBankUser(bankUser, this.CurrentUserID);

                    HttpClient client = new HttpClient();
                    ////string accessToken = await HttpContext.Authentication.GetTokenAsync("access_token");
                    ////client.SetBearerToken(accessToken);
                    string returnUrl = string.Format("{0}://{1}", HttpContext.Request.Scheme, HttpContext.Request.Host);
                    string resetPasswordUrl = string.Format("{0}/Account/ResetPassword", this.identityServerUrl);
                    HttpResponseMessage response = await client.PostAsync(string.Format("{0}?username={1}&returnUrl={2}", resetPasswordUrl, bankUser.LOGIN, returnUrl), null);

                    if (response.StatusCode != System.Net.HttpStatusCode.OK
                        && response.StatusCode != System.Net.HttpStatusCode.NoContent)
                    {
                        throw new ApplicationException("ERR-0004", "Error occured wile initiating the password reset. Please retry.");
                    }

                }
            }
        }
        
        /// <summary>
        /// Close/Open Bank user
        /// </summary>
        [HttpPost]
        [Route("close_open")]
        [Authorize(Roles = "BankPowerUser")]
        public IActionResult CloseOpen([FromBody]BankUser bankUser)
        {
            if (bankUser == null
                || bankUser.ID == null
                || bankUser.OBJECT_STATE_ID == null)
            {
                throw new ApplicationException("ERR-1001", "Not all required parameters had been provided");
            }
            int bankUserId = bankUser.ID.Value;
            bool closeOpen = bankUser.OBJECT_STATE_ID.Value == 2;
            this.repository.CloseOpenBankUser(bankUserId,
                                              this.CurrentUserID,
                                              closeOpen);

            return Ok();
        }

        /// <summary>
        /// Returns the Bank users with the given Id
        /// </summary>
        /// <returns>Bank user with the given Id</returns>
        [HttpGet("{id}")]
        public BankUser Get(int id)
        {
            BankUser bankUser = this.repository.GetBankUser(id);
            return bankUser;
        }

        /// <summary>
        /// Changes Bank user password
        /// </summary>
        [HttpPut("login")]
        public void ChangeBankUserPassword(string login,
                                           [FromBody]string oldPassword,
                                           [FromBody]string newPassword,
                                           [FromBody]string newPasswordRepeat)
        {
            ValidationManager.ValidatePasswordChange(login, oldPassword, newPassword, newPasswordRepeat);

            if (repository.AuthenticateBankUser(login, Crypto.HashString(oldPassword)) == null)
                throw new Exception("Հին գաղտնաբառը սխալ է");

            repository.ChangeBankUserPassword(login, Crypto.HashString(newPassword));
        }
    }
}
