using Microsoft.AspNetCore.Mvc;
using Skender.Stock.Indicators;
using System.Collections.Generic;
using System.Linq;
using Trial.Services;

namespace Trial.Controllers
{
    public class HomeController : Controller
    {
        private readonly IFileReader _fileReader;
        private readonly ITaCalculator _taCalculator;

        public HomeController(IFileReader fileReader, ITaCalculator taCalculator)
        {
            _fileReader = fileReader;
            _taCalculator = taCalculator;
        }

        public IActionResult Index()
        {
            List<Quote> history = _fileReader.GetHistory();
            List<Quote> quotes = _taCalculator.GetQuotes(history);
            List<SmaResult> results = quotes.GetSma(10).ToList();

            return View();
        }
    }
}
