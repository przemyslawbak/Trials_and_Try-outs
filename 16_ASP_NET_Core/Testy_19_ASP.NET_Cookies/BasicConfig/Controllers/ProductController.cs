using BasicConfig.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

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
            CookieOptions option = new CookieOptions();
            option.Expires = DateTime.Now.AddMinutes(10);

            int x = 0;
            if (Request.Cookies["Counter"] != null)
            {
                x = int.Parse(Request.Cookies["Counter"]);
            }
            x++;
            Response.Cookies.Append("Counter", x.ToString(), option);

            return View(x);
        }
    }
}
