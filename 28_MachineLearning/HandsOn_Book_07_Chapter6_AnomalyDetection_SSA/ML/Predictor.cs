using System;
using System.IO;

using chapter06.ML.Base;
using chapter06.ML.Objects;

using Microsoft.ML;
using Microsoft.ML.Transforms.TimeSeries;

namespace chapter06.ML
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

            ITransformer mlModel = MlContext.Model.Load(ModelPath, out var modelInputSchema);

            if (mlModel == null)
            {
                Console.WriteLine("Failed to load model");

                return;
            }

            //1. First, we create our prediction engine
            //with the NetworkTrafficHistory and NetworkHistoryPrediction types
            TimeSeriesPredictionEngine<NetworkTrafficHistory, NetworkTrafficPrediction> predictionEngine = mlModel.CreateTimeSeriesEngine<NetworkTrafficHistory, NetworkTrafficPrediction>(MlContext);

            //2. Next, we read the input file into an IDataView variable (note the override to use
            //a comma as separatorChar)
            IDataView inputData =
                MlContext.Data.LoadFromTextFile<NetworkTrafficHistory>(inputDataFile, separatorChar: ',');

            //3. Next, we take the newly created IDataView variable and get an enumerable
            //based off of that data view
            System.Collections.Generic.IEnumerable<NetworkTrafficHistory> rows = MlContext.Data.CreateEnumerable<NetworkTrafficHistory>(inputData, false);

            //4. Lastly, we need to run the prediction and then output the results of the model run
            Console.WriteLine($"Based on input file ({inputDataFile}):");

            foreach (var row in rows)
            {
                var prediction = predictionEngine.Predict(row);

                Console.Write($"HOST: {row.HostMachine} TIMESTAMP: {row.Timestamp} TRANSFER: {row.BytesTransferred} ");
                Console.Write($"ALERT: {prediction.Prediction[0]} SCORE: {prediction.Prediction[1]:f2} P-VALUE: {prediction.Prediction[2]:F2}{Environment.NewLine}");
            }
        }
    }
}