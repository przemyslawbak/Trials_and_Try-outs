using Microsoft.AspNetCore.Mvc;
using React_01_Podrecznik.Models;

namespace React_01_Podrecznik.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index(MultiselectViewModel model)
        {
            return View(model);
        }
    }
}
