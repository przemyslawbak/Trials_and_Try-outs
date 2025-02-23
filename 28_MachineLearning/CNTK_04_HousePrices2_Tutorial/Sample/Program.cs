using Microsoft.ML;
using Microsoft.ML.Data;
using CNTK;
using Sample.CNTKUtil;

namespace Sample
{
    //https://www.mdft.academy/blog/use-csharp-and-a-cntk-neural-network-to-predict-house-prices-in-california

    /// <summary>
    /// HouseTrainingEngine is a custom training engine for this assignment.
    /// </summary>
    class HouseTrainingEngine : TrainingEngine
    {
        /// <summary>
        /// Set up the feature variable.
        /// </summary>
        /// <returns>The feature variable to use.</returns>
        protected override Variable CreateFeatureVariable()
        {
            return NetUtil.Var(new int[] { 8 }, DataType.Float);
        }

        /// <summary>
        /// Set up the label variable.
        /// </summary>
        /// <returns>The label variable to use.</returns>
        protected override Variable CreateLabelVariable()
        {
            return NetUtil.Var(new int[] { 1 }, DataType.Float);
        }

        /// <summary>
        /// Set up the model.
        /// </summary>
        /// <param name="features">The input feature to use.</param>
        /// <returns>The completed model.</returns>
        protected override Function CreateModel(Variable features)
        {
            return features
                .Dense(64, CNTKLib.ReLU)  // 64 nodes in this layer
                .Dense(64, CNTKLib.ReLU)  // 64 nodes in this layer
                .Dense(1)
                .ToNetwork();
        }

        // the rest of the code goes here...
    }

    /// <summary>
    /// The HouseBlockData class holds one single housing block data record.
    /// </summary>
    public class HouseBlockData
    {
        [LoadColumn(0)] public float Longitude { get; set; }
        [LoadColumn(1)] public float Latitude { get; set; }
        [LoadColumn(2)] public float HousingMedianAge { get; set; }
        [LoadColumn(3)] public float TotalRooms { get; set; }
        [LoadColumn(4)] public float TotalBedrooms { get; set; }
        [LoadColumn(5)] public float Population { get; set; }
        [LoadColumn(6)] public float Households { get; set; }
        [LoadColumn(7)] public float MedianIncome { get; set; }
        [LoadColumn(8)] public float MedianHouseValue { get; set; }

        public float[] GetFeatures() => new float[] { Longitude, Latitude, HousingMedianAge, TotalRooms, TotalBedrooms, Population, Households, MedianIncome };

        public float GetLabel() => MedianHouseValue / 1000.0f;
    }

    /// <summary>
    /// The program class.
    /// </summary>
    internal class Program
    {
        private static readonly string _dataPath = Path.GetFullPath(@"..\..\..\..\Data\california_housing.csv");

        /// <summary>
        /// The main application entry point.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        [STAThread]
        static void Main(string[] args)
        {
            // create the machine learning context
            var context = new MLContext();

            // check the current device for running neural networks
            Console.WriteLine($"Using device: {NetUtil.CurrentDevice.AsString()}");

            // load the dataset
            Console.WriteLine("Loading data...");
            var data = context.Data.LoadFromTextFile<HouseBlockData>(
                path: _dataPath,
                hasHeader: true,
                separatorChar: ',');

            // split into training and testing partitions
            var partitions = context.Data.TrainTestSplit(data, 0.2);

            // load training and testing data
            var training = context.Data.CreateEnumerable<HouseBlockData>(partitions.TrainSet, reuseRowObject: false);
            var testing = context.Data.CreateEnumerable<HouseBlockData>(partitions.TestSet, reuseRowObject: false);

            //CNTK can't train on an enumeration of class instances. It requires a float[][] for features and float[] for labels.
            // set up data arrays
            var training_data = training.Select(v => v.GetFeatures()).ToArray();
            var training_labels = training.Select(v => v.GetLabel()).ToArray();
            var testing_data = testing.Select(v => v.GetFeatures()).ToArray();
            var testing_labels = testing.Select(v => v.GetLabel()).ToArray();

            // build features and labels
            var features = NetUtil.Var(new int[] { 8 }, DataType.Float); //our neural network will use a 1-dimensional tensor of 8 float values as input
            var labels = NetUtil.Var(new int[] { 1 }, DataType.Float); //we want our neural network to output a single float value

            // build the network
            var network = features
                .Dense(8, CNTKLib.ReLU)
                .Dense(8, CNTKLib.ReLU)
                .Dense(1)
                .ToNetwork();
            Console.WriteLine("Model architecture:");
            Console.WriteLine(network.ToSummary());

            // set up a new training engine
            Console.WriteLine("Setting up training engine...");
            var engine = new HouseTrainingEngine()
            {
                lossFunctionType = TrainingEngine.LossFunctionType.MSE,
                metricType = TrainingEngine.MetricType.Loss,
                NumberOfEpochs = 50,
                BatchSize = 16,
                LearningRate = 0.001
            };

            // load the data into the engine
            engine.SetData(
                training.Select(v => v.GetFeatures()).ToArray(),
                training.Select(v => v.GetLabel()).ToArray(),
                testing.Select(v => v.GetFeatures()).ToArray(),
                testing.Select(v => v.GetLabel()).ToArray());

            // start the training
            Console.WriteLine("Start training...");
            engine.Train();

        }
    }
}