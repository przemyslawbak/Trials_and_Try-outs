using Microsoft.ML;
using Microsoft.ML.Transforms.TimeSeries;
using Rental.Models;

namespace Taxi.AppConsumingTrainedModel
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Create MLContext and loading pre-trained model
            MLContext mlContext = new MLContext();
            var trainedModel = mlContext.Model.Load(@"..\..\..\..\Sample\Data\RentalForecast.Model.zip", out var modelInputSchema);

            //creating forecast engine
            var forecastEngine = trainedModel.CreateTimeSeriesEngine<RentalData, RentalPrediction>(mlContext);

            //make a prediction
            var horizon = 5;
            RentalData latest = new RentalData()
            {
                RentalDate = DateTime.UtcNow,
                TotalRentals = 666,
                Year = 2024
            };
            var prediction = forecastEngine.Predict(latest, horizon);
        }
    }
}
