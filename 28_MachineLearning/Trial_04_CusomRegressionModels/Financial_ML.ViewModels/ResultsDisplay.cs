using Financial_ML.Models;
using Skender.Stock.Indicators;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Financial_ML.ViewModels
{
    public class ResultsDisplay
    {
        public List<TotalQuote> AllTotalQuotes { get; set; }

        public object Skip(object p)
        {
            throw new NotImplementedException();
        }
    }
}
