using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using IntelART.Ameria.Repositories;

namespace IntelART.Ameria.LoanApplicationRestApi.Controllers
{
    /// <summary>
    /// Controller class to implement the API methods required for getting settings
    /// </summary>
    ////[Authorize(Roles ="ShopUser,ShopPowerUser")]
    [Authorize]
    [Route("/Settings")]
    public class LoanSettingsController : RepositoryControllerBase<ApplicationParameterRepository>
    {
        public LoanSettingsController(IConfigurationRoot configuration)
            : base(configuration, (connectionString) => new ApplicationParameterRepository(connectionString))
        {
        }

        /// <summary>
        /// Implements GET /Settings/FileMaxSize
        /// Returns maximal size of files allowed to be uploaded.
        /// </summary>
        [HttpGet("FileMaxSize")]
        public int GetFileMaxSize()
        {
            int fileMaxSize = Repository.GetFileMaxSize();
            return fileMaxSize;
        }
    }
}
