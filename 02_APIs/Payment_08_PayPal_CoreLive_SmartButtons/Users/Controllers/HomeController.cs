using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Users.Controllers
{
    //https://developer.paypal.com/docs/checkout/integrate/#
    //8. Go Live
    public class HomeController : Controller
    {
        private readonly IConfiguration _config;

        public HomeController(IConfiguration config)
        {
            _config = config;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Cancel()
        {
            //jeśli przerwane przez użytkownika lub PayPal
            return View();
        }

        public IActionResult Completed()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Success(string orderID)
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
