using BasicConfig.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace BasicConfig.Controllers
{
    //http://rion.io/2013/03/03/implementing-audit-trails-using-asp-net-mvc-actionfilters/
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
