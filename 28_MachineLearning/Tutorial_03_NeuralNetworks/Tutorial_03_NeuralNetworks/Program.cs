using CsvHelper;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms;
using Microsoft.ML.Vision;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Tutorial_03_NeuralNetworks
{
    public class ImageData
    {
        public string ImagePath { get; set; }
        public string Label { get; set; }
    }

    public class Article
    {
        public string Id { get; set; }
        public string Gender { get; set; }
        public string MasterCategory { get; set; }
        public string SubCategory { get; set; }
        public string ArticleType { get; set; }
        public string BaseColor { get; set; }
        public string Season { get; set; }
        public string Year { get; set; }
        public string Usage { get; set; }
        public string ProductDisplayName { get; set; }
    }

    //https://www.youtube.com/watch?v=onxRLTc2SaA

    class Program
    {
        private static readonly string imagePath = "imagePath";
        private static readonly string csvPath = "csvPath";

        static void Main(string[] args)
        {
            IEnumerable<ImageData> images = GetImages(csvPath);
            MLContext mlContext = new MLContext(seed: 1);
            IDataView dataView = mlContext.Data.LoadFromEnumerable(images);
            dataView = mlContext.Data.ShuffleRows(dataView);

            IDataView shuffledFullImagesDataset = mlContext.Transforms.Conversion.MapValueToKey(
                outputColumnName: "LabelAsKey",
                inputColumnName: "Label",
                keyOrdinality: ValueToKeyMappingEstimator.KeyOrdinality.ByValue)
                .Append(mlContext.Transforms.LoadRawImageBytes(
                    outputColumnName: "Image",
                    imageFolder: "imagePath",
                    inputColumnName: "ImagePath"))
                .Fit(dataView)
                .Transform(dataView);

            //data exploration
            IEnumerable<string> categories = shuffledFullImagesDataset.GetColumn<string>("Label");

            //split data
            DataOperationsCatalog.TrainTestData trainTestSplit = mlContext.Data.TrainTestSplit(shuffledFullImagesDataset);
            IDataView testSet = trainTestSplit.TestSet;
            IDataView trainSet = trainTestSplit.TrainSet;

            //data transformation
            ImageClassificationTrainer.Options options = new ImageClassificationTrainer.Options()
            {
                FeatureColumnName = "Image", //feature type
                LabelColumnName = "LaelAsKey",
                Arch = ImageClassificationTrainer.Architecture.ResnetV250, //architecture type
                Epoch = 5, //pass all images 5 times
                BatchSize = 10, //each time passing 10 images
                LearningRate = 0.01f, //optimizing, defaults should be fine
                MetricsCallback = (metrics) => System.Console.WriteLine(metrics),
                ValidationSet = testSet
            };

            EstimatorChain<KeyToValueMappingTransformer> trainingPipeline = mlContext.MulticlassClassification.Trainers.ImageClassification(options)
                .Append(mlContext.Transforms.Conversion.MapKeyToValue(outputColumnName: "PredictedLabel", inputColumnName: "PredictedLabel"));

            //training model
            ITransformer model = trainingPipeline.Fit(trainSet);

            //evaluate the model
            IDataView predictions = model.Transform(testSet);
            MulticlassClassificationMetrics metrics = mlContext.MulticlassClassification.Evaluate(predictions, labelColumnName: "LabelAsKey", predictedLabelColumnName: "PredictedLabel");

            //display evaluation
            //can use XPlot
        }

        public static IEnumerable<ImageData> GetImages(string path)
        {
            using (StreamReader reader = new StreamReader(path))
            using (CsvReader csv = new CsvReader(reader, CultureInfo.CreateSpecificCulture("enUS")))
            {
                return csv.GetRecords<Article>()
                    .Select(x => new ImageData
                    {
                        Label = x.ArticleType,
                        ImagePath = Path.Combine(imagePath, x.Id) + ".jpg"
                    })
                    .Where(y => File.Exists(y.ImagePath))
                    .ToList();
            }
        }
    }
}
