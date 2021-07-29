using System;
using System.IO;

using chapter03_logistic_regression.ML.Base;
using chapter03_logistic_regression.ML.Objects;

using Microsoft.ML;

namespace chapter03_logistic_regression.ML
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

            //1. We begin by passing in the two new classes, FileInput and
            //FilePrediction, to the CreatePredictionEngine method
            PredictionEngine<FileInput, FilePrediction> predictionEngine = MlContext.Model.CreatePredictionEngine<FileInput, FilePrediction>(mlModel);

            //2. Next, we create the FileInput object, setting the Strings property with the
            //return value of the GetStrings method we wrote earlier
            FilePrediction prediction = predictionEngine.Predict(new FileInput
            {
                //Strings = GetStrings(File.ReadAllBytes(inputDataFile))
            });

            //3. Finally, we update the output call to the Console object with our file
            //classification and probability
            Console.WriteLine(
                                $"Based on the file ({inputDataFile}) the file is classified as {(prediction.IsMalicious ? "malicious" : "benign")}" + 
                                $" at a confidence level of {prediction.Probability:P0}");
        }
    }
}