using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using React_01_Podrecznik.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace React_01_Podrecznik.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context; //database context
        public HomeController(ApplicationDbContext context)
        {
            _context = context; //context for controller
        }
        public ViewResult Index()
        {
            var dbData = _context.Products;
            return View(dbData);
        }

        [Route("comments")]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public ActionResult Comments()
        {
            var _comments = _context.Products.OrderBy(c => c.ProductID).ToList();
            return Json(_comments);
        }

        [Route("comments/new")]
        public ActionResult AddComment(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
            return Content("Success :)");
        }

        [Route("comments/delete")]
        public ActionResult DeleteComment(int productID)
        {
            var products = _context.Products;
            foreach(var item in products.ToList())
            {
                if(item.ProductID == productID)
                {
                    products.Remove(item);
                    _context.SaveChanges();
                }
            }
            
            return Content("Success :)");
        }
    }
}
