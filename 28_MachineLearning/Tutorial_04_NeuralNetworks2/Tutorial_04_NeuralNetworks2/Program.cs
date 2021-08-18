using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Vision;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Tutorial_04_NeuralNetworks2
{
    public class ImageData
    {
        [LoadColumn(0)]
        public string ImagePath { get; set; }
        [LoadColumn(1)]
        public string Label { get; set; }
    }

    public class ImagePrediction
    {
        public float[] Score { get; set; }
        public uint PredictedLabel { get; set; }
    }

    //https://www.youtube.com/watch?v=bXTN-rnwDso
    class Program
    {
        static void Main(string[] args)
        {
            string imagesFolder = Path.Combine(Environment.CurrentDirectory, "..", "..", "..", "images");
            string[] file = Directory.GetFiles(imagesFolder, "*", SearchOption.AllDirectories);
            IEnumerable<ImageData> images = file.Select(f => new ImageData()
            {
                ImagePath = f,
                Label = Directory.GetParent(f).Name
            });
            MLContext context = new MLContext();
            IDataView imageData = context.Data.LoadFromEnumerable(images);
            IDataView imageDataShuffle = context.Data.ShuffleRows(imageData);

            DataOperationsCatalog.TrainTestData testTrainData = context.Data.TrainTestSplit(imageDataShuffle, testFraction: 0.2);
            IDataView validateData = context.Transforms.Conversion.MapValueToKey("LabelKey", "Label", keyOrdinality: Microsoft.ML.Transforms.ValueToKeyMappingEstimator.KeyOrdinality.ByValue)
                .Fit(testTrainData.TestSet).Transform(testTrainData.TestSet);
            ImageClassificationTrainer.Options options = new ImageClassificationTrainer.Options()
            {
                FeatureColumnName = "ImagePath",
                LabelColumnName = "LaelKey",
                Arch = ImageClassificationTrainer.Architecture.ResnetV2101,
                Epoch = 100,
                BatchSize = 10,
                LearningRate = 0.01f,
                MetricsCallback = (metrics) => System.Console.WriteLine(metrics),
                ValidationSet = validateData
            };
            var pipeline = context.Transforms.Conversion.MapValueToKey("LabelKey", "Label", keyOrdinality: Microsoft.ML.Transforms.ValueToKeyMappingEstimator.KeyOrdinality.ByValue)
                .Append(context.MulticlassClassification.Trainers.ImageClassification(options));
            var model = pipeline.Fit(testTrainData.TrainSet);
            IDataView predictions = model.Transform(testTrainData.TestSet);
            MulticlassClassificationMetrics metrics = context.MulticlassClassification.Evaluate(predictions, labelColumnName: "LabelKey", predictedLabelColumnName: "PredictedLabel");

            Console.WriteLine($"Log loss - {metrics.LogLoss}");

            PredictionEngine<ImageData, ImagePrediction> predictionEngine = context.Model.CreatePredictionEngine<ImageData, ImagePrediction>(model);
            string testImagesFolder = Path.Combine(Environment.CurrentDirectory, "..", "..", "..", "test");
            string[] testFiles = Directory.GetFiles(imagesFolder, "*", SearchOption.AllDirectories);
            IEnumerable<ImageData> testImages = testFiles.Select(f => new ImageData()
            {
                ImagePath = f
            });
            VBuffer<ReadOnlyMemory<char>> keys = default;
            predictionEngine.OutputSchema["LabelKey"].GetKeyValues(ref keys);

            ReadOnlyMemory<char>[] originalLabels = keys.DenseValues().ToArray();

            foreach (var image in testImages)
            {
                ImagePrediction prediction = predictionEngine.Predict(image);
                uint labelIndex = prediction.PredictedLabel;

                Console.WriteLine($"Image: { Path.GetFileName(image.ImagePath)}, Score: {prediction.Score.Max()}" + $"Predicted Label: {originalLabels[labelIndex]}");
            }

            Console.ReadLine();
        }
    }
}
