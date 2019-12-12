using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Users.Controllers
{
    public class HomeController : Controller
    {
        public ViewResult Index() =>
        View(new Dictionary<string, object>
        { ["Miejsce zarezerwowane"] = "Miejsce zarezerwowane" });
    }
}
