using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Trial.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
