using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace IntelART.Ameria.BankModuleWebApp.Controllers
{
    public class SharedController : Controller
    {
        public async Task<IActionResult> Unsupported()
        {
            ViewBag.DisableIERedirect = true;
            return View();
        }
    }
}
