using Microsoft.AspNetCore.Mvc;

namespace BasicConfig.Controllers
{
    public class ProductController : Controller
    {
        public ViewResult Index()
        {
            return View();
        }
    }
}
