using Newtonsoft.Json;
using SharpLearning.CrossValidation.TrainingTestSplitters;
using SharpLearning.GradientBoost.Learners;
using SharpLearning.InputOutput.Csv;
using SharpLearning.Metrics.Regression;
using SharpLearning.Optimization;
using SharpLearning.RandomForest.Learners;
using System.ComponentModel;

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
            var targetName = "quality";

            // read the "quality" column, this is the targets for our learner. 
            var targets = parser.EnumerateRows(targetName)
                .ToF64Vector();

            // read the feature matrix, all columns except "quality",
            // this is the observations for our learner.
            var observations = parser.EnumerateRows(c => c != targetName)
                .ToF64Matrix();

            var numberOfFeatures = observations.ColumnCount;

            // 30 % of the data is used for the test set. 
            var splitter = new RandomTrainingTestIndexSplitter<double>(trainingPercentage: 0.7, seed: 24);

            var trainingTestSplit = splitter.SplitSet(observations, targets);
            var trainSet = trainingTestSplit.TrainingSet;
            var testSet = trainingTestSplit.TestSet;

            // create the metric
            var metric = new MeanSquaredErrorRegressionMetric();

            //HYPERPARAMETERS
            //https://github.com/mdabros/SharpLearning/wiki/hyperparameter-tuning
            // Parameter specs for the optimizer
            var parameters = new IParameterSpec[]
            {
    new MinMaxParameterSpec(min: 80, max: 300,
        transform: Transform.Linear, parameterType: ParameterType.Discrete), // iterations

    new MinMaxParameterSpec(min: 0.02, max:  0.2,
        transform: Transform.Log10, parameterType: ParameterType.Continuous), // learning rate

    new MinMaxParameterSpec(min: 8, max: 15,
        transform: Transform.Linear, parameterType: ParameterType.Discrete), // maximumTreeDepth

    new MinMaxParameterSpec(min: 0.5, max: 0.9,
        transform: Transform.Linear, parameterType: ParameterType.Continuous), // subSampleRatio

    new MinMaxParameterSpec(min: 1, max: numberOfFeatures,
        transform: Transform.Linear, parameterType: ParameterType.Discrete), // featuresPrSplit
            };

            // Further split the training data to have a validation set to measure
            // how well the model generalizes to unseen data during the optimization.
            var validationSplit = new RandomTrainingTestIndexSplitter<double>(trainingPercentage: 0.7, seed: 24)
                .SplitSet(trainSet.Observations, trainSet.Targets);

            // Define optimizer objective (function to minimize)
            Func<double[], OptimizerResult> minimize = p =>
            {
                // create the candidate learner using the current optimization parameters.
                var candidateLearner = new RegressionRandomForestLearner(
                                     maximumTreeDepth: (int)p[2],
                                     subSampleRatio: p[3],
                                     featuresPrSplit: (int)p[4],
                                     runParallel: false);

                var candidateModel = candidateLearner.Learn(validationSplit.TrainingSet.Observations,
                validationSplit.TrainingSet.Targets);

                var validationPredictions = candidateModel.Predict(validationSplit.TestSet.Observations);
                var candidateError = metric.Error(validationSplit.TestSet.Targets, validationPredictions);

                return new OptimizerResult(p, candidateError);
            };

            // create optimizer
            var optimizer = new RandomSearchOptimizer(parameters, iterations: 30, runParallel: true);

            // find best hyperparameters
            var result = optimizer.OptimizeBest(minimize);
            var best = result.ParameterSet;

            // create the final learner using the best hyperparameters.
            var learner = new RegressionSquareLossGradientBoostLearner(
                            iterations: (int)best[0],
                            learningRate: best[1],
                            maximumTreeDepth: (int)best[2],
                            subSampleRatio: best[3],
                            featuresPrSplit: (int)best[4],
                            runParallel: false);

            // learn model with found parameters
            var model = learner.Learn(trainSet.Observations, trainSet.Targets);

            // Create the learner and learn the model.
            /*var learner = new RegressionRandomForestLearner(trees: 100);
            var model = learner.Learn(trainSet.Observations, trainSet.Targets);*/

            // predict the training and test set.
            var trainPredictions = model.Predict(trainSet.Observations);
            var testPredictions = model.Predict(testSet.Observations);

            // measure the error on training and test set.
            var trainError = metric.Error(trainSet.Targets, trainPredictions);
            var testError = metric.Error(testSet.Targets, testPredictions);

            // the variable importance requires the featureNameToIndex
            // from the data set. This mapping describes the relation
            // from column name to index in the feature matrix.
            var featureNameToIndex = parser.EnumerateRows(c => c != targetName)
                .First().ColumnNameToIndex;

            // Get the variable importance from the model.
            var importances = model.GetVariableImportance(featureNameToIndex);

            //POSSIBLE TO SAVE AND LOAD THE MODEL
        }
    }
}