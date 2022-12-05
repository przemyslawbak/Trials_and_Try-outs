using Microsoft.AspNetCore.Mvc;
using Skender.Stock.Indicators;
using System;
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
            List<Quote> quotes = _taCalculator.GetQuotes();
            DisplayViewModel model = new DisplayViewModel();
            List<SmaResult> smaResults = quotes.GetSma(10).ToList();
            List<decimal> stockPrices = _taCalculator.GetPrices();
            model.SmaResults = smaResults.Skip(Math.Max(0, smaResults.Count - 50)).ToList();
            model.StockPrices = stockPrices.Skip(Math.Max(0, stockPrices.Count - 50)).ToList();
            model.Dates = _taCalculator.GetDates();

            return View(model);
        }
    }
}
