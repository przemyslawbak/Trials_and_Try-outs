using System;
using System.IO;
using System.Linq;
using Microsoft.ML;

namespace MS_Tutorial
{
    //https://docs.microsoft.com/en-us/dotnet/machine-learning/tutorials/sentiment-analysis
    class Program
    {
        private static string _appPath => Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]);
        private static string _trainDataPath => Path.Combine(_appPath, "..", "..", "..", "Data", "issues_train.tsv");
        private static string _testDataPath => Path.Combine(_appPath, "..", "..", "..", "Data", "issues_test.tsv");
        private static string _modelPath => Path.Combine(_appPath, "..", "..", "..", "Models", "model.zip");

        private static MLContext _mlContext;
        private static PredictionEngine<GitHubIssue, IssuePrediction> _predEngine;
        private static ITransformer _trainedModel;
        static IDataView _trainingDataView;

        static void Main(string[] args)
        {
            _mlContext = new MLContext(seed: 0);
            _trainingDataView = _mlContext.Data.LoadFromTextFile<GitHubIssue>(_trainDataPath, hasHeader: true);
            IEstimator<ITransformer> pipeline = ProcessData();
            IEstimator<ITransformer> trainingPipeline = BuildAndTrainModel(_trainingDataView, pipeline);
            Evaluate(_trainingDataView.Schema);
            PredictIssue();
        }

        /// <summary>
        /// The ProcessData method executes the following tasks:
        ///Extracts and transforms the data.
        ///Returns the processing pipeline.
        /// </summary>
        public static IEstimator<ITransformer> ProcessData()
        {
            Microsoft.ML.Data.EstimatorChain<Microsoft.ML.Data.ColumnConcatenatingTransformer> pipeline = _mlContext.Transforms.Conversion.MapValueToKey(inputColumnName: "Area", outputColumnName: "Label")
                .Append(_mlContext.Transforms.Text.FeaturizeText(inputColumnName: "Title", outputColumnName: "TitleFeaturized"))
                .Append(_mlContext.Transforms.Text.FeaturizeText(inputColumnName: "Description", outputColumnName: "DescriptionFeaturized"))
                .Append(_mlContext.Transforms.Concatenate("Features", "TitleFeaturized", "DescriptionFeaturized"))
                //Use AppendCacheCheckpoint for small/medium datasets to lower training time
                //Do NOT use it (remove .AppendCacheCheckpoint()) when handling very large datasets.
                .AppendCacheCheckpoint(_mlContext);

            return pipeline;
        }

        /// <summary>
        /// The BuildAndTrainModel method executes the following tasks:
        ///Creates the training algorithm class.
    ///Trains the model.
    ///Predicts area based on training data.
    ///Returns the model.
        /// </summary>
        /// <param name="trainingDataView"></param>
        /// <param name="pipeline"></param>
        /// <returns></returns>
        public static IEstimator<ITransformer> BuildAndTrainModel(IDataView trainingDataView, IEstimator<ITransformer> pipeline)
        {
            Microsoft.ML.Data.EstimatorChain<Microsoft.ML.Transforms.KeyToValueMappingTransformer> trainingPipeline = pipeline.Append(_mlContext.MulticlassClassification.Trainers.SdcaMaximumEntropy("Label", "Features"))
        .Append(_mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));

            _trainedModel = trainingPipeline.Fit(trainingDataView);
            _predEngine = _mlContext.Model.CreatePredictionEngine<GitHubIssue, IssuePrediction>(_trainedModel);

            GitHubIssue issue = new GitHubIssue()
            {
                Title = "WebSockets communication is slow in my machine",
                Description = "The WebSockets communication used under the covers by SignalR looks like is going slow in my development machine.."
            };
            IssuePrediction prediction = _predEngine.Predict(issue);
            Console.WriteLine($"=============== Single Prediction just-trained-model - Result: {prediction.Area} ===============");
            return trainingPipeline;
        }

        /// <summary>
        /// The Evaluate method executes the following tasks:
        //Loads the test dataset.
        //Creates the multiclass evaluator.
        //Evaluates the model and create metrics.
        //Displays the metrics.

        /// </summary>
        /// <param name="trainingDataViewSchema"></param>
        public static void Evaluate(DataViewSchema trainingDataViewSchema)
        {
            var testDataView = _mlContext.Data.LoadFromTextFile<GitHubIssue>(_testDataPath, hasHeader: true);
            var testMetrics = _mlContext.MulticlassClassification.Evaluate(_trainedModel.Transform(testDataView));
            Console.WriteLine($"*************************************************************************************************************");
            Console.WriteLine($"*       Metrics for Multi-class Classification model - Test Data     ");
            Console.WriteLine($"*------------------------------------------------------------------------------------------------------------");
            Console.WriteLine($"*       MicroAccuracy:    {testMetrics.MicroAccuracy:0.###}");
            Console.WriteLine($"*       MacroAccuracy:    {testMetrics.MacroAccuracy:0.###}");
            Console.WriteLine($"*       LogLoss:          {testMetrics.LogLoss:#.###}");
            Console.WriteLine($"*       LogLossReduction: {testMetrics.LogLossReduction:#.###}");
            Console.WriteLine($"*************************************************************************************************************");
            SaveModelAsFile(_mlContext, trainingDataViewSchema, _trainedModel);
        }

        private static void SaveModelAsFile(MLContext mlContext, DataViewSchema trainingDataViewSchema, ITransformer model)
        {
            mlContext.Model.Save(model, trainingDataViewSchema, _modelPath);
        }

        /// <summary>
        /// The PredictIssue method executes the following tasks:
        ///Loads the saved model
    ///Creates a single issue of test data.
    ///Predicts Area based on test data.
    ///Combines test data and predictions for reporting.
    ///Displays the predicted results.
        /// </summary>
        private static void PredictIssue()
        {
            ITransformer loadedModel = _mlContext.Model.Load(_modelPath, out var modelInputSchema);
            GitHubIssue singleIssue = new GitHubIssue() { Title = "Entity Framework crashes", Description = "When connecting to the database, EF is crashing" };
            _predEngine = _mlContext.Model.CreatePredictionEngine<GitHubIssue, IssuePrediction>(loadedModel);
            IssuePrediction prediction = _predEngine.Predict(singleIssue);
            Console.WriteLine($"=============== Single Prediction - Result: {prediction.Area} ===============");
        }
    }
}
