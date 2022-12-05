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
            //1. First, we check to make sure that the training data filename exists
            if (!File.Exists(trainingFileName))
            {
                Console.WriteLine($"Failed to find training data file ({trainingFileName}");

                return;
            }

            //2. Use the LoadFromTextFile helper method that ML.NET provides to assist with
            //the loading of text files into an IDataView object
            IDataView trainingDataView = MlContext.Data.LoadFromTextFile<RestaurantFeedback>(trainingFileName);

            //3. Use the TrainTestSplit method that ML.NET provides to create a test set from the
            //main training data
            DataOperationsCatalog.TrainTestData dataSplit = MlContext.Data.TrainTestSplit(trainingDataView, testFraction: 0.2);

            //4. Firstly, we create the pipeline
            Microsoft.ML.Transforms.Text.TextFeaturizingEstimator dataProcessPipeline = MlContext.Transforms.Text.FeaturizeText(
                outputColumnName: "Features",
                inputColumnName: nameof(RestaurantFeedback.Text));

            //5. Next, we instantiate our Trainer class
            Microsoft.ML.Trainers.SdcaLogisticRegressionBinaryTrainer sdcaRegressionTrainer = MlContext.BinaryClassification.Trainers.SdcaLogisticRegression(
                labelColumnName: nameof(RestaurantFeedback.Label),
                featureColumnName: "Features");

            //6. Then, we complete the pipeline by appending the trainer we instantiated
            //previously
            Microsoft.ML.Data.EstimatorChain<Microsoft.ML.Data.BinaryPredictionTransformer<Microsoft.ML.Calibrators.CalibratedModelParametersBase<Microsoft.ML.Trainers.LinearBinaryModelParameters, Microsoft.ML.Calibrators.PlattCalibrator>>> trainingPipeline = dataProcessPipeline.Append(sdcaRegressionTrainer);

            //7. Next, we train the model with the dataset we created earlier in the chapter
            ITransformer trainedModel = trainingPipeline.Fit(dataSplit.TrainSet);
            MlContext.Model.Save(trainedModel, dataSplit.TrainSet.Schema, ModelPath);

            //8. We save our newly created model to the filename specified, matching the
            //training set's schema
            IDataView testSetTransform = trainedModel.Transform(dataSplit.TestSet);

            //9. Now, we transform our newly created model with the test set we created earlier
            Microsoft.ML.Data.CalibratedBinaryClassificationMetrics modelMetrics = MlContext.BinaryClassification.Evaluate(
                data: testSetTransform,
                labelColumnName: nameof(RestaurantFeedback.Label),
                scoreColumnName: nameof(RestaurantPrediction.Score));

            //10. Finally, we will use the testSetTransform function created previously and
            //pass it into the BinaryClassification class's Evaluate method
            //This method allows us to generate model metrics. We then print the main metrics using the
            //trained model with the test set. We will dive into these properties specifically in the
            //Evaluating the Model section of this chapter.
            Console.WriteLine($"Area Under Curve: {modelMetrics.AreaUnderRocCurve:P2}{Environment.NewLine}" +
                              $"Area Under Precision Recall Curve: {modelMetrics.AreaUnderPrecisionRecallCurve:P2}{Environment.NewLine}" +
                              $"Accuracy: {modelMetrics.Accuracy:P2}{Environment.NewLine}" +
                              $"F1Score: {modelMetrics.F1Score:P2}{Environment.NewLine}" +
                              $"Positive Recall: {modelMetrics.PositiveRecall:#.##}{Environment.NewLine}" +
                              $"Negative Recall: {modelMetrics.NegativeRecall:#.##}{Environment.NewLine}");

        }
    }
}
