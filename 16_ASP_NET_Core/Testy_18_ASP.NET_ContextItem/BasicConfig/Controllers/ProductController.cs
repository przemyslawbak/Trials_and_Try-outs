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
            if (HttpContext.Items["Counter"] != null)
            {
                x = int.Parse(HttpContext.Items["Counter"] as string);
            }
            x++;
            HttpContext.Items["Counter"] = x;

            x = int.Parse(HttpContext.Items["Counter"] as string);

            return View(x);
        }
    }
}
