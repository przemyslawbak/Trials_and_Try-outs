using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Users.Controllers
{
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public IActionResult Index() => View();
    }
}
