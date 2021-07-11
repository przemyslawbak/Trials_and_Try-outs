using System;
using System.Collections.Generic;
using System.IO;
using chapter07.Common;
using chapter07.ML.Base;
using chapter07.ML.Objects;

using Microsoft.ML;

using Newtonsoft.Json;

namespace chapter07.ML
{
    public class Predictor : BaseML
    {
        public void Predict(string inputDataFile)
        {
            if (!File.Exists(ModelPath))
            {
                Console.WriteLine($"Failed to find model at {ModelPath}");

                return;
            }

            if (!File.Exists(inputDataFile))
            {
                Console.WriteLine($"Failed to find input data at {inputDataFile}");

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

            //1. First, we create our prediction engine with
            PredictionEngine<MusicRating, MusicPrediction> predictionEngine = MlContext.Model.CreatePredictionEngine<MusicRating, MusicPrediction>(mlModel);

            //2. Next, we read the input file into a string object, like this
            string json = File.ReadAllText(inputDataFile);

            //3. Next, we deserialize the string into an object of type MusicRating, like this
            MusicRating rating = JsonConvert.DeserializeObject<MusicRating>(json);

            //4. Lastly, we need to run the prediction, and then output the results of the model
            //run, as follows
            MusicPrediction prediction = predictionEngine.Predict(rating);
            Console.WriteLine(
                $"Based on input:{System.Environment.NewLine}" +
                $"Label: {rating.Label} | MusicID: {rating.MusicID} | UserID: {rating.UserID}{System.Environment.NewLine}" +
                $"The music is {(prediction.Score > Constants.SCORE_THRESHOLD ? "recommended" : "not recommended")}");
        }
    }
}