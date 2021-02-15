using BasicConfig.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BasicConfig.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        public ViewResult Index() => View(_context.Products);
        public ViewResult Other()
        {
            int x = 0;
            if (HttpContext.Session.GetInt32("Counter").HasValue)
            {
                x = HttpContext.Session.GetInt32("Counter").Value;
            }
            x++;
            HttpContext.Session.SetInt32("Counter", x);

            return View(x);
        }
    }
}
