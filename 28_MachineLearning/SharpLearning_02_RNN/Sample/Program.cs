using Newtonsoft.Json;
using SharpLearning.FeatureTransformations.MatrixTransforms;
using SharpLearning.InputOutput.Csv;
using SharpLearning.Metrics.Regression;
using SharpLearning.Neural.Activations;
using SharpLearning.Neural.Layers;
using SharpLearning.Neural;
using System.ComponentModel;
using SharpLearning.Neural.Learners;
using SharpLearning.Neural.Loss;
using System.Diagnostics;

namespace Sample
{
    //https://github.com/mdabros/SharpLearning.Examples/tree/master/src/SharpLearning.Examples

    internal class Program
    {
        private static string dataPath = Path.GetFullPath(@"..\..\..\Data\output.csv");

        private static void SaveToCsv<T>(List<T> reportData, string path)
        {
            var lines = new List<string>();
            IEnumerable<PropertyDescriptor> props = TypeDescriptor.GetProperties(typeof(T)).OfType<PropertyDescriptor>();
            var header = string.Join(",", props.ToList().Select(x => x.Name));
            lines.Add(header);
            var valueLines = reportData.Select(row => string.Join(",", header.Split(',').Select(a => row.GetType().GetProperty(a).GetValue(row, null))));
            lines.AddRange(valueLines);
            File.WriteAllLines(path, lines.ToArray());
        }

        static async Task Main(string[] args)
        {
            string json = string.Empty;
            var client = new HttpClient();
            var url = "https://gist.githubusercontent.com/przemyslawbak/2058d9aeddfe09d2a26da81dfc16e5d0/raw/json_data_sample.txt";

            using (HttpResponseMessage response = await client.GetAsync(url))
            {
                using (HttpContent content = response.Content)
                {
                    json = content.ReadAsStringAsync().Result;
                }
            }

            var dataOhlcv = JsonConvert.DeserializeObject<List<OhlcvObject>>(json).Select(x => new OhlcvObject()
            {
                Open = x.Open,
                High = x.High,
                Low = x.Low,
                Close = x.Close,
                Volume = x.Volume,
            }).Reverse().ToList();

            SaveToCsv(dataOhlcv, dataPath);
            // Setup the CsvParser
            var parser = new CsvParser(() => new StreamReader(dataPath));

            // the column name in the wine quality data set we want to model.
            var targetName = "Close";

            // read the "quality" column, this is the targets for our learner. 
            var targets = parser.EnumerateRows(targetName)
                .ToF64Vector();

            // read the feature matrix, all columns except "quality",
            // this is the observations for our learner.
            var observations = parser.EnumerateRows(c => c != targetName)
                .ToF64Matrix();

            // transform pixel values to be between 0 and 1.
            var minMaxTransformer = new MinMaxTransformer(0.0, 1.0);
            minMaxTransformer.Transform(observations, observations);

            var numberOfFeatures = observations.ColumnCount;

            // define the neural net.
            var net = new NeuralNet();
            net.Add(new InputLayer(inputUnits: numberOfFeatures));
            net.Add(new DropoutLayer(0.2));
            net.Add(new DenseLayer(800, Activation.Relu));
            net.Add(new DropoutLayer(0.5));
            net.Add(new DenseLayer(800, Activation.Relu));
            net.Add(new DropoutLayer(0.5));
            net.Add(new SquaredErrorRegressionLayer());

            // using only 10 iteration to make the example run faster.
            // using square error as error metric. This is only used for reporting progress.
            var learner = new RegressionNeuralNetLearner(net, iterations: 10, loss: new SquareLoss());
            var model = learner.Learn(observations, targets);

            var metric = new MeanSquaredErrorRegressionMetric();
            var predictions = model.Predict(observations);

            Trace.WriteLine("Training Error: " + metric.Error(targets, predictions));

            //POSSIBLE TO SAVE AND LOAD THE MODEL
        }
    }
}