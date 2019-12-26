using BasicConfig.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasicConfig.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        public ViewResult Index()
        {
            ColorPanel dataBase = _context.ColorPanels.FirstOrDefault();
            if (dataBase == null)
            {
                dataBase = new ColorPanel
                {
                    Element1BackgroundColor = "blue",
                    Element2BackgroundColor = "blue",
                    Element3BackgroundColor = "blue"
                };
                _context.ColorPanels.Add(dataBase);
                _context.SaveChanges();
            }
            ViewBag.Element1BackgroundColor = dataBase.Element1BackgroundColor;
            ViewBag.Element2BackgroundColor = dataBase.Element2BackgroundColor;
            ViewBag.Element3BackgroundColor = dataBase.Element3BackgroundColor;
            return View();
        }
    }
}
