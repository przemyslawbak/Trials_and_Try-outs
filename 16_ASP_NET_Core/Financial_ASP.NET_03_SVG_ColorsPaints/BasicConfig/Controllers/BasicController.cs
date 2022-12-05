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
            ColorNames model = new ColorNames();

            return View(model);
        }
    }
}
