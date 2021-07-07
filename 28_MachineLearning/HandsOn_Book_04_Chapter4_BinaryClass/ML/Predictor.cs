using System;
using System.IO;
using chapter04.ML.Base;
using chapter04.ML.Objects;

using Microsoft.ML;

using Newtonsoft.Json;

namespace chapter04.ML
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

            //1. The first change is in the prediction call itself. As you probably guessed, the TSrc
            //and TDst arguments need to be adjusted to utilize both of the new classes we
            //created, CarInventory and CarInventoryPrediction, like this
            PredictionEngine<CarInventory, CarInventoryPrediction> predictionEngine = MlContext.Model.CreatePredictionEngine<CarInventory, CarInventoryPrediction>(mlModel);

            var json = File.ReadAllText(inputDataFile);

            //2. Given that we are no longer simply passing in the string and building an object
            //on the fly, we need to first read in the file as text.We then deserialize the JSON
            //into our CarInventory object, as follows
            CarInventoryPrediction prediction = predictionEngine.Predict(JsonConvert.DeserializeObject<CarInventory>(json));

            //3. Lastly, we need to adjust the output of our prediction to match our
            //new CarInventoryPrediction properties, like this
            Console.WriteLine(
                                $"Based on input json:{System.Environment.NewLine}" +
                                $"{json}{System.Environment.NewLine}" + 
                                $"The car price is a {(prediction.PredictedLabel ? "good" : "bad")} deal, with a {prediction.Probability:P0} confidence");
        }
    }
}