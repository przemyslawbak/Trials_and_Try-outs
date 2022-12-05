using BasicConfig.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace BasicConfig.Controllers
{
    [LogUserActions]
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult StageFirst()
        {
            return View();
        }

        public IActionResult StageSecond()
        {
            return View();
        }

        public IActionResult StageThird()
        {
            return View();
        }
    }


}
