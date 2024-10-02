using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using Microsoft.ML.Transforms.TimeSeries;
using Rental.Models;
using System.Data.SqlClient;

namespace Sample
{
    internal class Program
    {
        private static readonly string _dataPath = Path.GetFullPath(@"..\..\..\Data\DailyDemand.mdf");
        private static readonly string _outputPath = Path.GetFullPath(@"..\..\..\Data\RentalForecast.Model.zip");
        private static readonly string _connectionString = $"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename={_dataPath};Integrated Security=True;Connect Timeout=30;";

        static void Main(string[] args)
        {
            var mlContext = new MLContext();

            //load from mdf file
            DatabaseLoader loader = mlContext.Data.CreateDatabaseLoader<RentalData>();
            var query = "SELECT RentalDate, CAST(Year as REAL) as Year, CAST(TotalRentals as REAL) as TotalRentals FROM Rentals";
            DatabaseSource dbSource = new DatabaseSource(SqlClientFactory.Instance, _connectionString, query);
            IDataView dataView = loader.Load(dbSource);

            //split for training and testing sets
            IDataView firstYearData = mlContext.Data.FilterRowsByColumn(dataView, "Year", upperBound: 1);
            IDataView secondYearData = mlContext.Data.FilterRowsByColumn(dataView, "Year", lowerBound: 1);

            //train and evaluate
            TrainEvaluateSaveModel(mlContext, firstYearData, secondYearData, _outputPath);
        }

        private static void TrainEvaluateSaveModel(MLContext mlContext, IDataView firstYearData, IDataView secondYearData, string outputPath)
        {
            var forecastingPipeline = mlContext.Forecasting.ForecastBySsa(
                outputColumnName: "ForecastedRentals", //looking for
                inputColumnName: "TotalRentals",
                windowSize: 7,
                seriesLength: 30,
                trainSize: 365,
                horizon: 7, //forecast for next n days
                rankSelectionMethod: RankSelectionMethod.Fixed,
                confidenceLevel: 0.95f,
                confidenceLowerBoundColumn: "LowerBoundRentals",
                confidenceUpperBoundColumn: "UpperBoundRentals");
            SsaForecastingTransformer forecaster = forecastingPipeline.Fit(firstYearData); //train

            // Save model to disk
            mlContext.Model.Save(forecaster, secondYearData.Schema, outputPath);

            // Evaluate performance
            Evaluate(firstYearData, forecaster, mlContext);

            Console.WriteLine("The output is saved to {0}\n\n", outputPath);
        }

        private static void Evaluate(IDataView testData, ITransformer model, MLContext mlContext)
        {
            // Make predictions
            IDataView predictions = model.Transform(testData);

            // Actual values
            IEnumerable<float> actual = mlContext.Data
                .CreateEnumerable<RentalData>(testData, true)
                    .Select(observed => observed.TotalRentals);

            // Predicted values
            IEnumerable<float> forecast = mlContext.Data
                .CreateEnumerable<RentalPrediction>(predictions, true)
                .Select(prediction => prediction.ForecastedRentals[0]);

            // Calculate error (actual - forecast)
            var metrics = actual.Zip(forecast, (actualValue, forecastValue) => actualValue - forecastValue).ToArray();

            // Get metric averages
            var MAE = metrics.Average(error => Math.Abs(error)); // Mean Absolute Error
            var RMSE = Math.Sqrt(metrics.Average(error => Math.Pow(error, 2))); // Root Mean Squared Error

            // Output metrics
            Console.WriteLine("Evaluation Metrics");
            Console.WriteLine("---------------------");
            Console.WriteLine($"Mean Absolute Error: {MAE:F3}");
            Console.WriteLine($"Root Mean Squared Error: {RMSE:F3}\n");
        }
    }
}