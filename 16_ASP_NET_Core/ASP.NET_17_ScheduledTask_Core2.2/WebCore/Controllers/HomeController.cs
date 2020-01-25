using Microsoft.AspNetCore.Mvc;
using WebCore.Infrastructure;

namespace WebCore.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Integer = Number.Current.Integer.ToString();
            return View();
        }
    }
}
