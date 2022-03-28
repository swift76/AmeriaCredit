using System;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using IntelART.Utilities;
using IntelART.Ameria.Entities;
using IntelART.Ameria.Repositories;

namespace IntelART.Ameria.ShopRestApi.Controllers
{
    /// <summary>
    /// A controller class that exposes Shop users entities.
    /// </summary>
    [Route("[controller]")]
    [Authorize(Roles = "BankUser,BankPowerUser")]
    public class ShopUsersController : Controller
    {
        private string identityServerUrl;
        private string ConnectionString;
        private ShopUserRepository repository;
        private int CurrentUserID
        {
            get
            {
                return int.Parse(HttpContext.User.FindFirst("sub").Value);
            }
        }

        public ShopUsersController(IConfigurationRoot Configuration)
        {
            this.ConnectionString = Configuration.GetSection("ConnectionStrings")["ScoringDB"];
            this.identityServerUrl = Configuration.GetSection("AutenticationService")["Url"];
            this.repository = new ShopUserRepository(this.ConnectionString);
        }

        /// <summary>
        /// Returns the list of all Shop users
        /// </summary>
        /// <returns>Collection containing all Shop users</returns>
        [HttpGet]
        public IEnumerable<ShopUser> Get()
        {
            IEnumerable<ShopUser> shopUsers = repository.GetShopUsers();
            return shopUsers;
        }

        /// <summary>
        /// If ID is not null, modifies Shop user,
        /// else, creates Shop user
        /// </summary>
        [HttpPost]
        public async void Post([FromBody]ShopUser shopUser)
        {
            if (shopUser != null)
            {
                shopUser.HASH = "";
                if (shopUser.ID.HasValue) // modify shop user
                {
                    if (shopUser.OBJECT_STATE_ID.HasValue && shopUser.OBJECT_STATE_ID.Value == 2)
                    {
                        throw new ApplicationException("ERR-0010", "Shop users, who are locked, cannot be modified.");
                    }

                    if (!string.IsNullOrEmpty(shopUser.PASSWORD)) // password is modified
                    {
                        ValidationManager.ValidatePasswordChange(shopUser.LOGIN, string.Empty, shopUser.PASSWORD, shopUser.PASSWORD);
                    }
                    string login = string.Format("{0}_{1}", shopUser.SHOP_CODE.Trim(), shopUser.LOGIN.Trim().ToUpper());
                    shopUser.LOGIN = login;
                    this.repository.ModifyShopUser(shopUser, this.CurrentUserID);
                }
                else // create shop user
                {
                    string login = string.Format("{0}_{1}", shopUser.SHOP_CODE.Trim(), shopUser.LOGIN.Trim().ToUpper());
                    shopUser.LOGIN = login;
                    this.repository.CreateShopUser(shopUser, this.CurrentUserID);

                    HttpClient client = new HttpClient();
                    string returnUrl = string.Format("{0}://{1}", HttpContext.Request.Scheme, HttpContext.Request.Host);
                    string resetPasswordUrl = string.Format("{0}/Account/ResetPassword", this.identityServerUrl);
                    HttpResponseMessage response = await client.PostAsync(string.Format("{0}?username={1}&returnUrl={2}", resetPasswordUrl, shopUser.LOGIN, returnUrl), null);

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
        public IActionResult CloseOpen([FromBody]ShopUser shopUser)
        {
            if (shopUser == null
                || shopUser.ID == null
                || shopUser.OBJECT_STATE_ID == null)
            {
                throw new ApplicationException("ERR-1001", "Not all required parameters had been provided");
            }
            int shopUserId = shopUser.ID.Value;
            bool closeOpen = shopUser.OBJECT_STATE_ID.Value == 2;
            this.repository.CloseOpenShopUser(shopUserId,
                                              this.CurrentUserID,
                                              closeOpen);
            return Ok();
        }

        /// <summary>
        /// Returns the Shop users with the given Id
        /// </summary>
        /// <returns>Shop user with the given Id</returns>
        [HttpGet("{id}")]
        public ShopUser Get(int id)
        {
            ShopUser shopUser = this.repository.GetShopUser(id);
            return shopUser;
        }

        /// <summary>
        /// Changes Bank user password
        /// </summary>
        [HttpPut("login")]
        public void ChangeShopUserPassword(string login,
                                           [FromBody]string oldPassword,
                                           [FromBody]string newPassword,
                                           [FromBody]string newPasswordRepeat)
        {
            ValidationManager.ValidatePasswordChange(login, oldPassword, newPassword, newPasswordRepeat);

            if (repository.AuthenticateShopUser(login, Crypto.HashString(oldPassword)) == null)
                throw new Exception("Հին գաղտնաբառը սխալ է");

            repository.ChangeShopUserPassword(login, Crypto.HashString(newPassword));
        }
    }
}
