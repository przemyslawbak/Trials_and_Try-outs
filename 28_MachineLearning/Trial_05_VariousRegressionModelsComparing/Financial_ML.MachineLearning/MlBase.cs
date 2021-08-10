using Financial_ML.Models;
using Microsoft.ML;
using System.Collections.Generic;

namespace Financial_ML.MachineLearning
{
    public class MlBase : IMlBase
    {
        public IDataView GetDataViewFromEnumerable(List<TotalQuote> allTotalQuotes, MLContext context)
        {
            return context.Data.LoadFromEnumerable(allTotalQuotes);
        }

        public MLContext GetMlContext()
        {
            return new MLContext(seed: 0);
        }

        public DataOperationsCatalog.TrainTestData GetTestData(MLContext context, IDataView data)
        {
            return context.Data.TrainTestSplit(data, testFraction: 0.2, seed: 0);
        }
    }
}
