using System;
using System.IO;
using Microsoft.ML;

namespace MS_Tutorial
{
    //https://docs.microsoft.com/en-us/dotnet/machine-learning/tutorials/predict-prices
    class Program
    {
        static readonly string _trainDataPath = Path.Combine(Environment.CurrentDirectory, "Data", "taxi-fare-train.csv");
        static readonly string _testDataPath = Path.Combine(Environment.CurrentDirectory, "Data", "taxi-fare-test.csv");
        static readonly string _modelPath = Path.Combine(Environment.CurrentDirectory, "Data", "Model.zip");

        static void Main(string[] args)
        {
            MLContext mlContext = new MLContext(seed: 0);
            ITransformer model = Train(mlContext, _trainDataPath);
            Evaluate(mlContext, model);
            TestSinglePrediction(mlContext, model);
        }

        /// <summary>
        /// The Train() method executes the following tasks:
        ///Loads the data.
        ///Extracts and transforms the data.
        ///Trains the model.
        ///Returns the model.

        /// </summary>
        public static ITransformer Train(MLContext mlContext, string dataPath)
        {
            IDataView dataView = mlContext.Data.LoadFromTextFile<TaxiTrip>(dataPath, hasHeader: true, separatorChar: ',');
            Microsoft.ML.Data.EstimatorChain<Microsoft.ML.Data.RegressionPredictionTransformer<Microsoft.ML.Trainers.PoissonRegressionModelParameters>> pipeline = mlContext.Transforms.CopyColumns(outputColumnName: "Label", inputColumnName: "FareAmount")
                .Append(mlContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "VendorIdEncoded", inputColumnName: "VendorId"))
                .Append(mlContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "RateCodeEncoded", inputColumnName: "RateCode"))
                .Append(mlContext.Transforms.Categorical.OneHotEncoding(outputColumnName: "PaymentTypeEncoded", inputColumnName: "PaymentType"))
                .Append(mlContext.Transforms.Concatenate("Features", "VendorIdEncoded", "RateCodeEncoded", "PassengerCount", "TripDistance", "PaymentTypeEncoded"))
                .Append(mlContext.Regression.Trainers.LbfgsPoissonRegression());
            Microsoft.ML.Data.TransformerChain<Microsoft.ML.Data.RegressionPredictionTransformer<Microsoft.ML.Trainers.PoissonRegressionModelParameters>> model = pipeline.Fit(dataView);

            return model;
        }

        /// <summary>
        /// The Evaluate method executes the following tasks:
        ///Loads the test dataset.
        ///Creates the regression evaluator.
        ///Evaluates the model and creates metrics.
        ///Displays the metrics.
        /// </summary>
        private static void Evaluate(MLContext mlContext, ITransformer model)
        {
            IDataView dataView = mlContext.Data.LoadFromTextFile<TaxiTrip>(_testDataPath, hasHeader: true, separatorChar: ',');
            IDataView predictions = model.Transform(dataView);
            Microsoft.ML.Data.RegressionMetrics metrics = mlContext.Regression.Evaluate(predictions, "Label", "Score");

            Console.WriteLine();
            Console.WriteLine($"*************************************************");
            Console.WriteLine($"*       Model quality metrics evaluation         ");
            Console.WriteLine($"*------------------------------------------------");

            Console.WriteLine($"*       RSquared Score:      {metrics.RSquared:0.##}");
            Console.WriteLine($"*       Root Mean Squared Error:      {metrics.RootMeanSquaredError:#.##}");
        }

        /// <summary>
        /// The TestSinglePrediction method executes the following tasks:
        ///Creates a single comment of test data.
        ///Predicts fare amount based on test data.
        ///Combines test data and predictions for reporting.
        ///Displays the predicted results.
        /// </summary>
        private static void TestSinglePrediction(MLContext mlContext, ITransformer model)
        {
            PredictionEngine<TaxiTrip, TaxiTripFarePrediction> predictionFunction = mlContext.Model.CreatePredictionEngine<TaxiTrip, TaxiTripFarePrediction>(model);
            TaxiTrip taxiTripSample = new TaxiTrip()
            {
                VendorId = "VTS",
                RateCode = "1",
                PassengerCount = 1,
                TripTime = 1140,
                TripDistance = 3.75f,
                PaymentType = "CRD",
                FareAmount = 0 // To predict. Actual/Observed = 15.5
            };
            var prediction = predictionFunction.Predict(taxiTripSample);
            Console.WriteLine($"**********************************************************************");
            Console.WriteLine($"Predicted fare: {prediction.FareAmount:0.####}, actual fare: 15.5");
            Console.WriteLine($"Predicted probability: {prediction.Probability:0.####}");
            Console.WriteLine($"**********************************************************************");
        }
    }
}
