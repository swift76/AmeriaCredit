using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using IntelART.Ameria.Entities;
using IntelART.Ameria.Repositories;

namespace IntelART.Ameria.BankRestApi.Controllers
{
    /// <summary>
    /// A controller class that exposes partner entities.
    /// The partners are the establishments that sell
    /// bank's loan services to the customers. For now
    /// only one type of partner will be there, a shop,
    /// but in the future other types of partners might
    /// be added as well.
    /// </summary>
    [Route("[controller]")]
    [Authorize(Roles = "BankUser,BankPowerUser")]
    public class ShopsController : Controller
    {
        public string ConnectionString;

        public ShopsController(IConfigurationRoot Configuration)
        {
            this.ConnectionString = Configuration.GetSection("ConnectionStrings")["ScoringDB"];
        }

        /// <summary>
        /// Returns the list of all partners
        /// </summary>
        /// <returns>Collection containing all partners</returns>
        [HttpGet]
        public IEnumerable<Shop> Get()
        {
            IEnumerable<Shop> shops = (new ShopRepository(this.ConnectionString)).GetShops();
            return shops;
        }
    }
}
