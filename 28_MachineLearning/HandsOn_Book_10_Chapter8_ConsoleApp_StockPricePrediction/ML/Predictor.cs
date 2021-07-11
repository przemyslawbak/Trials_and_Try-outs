using System;
using System.IO;
using System.Linq;

using chapter08.ML.Base;
using chapter08.ML.Objects;
using chapter08.Objects;

using Microsoft.ML;
using Microsoft.ML.Transforms.TimeSeries;

namespace chapter08.ML
{
    public class Predictor : BaseML
    {
        //1. First, we adjust the Predict method to accept the newly defined
        //ProgramArguments class object
        public void Predict(ProgramArguments arguments)
        {
            //2. Next, we update the model file.Exists check to utilize the arguments object
            if (!File.Exists(arguments.ModelFileName))
            {
                Console.WriteLine($"Failed to find model at {arguments.ModelFileName}");

                return;
            }

            //3. Similarly, we also update the prediction filename reference to the utilize the
            //arguments object
            if (!File.Exists(arguments.PredictionFileName))
            {
                Console.WriteLine($"Failed to find input data at {arguments.PredictionFileName}");

                return;
            }

            ITransformer mlModel;

            //4. Next, we also modify the model open call to utilize the arguments object
            using (var stream = new FileStream(Path.Combine(AppContext.BaseDirectory, arguments.ModelFileName), FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                mlModel = MlContext.Model.Load(stream, out _);
            }

            if (mlModel == null)
            {
                Console.WriteLine("Failed to load model");

                return;
            }

            //5. We then create the Time Series Engine object with our StockPrices and
            //StockPrediction types
            TimeSeriesPredictionEngine<StockPrices, StockPrediction> predictionEngine = mlModel.CreateTimeSeriesEngine<StockPrices, StockPrediction>(MlContext);

            //6. Next, we read the stock price prediction file into a string array
            string[] stockPrices = File.ReadAllLines(arguments.PredictionFileName);

            //7. Lastly, we iterate through each input, call the prediction engine, and display the
            //estimated values
            foreach (var stockPrice in stockPrices)
            {
                StockPrediction prediction = predictionEngine.Predict(new StockPrices(Convert.ToSingle(stockPrice)));

                Console.WriteLine($"Given a stock price of ${stockPrice}, the next 5 values are predicted to be: " +
                                  $"{string.Join(", ", prediction.StockForecast.Select(a => $"${Math.Round(a)}"))}");
            }
        }
    }
}