using HelloML.App;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms.TimeSeries;
using System.Data.SqlClient;

namespace Sample
{
    //https://learn.microsoft.com/en-us/dotnet/machine-learning/tutorials/time-series-demand-forecasting

    internal class Program
    {
        private static readonly string _rootDir = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../"));
        private static readonly string _dbFilePath = Path.Combine(_rootDir, "Data", "DailyDemand.mdf");
        private static readonly string _modelPath = Path.Combine(_rootDir, "Data", "MLModel.zip");
        private static readonly string _connectionString = $"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename={_dbFilePath};Integrated Security=True;Connect Timeout=30;";

        static void Main(string[] args)
        {
            //Initialize the mlContext variable
            MLContext mlContext = new MLContext();

            //Create DatabaseLoader that loads records of type ModelInput.
            DatabaseLoader loader = mlContext.Data.CreateDatabaseLoader<ModelInput>();

            //Define the query to load the data from the database.
            string query = "SELECT RentalDate, CAST(Year as REAL) as Year, CAST(TotalRentals as REAL) as TotalRentals FROM Rentals";

            //Create a DatabaseSource to connect to the database and execute the query
            DatabaseSource dbSource = new DatabaseSource(SqlClientFactory.Instance,
                                _connectionString,
                                query);

            //Load the data into an IDataView
            IDataView dataView = loader.Load(dbSource);

            //The dataset contains two years worth of data
            IDataView firstYearData = mlContext.Data.FilterRowsByColumn(dataView, "Year", upperBound: 1);
            IDataView secondYearData = mlContext.Data.FilterRowsByColumn(dataView, "Year", lowerBound: 1);

            //Define time series analysis pipeline
            var forecastingPipeline = mlContext.Forecasting.ForecastBySsa(
                outputColumnName: "ForecastedRentals",
                inputColumnName: "TotalRentals",
                windowSize: 7,
                seriesLength: 30,
                trainSize: 365,
                horizon: 7,
                confidenceLevel: 0.95f,
                confidenceLowerBoundColumn: "LowerBoundRentals",
                confidenceUpperBoundColumn: "UpperBoundRentals");

            //Use the Fit method to train the model and fit the data to the previously defined forecastingPipeline.
            SsaForecastingTransformer forecaster = forecastingPipeline.Fit(firstYearData);

            //Evaluate model
            Evaluate(secondYearData, forecaster, mlContext);

            //Save the model
            var forecastEngine = forecaster.CreateTimeSeriesEngine<ModelInput, ModelOutput>(mlContext);
            forecastEngine.CheckPoint(mlContext, _modelPath);

            //Forecast demand
            Forecast(secondYearData, 7, forecastEngine, mlContext);
        }


        private static void Evaluate(IDataView testData, ITransformer model, MLContext mlContext)
        {
            //Inside the Evaluate method, forecast the second year's data by using the Transform method with the trained model.
            IDataView predictions = model.Transform(testData);

            IEnumerable<float> actual =
                mlContext.Data.CreateEnumerable<ModelInput>(testData, true)
                    .Select(observed => observed.TotalRentals);

            IEnumerable<float> forecast =
                mlContext.Data.CreateEnumerable<ModelOutput>(predictions, true)
                    .Select(prediction => prediction.ForecastedRentals[0]);

            var metrics = actual.Zip(forecast, (actualValue, forecastValue) => actualValue - forecastValue);

            var MAE = metrics.Average(error => Math.Abs(error)); // Mean Absolute Error
            var RMSE = Math.Sqrt(metrics.Average(error => Math.Pow(error, 2))); // Root Mean Squared Error

            Console.WriteLine("Evaluation Metrics");
            Console.WriteLine("---------------------");
            Console.WriteLine($"Mean Absolute Error: {MAE:F3}");
            Console.WriteLine($"Root Mean Squared Error: {RMSE:F3}\n");
        }

        private static void Forecast(IDataView testData, int horizon, TimeSeriesPredictionEngine<ModelInput, ModelOutput> forecaster, MLContext mlContext)
        {
            ModelOutput forecast = forecaster.Predict();

            IEnumerable<string> forecastOutput =
                mlContext.Data.CreateEnumerable<ModelInput>(testData, reuseRowObject: false)
                    .Take(horizon)
                    .Select((ModelInput rental, int index) =>
                    {
                        string rentalDate = rental.RentalDate.ToShortDateString();
                        float actualRentals = rental.TotalRentals;
                        float lowerEstimate = Math.Max(0, forecast.LowerBoundRentals[index]);
                        float estimate = forecast.ForecastedRentals[index];
                        float upperEstimate = forecast.UpperBoundRentals[index];
                        return $"Date: {rentalDate}\n" +
                        $"Actual Rentals: {actualRentals}\n" +
                        $"Lower Estimate: {lowerEstimate}\n" +
                        $"Forecast: {estimate}\n" +
                        $"Upper Estimate: {upperEstimate}\n";
                    });

            Console.WriteLine("Rental Forecast");
            Console.WriteLine("---------------------");
            foreach (var prediction in forecastOutput)
            {
                Console.WriteLine(prediction);
            }
        }
    }
}