using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BasicConfig.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public async Task<PartialViewResult> LoadSomeShit()
        {
            await Task.Delay(5000);
            ViewBag.Dupa = "viewbag dupa displayed, together with partial";

            return new PartialViewResult
            {
                ViewName = "_LoadSomeShit",
                ViewData = ViewData,
            };
        }
    }


}
