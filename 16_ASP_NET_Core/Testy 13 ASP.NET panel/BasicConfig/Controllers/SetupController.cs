using BasicConfig.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasicConfig.Controllers
{
    public class SetupController : Controller
    {
        private readonly ApplicationDbContext _context;
        public SetupController(ApplicationDbContext context)
        {
            _context = context;
        }

        public ActionResult Edit()
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
            return View(dataBase);
        }
        [HttpPost, ActionName("Edit")]
        public IActionResult Edit(ColorPanel modelReturned)
        {
            ColorPanel dataBase = _context.ColorPanels.FirstOrDefault();
            dataBase.Element1BackgroundColor = modelReturned.Element1BackgroundColor;
            dataBase.Element2BackgroundColor = modelReturned.Element2BackgroundColor;
            dataBase.Element3BackgroundColor = modelReturned.Element3BackgroundColor;
            _context.SaveChanges();
            return RedirectToAction("Index", "Product");
        }
    }
}
