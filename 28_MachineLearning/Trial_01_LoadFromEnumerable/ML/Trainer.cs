using System;
using System.Collections.Generic;
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
        public void Train()
        {
            IEnumerable<FileInput> input = new FileInput[]
            {
                new FileInput { Date = DateTime.Now, CloseBrent = 111.34F, CloseDax = 15674.38F, SmaBrent = 112.23F, SmaDax = 15632.13F, SmaDeltaBrent = true, SmaDeltaDax = false },
                new FileInput { Date = DateTime.Now.AddDays(1), CloseBrent = 111.35F, CloseDax = 15374.38F, SmaBrent = 111.23F, SmaDax = 15642.13F, SmaDeltaBrent = true, SmaDeltaDax = true },
                new FileInput { Date = DateTime.Now.AddDays(2), CloseBrent = 112.35F, CloseDax = 15474.38F, SmaBrent = 110.23F, SmaDax = 15242.13F, SmaDeltaBrent = false, SmaDeltaDax = true },
            };

            IDataView trainingDataView = MlContext.Data.LoadFromEnumerable(input);

            DataOperationsCatalog.TrainTestData dataSplit = MlContext.Data.TrainTestSplit(trainingDataView, testFraction: 0.2);

            Microsoft.ML.Data.EstimatorChain<Microsoft.ML.Data.ColumnConcatenatingTransformer> dataProcessPipeline = MlContext.Transforms.CopyColumns("Label", nameof(FileInput.SmaDax))
                .Append(MlContext.Transforms.Text.FeaturizeText("NGrams", nameof(FileInput.SmaDax)))
                .Append(MlContext.Transforms.Concatenate("Features", "NGrams"));

            Microsoft.ML.Trainers.SdcaLogisticRegressionBinaryTrainer trainer = MlContext.BinaryClassification.Trainers.SdcaLogisticRegression(labelColumnName: "Label", featureColumnName: "Features");

            Microsoft.ML.Data.EstimatorChain<Microsoft.ML.Data.BinaryPredictionTransformer<Microsoft.ML.Calibrators.CalibratedModelParametersBase<Microsoft.ML.Trainers.LinearBinaryModelParameters, Microsoft.ML.Calibrators.PlattCalibrator>>> trainingPipeline = dataProcessPipeline.Append(trainer);

            ITransformer trainedModel = trainingPipeline.Fit(dataSplit.TrainSet);
            MlContext.Model.Save(trainedModel, dataSplit.TrainSet.Schema, ModelPath);
        }
    }
}