using Financial_ML.ViewModels;

namespace Financial_ML.Services
{
    public interface IDataProvider
    {
        ResultsDisplay GetResultsDisplayViewModel();
    }
}