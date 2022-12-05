using Microsoft.AspNetCore.Mvc;

namespace React_01_Podrecznik.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
