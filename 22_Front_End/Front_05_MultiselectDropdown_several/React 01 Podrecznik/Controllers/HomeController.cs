﻿using Microsoft.AspNetCore.Mvc;
using React_01_Podrecznik.Models;
using System.Collections.Generic;

namespace React_01_Podrecznik.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index(MultiselectViewModel model)
        {
            List<string> vesselTypes = new List<string>() { "bulk", "tug", "yacht", "tanker", "supply" };
            List<string> vesselFlags = new List<string>() { "belize", "china", "norway", "cyprus", "poland" };
            List<string> dupa = new List<string>() { "belize", "nikaragua", "norway", "cyprus", "poland" };

            ViewBag.VesselTypes = vesselTypes;
            ViewBag.VesselFlags = vesselFlags;
            ViewBag.Dupa = dupa;

            return View(model);
        }
    }
}
