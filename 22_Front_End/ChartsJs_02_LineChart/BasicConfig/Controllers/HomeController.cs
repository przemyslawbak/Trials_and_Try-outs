using BasicConfig.Models;
using BasicConfig.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace BasicConfig.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public ViewResult Index()
        {
            return View(GetFakeDailyStats());
        }

        private List<StatsViewModel> GetFakeDailyStats()
        {
            return new List<StatsViewModel>()
            {
                new StatsViewModel() { Date = DateTime.UtcNow.AddDays(-0), Expired = 31231, Missing = 21312, Moving = 13943, NotMoving = 12532 },
                new StatsViewModel() { Date = DateTime.UtcNow.AddDays(-1), Expired = 21231, Missing = 31312, Moving = 14943, NotMoving = 11532 },
                new StatsViewModel() { Date = DateTime.UtcNow.AddDays(-2), Expired = 28231, Missing = 24312, Moving = 11943, NotMoving = 14532 },
                new StatsViewModel() { Date = DateTime.UtcNow.AddDays(-3), Expired = 31231, Missing = 21312, Moving = 13943, NotMoving = 12532 },
                new StatsViewModel() { Date = DateTime.UtcNow.AddDays(-4), Expired = 24231, Missing = 28312, Moving = 10943, NotMoving = 15532 },
                new StatsViewModel() { Date = DateTime.UtcNow.AddDays(-5), Expired = 26231, Missing = 26312, Moving = 19943, NotMoving = 6532 }
            };
        }
    }
}
