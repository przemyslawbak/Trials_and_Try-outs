using System;
using System.IO;

using chapter04.Common;
using chapter04.ML.Base;
using chapter04.ML.Objects;

using Microsoft.ML;

namespace chapter04.ML
{
    /// <summary>
    /// The FastTree trainer doesn't require normalization but does require all of the feature
    /// columns to use a float variable type and the label column to be a bool variable type.
    /// </summary>
    public class Trainer : BaseML
    {
        public void Train(string trainingFileName, string testFileName)
        {
            if (!File.Exists(trainingFileName))
            {
                Console.WriteLine($"Failed to find training data file ({trainingFileName}");

                return;
            }

            //1. The first change is the check to ensure the test filename exists, shown in the
            //following code block
            if (!File.Exists(testFileName))
            {
                Console.WriteLine($"Failed to find test data file ({testFileName}");

                return;
            }

            IDataView trainingDataView = MlContext.Data.LoadFromTextFile<CarInventory>(trainingFileName, ',', hasHeader: false);

            //2. We then build the data process pipeline using the NormalizeMeanVariance
            //transform method we used in Chapter 3, Regression Model, on the inputted
            //values, like this
            IEstimator<ITransformer> dataProcessPipeline = MlContext.Transforms.Concatenate("Features",
                typeof(CarInventory).ToPropertyList<CarInventory>(nameof(CarInventory.Label)))
                .Append(MlContext.Transforms.NormalizeMeanVariance(inputColumnName: "Features",
                    outputColumnName: "FeaturesNormalizedByMeanVar"));

            //3. We can then create the FastTree trainer with the label from the CarInventory
            //class and the normalized mean variance, as follows
            Microsoft.ML.Trainers.FastTree.FastTreeBinaryTrainer trainer = MlContext.BinaryClassification.Trainers.FastTree(labelColumnName: nameof(CarInventory.Label),
                featureColumnName: "FeaturesNormalizedByMeanVar",
                numberOfLeaves: 2,
                numberOfTrees: 1000,
                minimumExampleCountPerLeaf: 1,
                learningRate: 0.2);

            //4. Lastly, we call the Regression.Evaluate method to provide regression-specific
            //metrics, followed by a Console.WriteLine call to provide these metrics to your
            //console output. We will go into detail about what each of these means in the last
            //section of the chapter, but for now, the code can be seen here
            Microsoft.ML.Data.EstimatorChain<Microsoft.ML.Data.BinaryPredictionTransformer<Microsoft.ML.Calibrators.CalibratedModelParametersBase<Microsoft.ML.Trainers.FastTree.FastTreeBinaryModelParameters, Microsoft.ML.Calibrators.PlattCalibrator>>> trainingPipeline = dataProcessPipeline.Append(trainer);
            Microsoft.ML.Data.TransformerChain<Microsoft.ML.Data.BinaryPredictionTransformer<Microsoft.ML.Calibrators.CalibratedModelParametersBase<Microsoft.ML.Trainers.FastTree.FastTreeBinaryModelParameters, Microsoft.ML.Calibrators.PlattCalibrator>>> trainedModel = trainingPipeline.Fit(trainingDataView);
            MlContext.Model.Save(trainedModel, trainingDataView.Schema, ModelPath);

            //Now, we evaluate the model we just trained
            Microsoft.ML.Data.TransformerChain<Microsoft.ML.Transforms.FeatureContributionCalculatingTransformer> evaluationPipeline = trainedModel.Append(MlContext.Transforms
                .CalculateFeatureContribution(trainedModel.LastTransformer)
                .Fit(dataProcessPipeline.Fit(trainingDataView).Transform(trainingDataView)));
            IDataView testDataView = MlContext.Data.LoadFromTextFile<CarInventory>(testFileName, ',', hasHeader: false);
            IDataView testSetTransform = evaluationPipeline.Transform(testDataView);
            Microsoft.ML.Data.CalibratedBinaryClassificationMetrics modelMetrics = MlContext.BinaryClassification.Evaluate(data: testSetTransform,
                labelColumnName: nameof(CarInventory.Label),
                scoreColumnName: "Score");

            //Finally, we output all of the classification metrics
            Console.WriteLine($"Accuracy: {modelMetrics.Accuracy:P2}");
            Console.WriteLine($"Area Under Curve: {modelMetrics.AreaUnderRocCurve:P2}");
            Console.WriteLine($"Area under Precision recall Curve: {modelMetrics.AreaUnderPrecisionRecallCurve:P2}");
            Console.WriteLine($"F1Score: {modelMetrics.F1Score:P2}");
            Console.WriteLine($"LogLoss: {modelMetrics.LogLoss:#.##}");
            Console.WriteLine($"LogLossReduction: {modelMetrics.LogLossReduction:#.##}");
            Console.WriteLine($"PositivePrecision: {modelMetrics.PositivePrecision:#.##}");
            Console.WriteLine($"PositiveRecall: {modelMetrics.PositiveRecall:#.##}");
            Console.WriteLine($"NegativePrecision: {modelMetrics.NegativePrecision:#.##}");
            Console.WriteLine($"NegativeRecall: {modelMetrics.NegativeRecall:P2}");
        }
    }
}