using Financial_ML.DAL;
using Financial_ML.ViewModels;
using Skender.Stock.Indicators;
using System.Collections.Generic;

namespace Financial_ML.Services
{
    public class DataProvider : IDataProvider
    {
        private readonly IDataRepository _dataRepo;
        private readonly IDataSplitter _dataSplitter;

        public DataProvider(IDataRepository dataRepo, IDataSplitter dataSplitter)
        {
            _dataRepo = dataRepo;
            _dataSplitter = dataSplitter;
        }

        public ResultsDisplay GetResultsDisplayViewModel()
        {
            List<Quote> quotesDax = _dataRepo.GetDaxQuotes();
            List<Quote> quotesBrent = _dataRepo.GetBrentQuotes();

            return new ResultsDisplay()
            {
                AllTotalQuotes = _dataSplitter.GetAllTotalQuotes(quotesDax, quotesBrent)
            };
        }
    }
}
