using Microsoft.AspNetCore.Mvc;
using Skender.Stock.Indicators;
using System.Collections.Generic;
using System.Linq;
using Trial.Models;
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

        public IActionResult Index() //TODO: refactor for DRY
        {
            DisplayViewModel model = new DisplayViewModel();
            List<Quote> quotes = _taCalculator.GetQuotes();
            model.SmaResults = quotes.GetSma(10).ToList();
            model.StockPrices = _taCalculator.GetPrices();

            return View(model);
        }
    }
}
