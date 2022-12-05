using API_Controller_04_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API_Controller_04_API.Controllers
{
    /// <summary>
    /// Home controller displaying simple View
    /// </summary>
    public class HomeController : Controller
    {
        private ApplicationDbContext _context { get; set; }
        public HomeController(ApplicationDbContext context) => _context = context;
        public ViewResult Index() => View();
    }
}
