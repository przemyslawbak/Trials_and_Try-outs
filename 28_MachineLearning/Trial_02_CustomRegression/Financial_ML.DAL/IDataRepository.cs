using System;
using System.Collections.Generic;

namespace Financial_ML.DAL
{
    public interface IDataRepository
    {
        List<global::Skender.Stock.Indicators.Quote> GetDaxQuotes();
        List<global::Skender.Stock.Indicators.Quote> GetBrentQuotes();
    }
}