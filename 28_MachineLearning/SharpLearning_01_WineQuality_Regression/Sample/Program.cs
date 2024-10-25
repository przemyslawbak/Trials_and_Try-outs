using SharpLearning.CrossValidation.TrainingTestSplitters;
using SharpLearning.InputOutput.Csv;
using SharpLearning.Metrics.Regression;
using SharpLearning.RandomForest.Learners;

namespace Sample
{
    //https://github.com/mdabros/SharpLearning/wiki/Introduction-to-SharpLearning
    internal class Program
    {
        private static string dataPath = Path.GetFullPath(@"..\..\..\Data\winequality-white.csv");

        static void Main(string[] args)
        {
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

            // 30 % of the data is used for the test set. 
            var splitter = new RandomTrainingTestIndexSplitter<double>(trainingPercentage: 0.7, seed: 24);

            var trainingTestSplit = splitter.SplitSet(observations, targets);
            var trainSet = trainingTestSplit.TrainingSet;
            var testSet = trainingTestSplit.TestSet;

            // Create the learner and learn the model.
            var learner = new RegressionRandomForestLearner(trees: 100);
            var model = learner.Learn(trainSet.Observations, trainSet.Targets);

            // predict the training and test set.
            var trainPredictions = model.Predict(trainSet.Observations);
            var testPredictions = model.Predict(testSet.Observations);

            // create the metric
            var metric = new MeanSquaredErrorRegressionMetric();

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