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
            //1. Akin to what was done in the Trainer class, we verify that the model exists
            //prior to reading it
            if (!File.Exists(ModelPath))
            {
                Console.WriteLine($"Failed to find model at {ModelPath}");

                return;
            }

            //2. Then, we define the ITransformer object
            //This object will contain our model once we load via the Model.Load method
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

            //3. Next, create a PredictionEngine object given the model we loaded earlier
            PredictionEngine<RestaurantFeedback, RestaurantPrediction> predictionEngine = MlContext.Model.CreatePredictionEngine<RestaurantFeedback, RestaurantPrediction>(mlModel);

            //4. Then, call the Predict method on the PredictionEngine class
            RestaurantPrediction prediction = predictionEngine.Predict(new RestaurantFeedback { Text = inputData });

            //5. Finally, display the prediction output along with the probability
            Console.WriteLine($"Based on \"{inputData}\", the feedback is predicted to be:{Environment.NewLine}{(prediction.Prediction ? "Negative" : "Positive")} at a {prediction.Probability:P0} confidence");
        }
    }
}
