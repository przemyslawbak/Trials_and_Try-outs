﻿using BasicConfig.Models;
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
            if (TempData["Counter"] != null)
            {
                x = int.Parse(TempData["Counter"] as string);
            }
            x++;
            TempData["Counter"] = x.ToString();

            return View(x);
        }
    }
}
