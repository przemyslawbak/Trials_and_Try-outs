using System;
using System.IO;

using chapter06.ML.Base;
using chapter06.ML.Objects;

using Microsoft.ML;

namespace chapter06.ML
{
    public class Trainer : BaseML
    {
        //1. The first addition is of the four variables to send to the transform
        private const int PvalueHistoryLength = 3;
        private const int SeasonalityWindowSize = 3;
        private const int TrainingWindowSize = 7;
        private const int Confidence = 98;

        public void Train(string trainingFileName)
        {
            if (!File.Exists(trainingFileName))
            {
                Console.WriteLine($"Failed to find training data file ({trainingFileName}");

                return;
            }

            //2. We then build the DataView object from the CSV training file
            IDataView trainingDataView = GetDataView(trainingFileName);

            //3. We can then create SSA spike detection
            Microsoft.ML.Transforms.TimeSeries.SsaSpikeEstimator trainingPipeLine = MlContext.Transforms.DetectSpikeBySsa(
                nameof(NetworkTrafficPrediction.Prediction),
                nameof(NetworkTrafficHistory.BytesTransferred),
                confidence: Confidence,
                pvalueHistoryLength: PvalueHistoryLength,
                trainingWindowSize: TrainingWindowSize,
                seasonalityWindowSize: SeasonalityWindowSize);

            //4. Now, we fit the model on the training data and save the model
            ITransformer trainedModel = trainingPipeLine.Fit(trainingDataView);

            MlContext.Model.Save(trainedModel, trainingDataView.Schema, ModelPath);

            Console.WriteLine("Model trained");
        }
    }
}