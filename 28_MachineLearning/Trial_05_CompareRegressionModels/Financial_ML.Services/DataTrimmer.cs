using Financial_ML.ViewModels;
using System;
using System.Linq;

namespace Financial_ML.Services
{
    public class DataTrimmer : IDataTrimmer
    {
        public ResultsDisplay TrimList(ResultsDisplay display, int records)
        {
            return new ResultsDisplay()
            {
                AllTotalQuotes = display.AllTotalQuotes.Skip(Math.Max(0, display.AllTotalQuotes.Count - 50)).ToList()
            };
        }
    }
}
