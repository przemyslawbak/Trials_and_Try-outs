using System;
using System.IO;

using chapter06.Common;
using chapter06.ML.Base;
using chapter06.ML.Objects;

using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;

namespace chapter06.ML
{
    public class Trainer : BaseML
    {
        //1. The first change is the addition of a GetDataView helper method, which builds
        //the IDataView data view from the columns previously defined in
        //the LoginHistory class
        private (IDataView DataView, IEstimator<ITransformer> Transformer) GetDataView(string fileName, bool training = true)
        {
            IDataView trainingDataView = MlContext.Data.LoadFromTextFile<LoginHistory>(fileName, ',');

            if (!training)
            {
                return (trainingDataView, null);
            }

            IEstimator<ITransformer> dataProcessPipeline = MlContext.Transforms.Concatenate(
                FEATURES, 
                typeof(LoginHistory).ToPropertyList<LoginHistory>(nameof(LoginHistory.Label)));

            return (trainingDataView, dataProcessPipeline);
        }

        public void Train(string trainingFileName, string testingFileName)
        {
            if (!File.Exists(trainingFileName))
            {
                Console.WriteLine($"Failed to find training data file ({trainingFileName}");

                return;
            }

            if (!File.Exists(testingFileName))
            {
                Console.WriteLine($"Failed to find test data file ({testingFileName}");

                return;
            }

            //2. We then build the training data view and the
            //RandomizedPcaTrainer.Options object
            (IDataView DataView, IEstimator<ITransformer> Transformer) trainingDataView = GetDataView(trainingFileName);

            RandomizedPcaTrainer.Options options = new RandomizedPcaTrainer.Options
            {
                FeatureColumnName = FEATURES,
                ExampleWeightColumnName = null,
                Rank = 5,
                Oversampling = 20,
                EnsureZeroMean = true,
                Seed = 1
            };

            //3. We can then create the randomized PCA trainer, append it to the training data
            //view, fit our model, and then save it
            IEstimator<ITransformer> trainer = MlContext.AnomalyDetection.Trainers.RandomizedPca(options: options);

            EstimatorChain<ITransformer> trainingPipeline = trainingDataView.Transformer.Append(trainer);

            TransformerChain<ITransformer> trainedModel = trainingPipeline.Fit(trainingDataView.DataView);

            MlContext.Model.Save(trainedModel, trainingDataView.DataView.Schema, ModelPath);

            //4. Now we evaluate the model we just trained using the testing dataset
            (IDataView DataView, IEstimator<ITransformer> Transformer) testingDataView = GetDataView(testingFileName, true);

            IDataView testSetTransform = trainedModel.Transform(testingDataView.DataView);

            AnomalyDetectionMetrics modelMetrics = MlContext.AnomalyDetection.Evaluate(testSetTransform);

            //5. Finally, we output all of the classification metrics. Each of these will be detailed
            //in the next section
            Console.WriteLine($"Area Under Curve: {modelMetrics.AreaUnderRocCurve:P2}{Environment.NewLine}" +
                              $"Detection at FP Count: {modelMetrics.DetectionRateAtFalsePositiveCount}");
        }
    }
}