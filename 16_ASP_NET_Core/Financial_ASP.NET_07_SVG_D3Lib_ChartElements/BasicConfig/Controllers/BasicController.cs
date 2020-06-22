using BasicConfig.Models;
using Microsoft.AspNetCore.Mvc;

namespace BasicConfig.Controllers
{
    public class BasicController : Controller
    {
        private readonly ApplicationDbContext _context;
        public BasicController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
