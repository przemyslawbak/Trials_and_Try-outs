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

        public ViewResult Index()
        {
            int[] arr = { 27, 50, 80, 65, 30, 81, 49, 94, 40, 100 };
            return View(arr);
        }
    }
}
