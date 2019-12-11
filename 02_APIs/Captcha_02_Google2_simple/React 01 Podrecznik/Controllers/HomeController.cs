using Microsoft.AspNetCore.Mvc;
using React_01_Podrecznik.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace React_01_Podrecznik.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(Comment comment)
        {
            if (!ModelState.IsValid)
            {
                //TODO: Comment saving logic here
                return View();
            }
            return View();
        }
    }
}
