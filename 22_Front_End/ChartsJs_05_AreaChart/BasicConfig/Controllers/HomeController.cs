using BasicConfig.Models;
using BasicConfig.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

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
            List<StatsViewModel> list = new List<StatsViewModel>()
            {
                new StatsViewModel() { Date = DateTime.UtcNow.AddDays(-0), Expired = 19893, Missing = 34953, Moving = 9019, NotMoving = 6786 },
                new StatsViewModel() { Date = DateTime.UtcNow.AddDays(-1), Expired = 19834, Missing = 34888, Moving = 9247, NotMoving = 6682 },
                new StatsViewModel() { Date = DateTime.UtcNow.AddDays(-2), Expired = 19835, Missing = 34798, Moving = 9263, NotMoving = 6661 },
                new StatsViewModel() { Date = DateTime.UtcNow.AddDays(-3), Expired = 19835, Missing = 34798, Moving = 9463, NotMoving = 6461 },
                new StatsViewModel() { Date = DateTime.UtcNow.AddDays(-4), Expired = 19835, Missing = 34798, Moving = 9363, NotMoving = 6561 },
                new StatsViewModel() { Date = DateTime.UtcNow.AddDays(-5), Expired = 19835, Missing = 34798, Moving = 9263, NotMoving = 6661 }
            };
            return list.OrderBy(s => s.Date).ToList();
        }
    }
}
