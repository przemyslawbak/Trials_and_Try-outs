using System;
using System.IO;
using Microsoft.ML;
using System.Collections.Generic;

namespace MS_Tutorial
{
    //https://docs.microsoft.com/en-us/dotnet/machine-learning/tutorials/sales-anomaly-detection
    class Program
    {
        static readonly string _dataPath = Path.Combine(Environment.CurrentDirectory, "Data", "product-sales.csv");
        //assign the Number of records in dataset file to constant variable
        const int _docsize = 36;

        [Obsolete]
        static void Main(string[] args)
        {
            MLContext mlContext = new MLContext();
            IDataView dataView = mlContext.Data.LoadFromTextFile<ProductSalesData>(path: _dataPath, hasHeader: true, separatorChar: ',');
            DetectSpike(mlContext, _docsize, dataView);
            DetectChangepoint(mlContext, _docsize, dataView);
        }

        static IDataView CreateEmptyDataView(MLContext mlContext)
        {
            // Create empty DataView. We just need the schema to call Fit() for the time series transforms
            IEnumerable<ProductSalesData> enumerableData = new List<ProductSalesData>();
            return mlContext.Data.LoadFromEnumerable(enumerableData);
        }

        [Obsolete]
        static void DetectSpike(MLContext mlContext, int docSize, IDataView productSales)
        {
            Microsoft.ML.Transforms.TimeSeries.IidSpikeEstimator iidSpikeEstimator = mlContext.Transforms.DetectIidSpike(outputColumnName: nameof(ProductSalesPrediction.Prediction), inputColumnName: nameof(ProductSalesData.numSales), confidence: 95, pvalueHistoryLength: docSize / 4);
            ITransformer iidSpikeTransform = iidSpikeEstimator.Fit(CreateEmptyDataView(mlContext));
            IDataView transformedData = iidSpikeTransform.Transform(productSales);
            IEnumerable<ProductSalesPrediction> predictions = mlContext.Data.CreateEnumerable<ProductSalesPrediction>(transformedData, reuseRowObject: false);
            foreach (var p in predictions)
            {
                string results = $"{p.Prediction[0]}\t{p.Prediction[1]:f2}\t{p.Prediction[2]:F2}";

                if (p.Prediction[0] == 1)
                {
                    results += " <-- Spike detected";
                }

                Console.WriteLine(results);
            }
            Console.WriteLine("");
        }

        [Obsolete]
        static void DetectChangepoint(MLContext mlContext, int docSize, IDataView productSales)
        {
            var iidChangePointEstimator = mlContext.Transforms.DetectIidChangePoint(outputColumnName: nameof(ProductSalesPrediction.Prediction), inputColumnName: nameof(ProductSalesData.numSales), confidence: 95, changeHistoryLength: docSize / 4);
            var iidChangePointTransform = iidChangePointEstimator.Fit(CreateEmptyDataView(mlContext));
            IDataView transformedData = iidChangePointTransform.Transform(productSales);
            var predictions = mlContext.Data.CreateEnumerable<ProductSalesPrediction>(transformedData, reuseRowObject: false);
            Console.WriteLine("Alert\tScore\tP-Value\tMartingale value");

            foreach (var p in predictions)
            {
                var results = $"{p.Prediction[0]}\t{p.Prediction[1]:f2}\t{p.Prediction[2]:F2}\t{p.Prediction[3]:F2}";

                if (p.Prediction[0] == 1)
                {
                    results += " <-- alert is on, predicted changepoint";
                }
                Console.WriteLine(results);
            }
            Console.WriteLine("");
        }
    }
}
