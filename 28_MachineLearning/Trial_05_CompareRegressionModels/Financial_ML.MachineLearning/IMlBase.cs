using Financial_ML.Models;
using Microsoft.ML;
using System.Collections.Generic;

namespace Financial_ML.MachineLearning
{
    public interface IMlBase
    {
        MLContext GetMlContext();
        IDataView GetDataViewFromEnumerable(List<TotalQuote> allTotalQuotes, MLContext context);
        DataOperationsCatalog.TrainTestData GetTestData(MLContext context, IDataView data);
    }
}