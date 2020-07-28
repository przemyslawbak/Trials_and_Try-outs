using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        public ViewResult Index()
        {
            return View();
        }

        [Route("app1/{*url}")]
        public IActionResult App1(string url)
        {
            return View("App1", url);
        }
    }
}

