using HandsOn_Book.ML.Base;
using HandsOn_Book.ML.Objects;
using Microsoft.ML;
using System;
using System.IO;

namespace HandsOn_Book.ML
{
    public class Trainer : BaseML
    {
        public void Train(string trainingFileName)
        {
            if (!File.Exists(trainingFileName))
            {
                Console.WriteLine($"Failed to find training data file ({trainingFileName}");

                return;
            }

            var trainingDataView = MlContext.Data.LoadFromTextFile<RestaurantFeedback>(trainingFileName);

            var dataSplit = MlContext.Data.TrainTestSplit(trainingDataView, testFraction: 0.2);

            var dataProcessPipeline = MlContext.Transforms.Text.FeaturizeText(
                outputColumnName: "Features",
                inputColumnName: nameof(RestaurantFeedback.Text));

            var sdcaRegressionTrainer = MlContext.BinaryClassification.Trainers.SdcaLogisticRegression(
                labelColumnName: nameof(RestaurantFeedback.Label),
                featureColumnName: "Features");

            var trainingPipeline = dataProcessPipeline.Append(sdcaRegressionTrainer);

            ITransformer trainedModel = trainingPipeline.Fit(dataSplit.TrainSet);
            MlContext.Model.Save(trainedModel, dataSplit.TrainSet.Schema, ModelPath);

            var testSetTransform = trainedModel.Transform(dataSplit.TestSet);

            var modelMetrics = MlContext.BinaryClassification.Evaluate(
                data: testSetTransform,
                labelColumnName: nameof(RestaurantFeedback.Label),
                scoreColumnName: nameof(RestaurantPrediction.Score));

            Console.WriteLine($"Area Under Curve: {modelMetrics.AreaUnderRocCurve:P2}{Environment.NewLine}" +
                              $"Area Under Precision Recall Curve: {modelMetrics.AreaUnderPrecisionRecallCurve:P2}{Environment.NewLine}" +
                              $"Accuracy: {modelMetrics.Accuracy:P2}{Environment.NewLine}" +
                              $"F1Score: {modelMetrics.F1Score:P2}{Environment.NewLine}" +
                              $"Positive Recall: {modelMetrics.PositiveRecall:#.##}{Environment.NewLine}" +
                              $"Negative Recall: {modelMetrics.NegativeRecall:#.##}{Environment.NewLine}");

        }
    }
}
