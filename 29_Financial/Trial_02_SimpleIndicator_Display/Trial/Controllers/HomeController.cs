using Microsoft.AspNetCore.Mvc;
using Skender.Stock.Indicators;
using System.Collections.Generic;
using System.Linq;
using Trial.Services;

namespace Trial.Controllers
{
    public class HomeController : Controller
    {
        private readonly ITaCalculator _taCalculator;

        public HomeController(ITaCalculator taCalculator)
        {
            _taCalculator = taCalculator;
        }

        public IActionResult Index()
        {
            List<Quote> quotes = _taCalculator.GetQuotes(history);
            List<SmaResult> results = quotes.GetSma(10).ToList();

            return View();
        }
    }
}
