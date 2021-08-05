using Financial_ML.Models;
using Skender.Stock.Indicators;
using System.Collections.Generic;

namespace Financial_ML.Services
{
    public interface IDataSplitter
    {
        List<TotalQuote> GetAllTotalQuotes(List<Quote> quotesDax, List<Quote> quotesBrent);
    }
}