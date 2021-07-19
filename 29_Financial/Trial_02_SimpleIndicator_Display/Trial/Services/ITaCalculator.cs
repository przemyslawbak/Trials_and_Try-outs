using Skender.Stock.Indicators;
using System.Collections.Generic;

namespace Trial.Services
{
    public interface ITaCalculator
    {
        List<Quote> GetQuotes();
        List<decimal> GetPrices();
    }
}