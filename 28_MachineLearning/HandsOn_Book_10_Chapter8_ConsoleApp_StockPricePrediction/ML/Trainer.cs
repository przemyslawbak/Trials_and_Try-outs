using System;
using System.IO;

using chapter08.ML.Base;
using chapter08.ML.Objects;
using chapter08.Objects;

using Microsoft.ML;
using Microsoft.ML.Transforms.TimeSeries;

namespace chapter08.ML
{
    public class Trainer : BaseML
    {
        //1. First, update the function prototype to take the ProgramArguments object
        public void Train(ProgramArguments arguments)
        {
            //2. Next, we update the training file check to utilize the argument object
            if (!File.Exists(arguments.TrainingFileName))
            {
                Console.WriteLine($"Failed to find training data file ({arguments.TrainingFileName})");

                return;
            }

            //3. Similarly, we then update the testing file check to utilize the argument object
            if (!File.Exists(arguments.TestingFileName))
            {
                Console.WriteLine($"Failed to find test data file ({arguments.TestingFileName})");

                return;
            }

            //4. Next, we load the StockPrices values from the training file
            IDataView dataView = MlContext.Data.LoadFromTextFile<StockPrices>(arguments.TrainingFileName);

            //5. We then create the Forecasting object and utilize the nameof C# feature to
            //avoid magic string references
            SsaForecastingEstimator model = MlContext.Forecasting.ForecastBySsa(
                outputColumnName: nameof(StockPrediction.StockForecast),
                inputColumnName: nameof(StockPrices.StockPrice), 
                windowSize: 7, 
                seriesLength: 30, 
                trainSize: 24, 
                horizon: 5,
                confidenceLevel: 0.95f,
                confidenceLowerBoundColumn: nameof(StockPrediction.LowerBound),
                confidenceUpperBoundColumn: nameof(StockPrediction.UpperBound));

            //6. Lastly, we transform the model with the training data, call the
            //CreateTimeSeriesEngine method, and write the model to disk
            SsaForecastingTransformer transformer = model.Fit(dataView);

            TimeSeriesPredictionEngine<StockPrices, StockPrediction> forecastEngine = transformer.CreateTimeSeriesEngine<StockPrices, StockPrediction>(MlContext);

            forecastEngine.CheckPoint(MlContext, arguments.ModelFileName);

            Console.WriteLine($"Wrote model to {arguments.ModelFileName}");
        }
    }
}