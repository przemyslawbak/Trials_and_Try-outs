using System;
using System.IO;

using chapter03.ML.Base;
using chapter03.ML.Objects;

using Microsoft.ML;

using Newtonsoft.Json;

namespace chapter03.ML
{
    public class Predictor : BaseML
    {
        public void Predict(string inputDataFile)
        {
            //1. First, validate that the input file exists before making a prediction on it
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

            //2. The other change is in the prediction call itself. As you probably guessed, the
            //TSrc and TDst arguments need to be adjusted to utilize both of the new classes
            //we created, EmploymentHistory and EmploymentHistoryPrediction
            PredictionEngine<EmploymentHistory, EmploymentHistoryPrediction> predictionEngine = MlContext.Model.CreatePredictionEngine<EmploymentHistory, EmploymentHistoryPrediction>(mlModel);

            //3. Given that we are no longer simply passing in the string and building an object
            //on the fly, we need to first read in the file as text.We then deserialize the JSON
            //into our EmploymentHistory object
            string json = File.ReadAllText(inputDataFile);

            //4. Lastly, we need to adjust the output of our prediction to match our new
            //EmploymentHistoryPrediction properties
            //EmploymentHistoryPrediction prediction = predictionEngine.Predict(JsonConvert.DeserializeObject<EmploymentHistory>(json));

            Console.WriteLine(
                                $"Based on input json:{System.Environment.NewLine}" +
                                $"{json}{System.Environment.NewLine}" + 
                                $"The employee is predicted to work {prediction.DurationInMonths:#.##} months");
        }
    }
}