using Skender.Stock.Indicators;
using System.Collections.Generic;

namespace Trial.Services
{
    public interface IFileReader
    {
        List<Quote> GetHistory();
    }
}