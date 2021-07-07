using System;
using System.IO;

using chapter04_multiclass.ML.Base;
using chapter04_multiclass.ML.Objects;

using Microsoft.ML;

using Newtonsoft.Json;

namespace chapter04_multiclass.ML
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
            //created, Email and EmailPrediction, as follows
            PredictionEngine<Email, EmalPrediction> predictionEngine = MlContext.Model.CreatePredictionEngine<Email, EmalPrediction>(mlModel);

            string json = File.ReadAllText(inputDataFile);

            //2. Given that we are no longer simply passing in the string and building an object
            //on the fly, we need to first read in the file as text.We then deserialize the JSON
            //into our Email object, like this
            EmalPrediction prediction = predictionEngine.Predict(JsonConvert.DeserializeObject<Email>(json));

            //3. Lastly, we need to adjust the output of our prediction to match our new
            //EmailPrediction properties, as follows
            Console.WriteLine(
                                $"Based on input json:{System.Environment.NewLine}" +
                                $"{json}{System.Environment.NewLine}" + 
                                $"The email is predicted to be a {prediction.Category}");
        }
    }
}