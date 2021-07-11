using System;

using chapter09.lib.Common;
using chapter09.lib.ML.Base;
using chapter09.lib.ML.Objects;

using Microsoft.ML;

namespace chapter09.lib.ML
{
    public class FileClassificationTrainer : BaseML
    {
        public void Train(string trainingFileName, string testingFileName)
        {
            if (!System.IO.File.Exists(trainingFileName))
            {
                Console.WriteLine($"Failed to find training data file ({trainingFileName}");

                return;
            }

            if (!System.IO.File.Exists(testingFileName))
            {
                Console.WriteLine($"Failed to find test data file ({testingFileName}");

                return;
            }

            //1. The first change is the use of the FileData class to read the CSV file into the
            //dataView property, as shown in the following code block
            IDataView dataView = MlContext.Data.LoadFromTextFile<FileData>(trainingFileName, hasHeader: false);

            //2. Next, we map our FileData features to create our pipeline, as follows
            Microsoft.ML.Data.EstimatorChain<Microsoft.ML.Data.ColumnConcatenatingTransformer> dataProcessPipeline = MlContext.Transforms.NormalizeMeanVariance(nameof(FileData.FileSize))
                .Append(MlContext.Transforms.NormalizeMeanVariance(nameof(FileData.Is64Bit)))
                .Append(MlContext.Transforms.NormalizeMeanVariance(nameof(FileData.IsSigned)))
                .Append(MlContext.Transforms.NormalizeMeanVariance(nameof(FileData.NumberImportFunctions)))
                .Append(MlContext.Transforms.NormalizeMeanVariance(nameof(FileData.NumberExportFunctions)))
                .Append(MlContext.Transforms.NormalizeMeanVariance(nameof(FileData.NumberImports)))
                .Append(MlContext.Transforms.Text.FeaturizeText("FeaturizeText", nameof(FileData.Strings)))
                .Append(MlContext.Transforms.Concatenate(FEATURES, nameof(FileData.FileSize), nameof(FileData.Is64Bit),
                    nameof(FileData.IsSigned), nameof(FileData.NumberImportFunctions), nameof(FileData.NumberExportFunctions),
                    nameof(FileData.NumberImports), "FeaturizeText"));

            //3. Lastly, we initialize our FastTree algorithm, as follows
            Microsoft.ML.Trainers.FastTree.FastTreeBinaryTrainer trainer = MlContext.BinaryClassification.Trainers.FastTree(labelColumnName: nameof(FileData.Label),
                featureColumnName: FEATURES,
                numberOfLeaves: 2,
                numberOfTrees: 1000,
                minimumExampleCountPerLeaf: 1,
                learningRate: 0.2);

            Microsoft.ML.Data.EstimatorChain<Microsoft.ML.Data.BinaryPredictionTransformer<Microsoft.ML.Calibrators.CalibratedModelParametersBase<Microsoft.ML.Trainers.FastTree.FastTreeBinaryModelParameters, Microsoft.ML.Calibrators.PlattCalibrator>>> trainingPipeline = dataProcessPipeline.Append(trainer);
            Microsoft.ML.Data.TransformerChain<Microsoft.ML.Data.BinaryPredictionTransformer<Microsoft.ML.Calibrators.CalibratedModelParametersBase<Microsoft.ML.Trainers.FastTree.FastTreeBinaryModelParameters, Microsoft.ML.Calibrators.PlattCalibrator>>> trainedModel = trainingPipeline.Fit(dataView);

            MlContext.Model.Save(trainedModel, dataView.Schema, Constants.MODEL_PATH);

            IDataView testingDataView = MlContext.Data.LoadFromTextFile<FileData>(testingFileName, hasHeader: false);

            IDataView testDataView = trainedModel.Transform(testingDataView);

            Microsoft.ML.Data.CalibratedBinaryClassificationMetrics modelMetrics = MlContext.BinaryClassification.Evaluate(
                data: testDataView,
                labelColumnName: nameof(FileDataPrediction.Label),
                scoreColumnName: nameof(FileDataPrediction.Score));

            Console.WriteLine($"Entropy: {modelMetrics.Entropy}");
            Console.WriteLine($"Log Loss: {modelMetrics.LogLoss}");
            Console.WriteLine($"Log Loss Reduction: {modelMetrics.LogLossReduction}");
        }
    }
}