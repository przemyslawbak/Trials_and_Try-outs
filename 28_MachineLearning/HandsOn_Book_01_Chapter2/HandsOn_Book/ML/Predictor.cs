using HandsOn_Book.ML.Base;
using HandsOn_Book.ML.Objects;
using Microsoft.ML;
using System;
using System.IO;

namespace HandsOn_Book.ML
{
    public class Predictor : BaseML
    {
        public void Predict(string inputData)
        {
            if (!File.Exists(ModelPath))
            {
                Console.WriteLine($"Failed to find model at {ModelPath}");

                return;
            }

            ITransformer mlModel;

            using (var stream = new FileStream(ModelPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                mlModel = MlContext.Model.Load(stream, out _);
            }

            if (mlModel == null)
            {
                Console.WriteLine("Failed to load model");

                return;
            }

            var predictionEngine = MlContext.Model.CreatePredictionEngine<RestaurantFeedback, RestaurantPrediction>(mlModel);

            var prediction = predictionEngine.Predict(new RestaurantFeedback { Text = inputData });

            Console.WriteLine($"Based on \"{inputData}\", the feedback is predicted to be:{Environment.NewLine}{(prediction.Prediction ? "Negative" : "Positive")} at a {prediction.Probability:P0} confidence");
        }
    }
}
