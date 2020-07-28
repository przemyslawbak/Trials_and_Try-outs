using Microsoft.AspNetCore.Mvc;

namespace Integrate.Controllers
{
    //Angular + ASP.NET Core tutorial: https://www.infoq.com/articles/Angular-Core-3/
    public class HomeController : Controller
    {
        public ViewResult Index() => View();
    }
}
