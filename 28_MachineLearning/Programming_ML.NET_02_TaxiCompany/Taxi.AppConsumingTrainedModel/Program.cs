using Microsoft.Extensions.ML;
using Microsoft.ML;
using Taxi.Models;

namespace Taxi.AppConsumingTrainedModel
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //https://learn.microsoft.com/en-us/dotnet/machine-learning/how-to-guides/machine-learning-model-predictions-ml-net
            //Create MLContext
            MLContext mlContext = new MLContext();

            // Load Trained Model
            DataViewSchema predictionPipelineSchema;
            ITransformer predictionPipeline = mlContext.Model.Load(@"..\..\..\..\Sample\Data\SampleTaxi.Fare.zip", out predictionPipelineSchema);

            int i;
            int pax = 0;
            int dist = 0;
            int time = 0;

            //inputs
            Console.WriteLine("How many passenger will you take?");
            if (int.TryParse(Console.ReadLine(), out i))
            {
                pax = i;
            }

            Console.WriteLine("Write trip distance in km.");
            if (int.TryParse(Console.ReadLine(), out i))
            {
                dist = i;
            }

            Console.WriteLine("Write trip time in min.");
            if (int.TryParse(Console.ReadLine(), out i))
            {
                time = i * 60;
            }

            var testTrip = new TaxiTrip()
            {
                VendorId = "VTS",
                RateCode = "1",
                PassengerCount = pax,
                PaymentType = "CRD",
                TripDistance = dist,
                TripTime = time,
            };

            // Create PredictionEngines
            PredictionEngine<TaxiTrip, TaxiFarePrediction> predictionEngine = mlContext.Model.CreatePredictionEngine<TaxiTrip, TaxiFarePrediction>(predictionPipeline);
            
            //predict
            var predictedFee = predictionEngine.Predict(testTrip).FareAmount;
        }
    }
}
