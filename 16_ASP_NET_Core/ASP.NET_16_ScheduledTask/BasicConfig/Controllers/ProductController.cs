using BasicConfig.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace BasicConfig.Controllers
{
    //https://blog.maartenballiauw.be/post/2017/08/01/building-a-scheduled-cache-updater-in-aspnet-core-2.html
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Integer = Number.Current.Integer.ToString();



            return View();
        }
    }


}
