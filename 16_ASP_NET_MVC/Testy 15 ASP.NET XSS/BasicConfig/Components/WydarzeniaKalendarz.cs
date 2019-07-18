using BasicConfig.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasicConfig.Components
{
    public class WydarzeniaKalendarz : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        public WydarzeniaKalendarz(ApplicationDbContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var wydarzeniaData = _context.ColorPanels.FirstOrDefault();
            ViewBag.Background1 = wydarzeniaData.Element1BackgroundColor;
            ViewBag.Background2 = wydarzeniaData.Element2BackgroundColor;
            return View(wydarzeniaData);
        }
    }
}
