using System.IO;

using chapter09.lib.Data;
using chapter09.lib.Helpers;
using chapter09.lib.ML.Base;
using chapter09.lib.ML.Objects;

using Microsoft.ML;

namespace chapter09.lib.ML
{
    public class FileClassificationPredictor : BaseML
    {
        //1. The first Predict method is for our command-line application that simply takes
        //in the filename and is called into the overload in Step 2 after loading in the bytes,
        //as follows
        public FileClassificationResponseItem Predict(string fileName)
        {
            var bytes = File.ReadAllBytes(fileName);

            return Predict(new FileClassificationResponseItem(bytes));
        }

        //2. The second implementation is for our web application that takes the
        //FileClassificationResponseItem object, creates our prediction engine, and
        //returns the prediction data, as follows
        public FileClassificationResponseItem Predict(FileClassificationResponseItem file)
        {
            if (!File.Exists(Common.Constants.MODEL_PATH))
            {
                file.ErrorMessage = $"Model not found ({Common.Constants.MODEL_PATH}) - please train the model first";

                return file;
            }

            ITransformer mlModel;

            using (var stream = new FileStream(Common.Constants.MODEL_PATH, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                mlModel = MlContext.Model.Load(stream, out _);
            }

            var predictionEngine = MlContext.Model.CreatePredictionEngine<FileData, FileDataPrediction>(mlModel);

            var prediction = predictionEngine.Predict(file.ToFileData());

            file.Confidence = prediction.Probability;
            file.IsMalicious = prediction.PredictedLabel;

            return file;
        }
    }
}