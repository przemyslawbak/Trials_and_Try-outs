using Financial_ML.ViewModels;

namespace Financial_ML.Services
{
    public interface IDataTrimmer
    {
        ResultsDisplay TrimList(ResultsDisplay display, int v);
    }
}