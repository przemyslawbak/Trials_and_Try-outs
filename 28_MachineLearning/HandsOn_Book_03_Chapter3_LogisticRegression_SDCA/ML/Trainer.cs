using System;
using System.IO;

using chapter03_logistic_regression.ML.Base;
using chapter03_logistic_regression.ML.Objects;

using Microsoft.ML;

namespace chapter03_logistic_regression.ML
{
    //You can think of NGrams as breaking a longer string into ranges of characters based on the
    //value of the NGram parameter.
    public class Trainer : BaseML
    {
        public void Train(string trainingFileName)
        {
            if (!File.Exists(trainingFileName))
            {
                Console.WriteLine($"Failed to find training data file ({trainingFileName}");

                return;
            }

            IDataView trainingDataView = MlContext.Data.LoadFromTextFile<FileInput>(trainingFileName);

            DataOperationsCatalog.TrainTestData dataSplit = MlContext.Data.TrainTestSplit(trainingDataView, testFraction: 0.2);

            Microsoft.ML.Data.EstimatorChain<Microsoft.ML.Data.ColumnConcatenatingTransformer> dataProcessPipeline = MlContext.Transforms.CopyColumns("Label", nameof(FileInput.Label))
                .Append(MlContext.Transforms.Text.FeaturizeText("NGrams", nameof(FileInput.Strings)))
                .Append(MlContext.Transforms.Concatenate("Features", "NGrams"));

            Microsoft.ML.Trainers.SdcaLogisticRegressionBinaryTrainer trainer = MlContext.BinaryClassification.Trainers.SdcaLogisticRegression(labelColumnName: "Label", featureColumnName: "Features");

            Microsoft.ML.Data.EstimatorChain<Microsoft.ML.Data.BinaryPredictionTransformer<Microsoft.ML.Calibrators.CalibratedModelParametersBase<Microsoft.ML.Trainers.LinearBinaryModelParameters, Microsoft.ML.Calibrators.PlattCalibrator>>> trainingPipeline = dataProcessPipeline.Append(trainer);

            ITransformer trainedModel = trainingPipeline.Fit(dataSplit.TrainSet);
            MlContext.Model.Save(trainedModel, dataSplit.TrainSet.Schema, ModelPath);
        }
    }
}