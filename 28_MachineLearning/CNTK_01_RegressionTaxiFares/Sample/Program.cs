using System;
using System.IO;
using System.Linq;
using Microsoft.ML;
using Microsoft.ML.Data;
using CNTK;

namespace Sample
{
    //https://github.com/mdfarragher/DLR/tree/master/Regression/TaxiFarePrediction

    /// <summary>
    /// The TaxiTrip class represents a single taxi trip.
    /// </summary>
    public class TaxiTrip
    {
        [LoadColumn(0)] public float VendorId;
        [LoadColumn(5)] public float RateCode;
        [LoadColumn(3)] public float PassengerCount;
        [LoadColumn(4)] public float TripDistance;
        [LoadColumn(9)] public float PaymentType;
        [LoadColumn(10)] public float FareAmount;

        public float[] GetFeatures() => new float[] { VendorId, RateCode, PassengerCount, TripDistance, PaymentType };

        public float GetLabel() => FareAmount;
    }

    /// <summary>
    /// The program class.
    /// </summary>
    internal class Program
    {
        // file paths to data files
        private static readonly string dataPath = Path.GetFullPath(@"..\..\..\Data\yellow_tripdata_2018-12.csv");

        /// <summary>
        /// The main application entry point.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        static void Main(string[] args)
        {
            // create the machine learning context
            var context = new MLContext();

            // set up the text loader 
            var textLoader = context.Data.CreateTextLoader(
                new TextLoader.Options()
                {
                    Separators = new[] { ',' },
                    HasHeader = true,
                    Columns = new[]
                    {
                        new TextLoader.Column("VendorId", DataKind.Single, 0),
                        new TextLoader.Column("RateCode", DataKind.Single, 5),
                        new TextLoader.Column("PassengerCount", DataKind.Single, 3),
                        new TextLoader.Column("TripDistance", DataKind.Single, 4),
                        new TextLoader.Column("PaymentType", DataKind.Single, 9),
                        new TextLoader.Column("FareAmount", DataKind.Single, 10)
                    }
                }
            );

            // load the data 
            Console.Write("Loading training data....");
            var dataView = textLoader.Load(dataPath);
            Console.WriteLine("done");

            // load training data
            var training = context.Data.CreateEnumerable<TaxiTrip>(dataView, reuseRowObject: false);

            // set up data arrays
            var training_data = training.Select(v => v.GetFeatures()).ToArray();
            var training_labels = training.Select(v => v.GetLabel()).ToArray();

            // build features and labels
            var features = NetUtil.Var(new int[] { 5 }, DataType.Float); //CNTKUtil not available anymore
            var labels = NetUtil.Var(new int[] { 1 }, DataType.Float); //CNTKUtil not available anymore

            // build a regression model
            var network = features
                .Dense(1)
                .ToNetwork();
            Console.WriteLine("Model architecture:");
            Console.WriteLine(network.ToSummary());
        }
    }
}