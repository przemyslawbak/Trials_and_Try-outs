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
            EstimatorChain<Microsoft.ML.Transforms.ValueToKeyMappingTransformer> pipeline = context.MulticlassClassification.Trainers.ImageClassification(options)
                .Append(context.Transforms.Conversion.MapValueToKey("LabelToKey", "Label", keyOrdinality: Microsoft.ML.Transforms.ValueToKeyMappingEstimator.KeyOrdinality.ByValue));
            TransformerChain<Microsoft.ML.Transforms.ValueToKeyMappingTransformer> model = pipeline.Fit(testTrainData.TrainSet);
            var predictions = model.Transform(testTrainData.TestSet);
            var metrics = context.MulticlassClassification.Evaluate(predictions, labelColumnName: "LabelKey", predictedLabelColumnName: "PredictedLabel");

            Console.WriteLine($"Log loss - {metrics.LogLoss}");

            var predictionEngine = context.Model.CreatePredictionEngine<ImageData, ImagePrediction>(model);
            var testImagesFolder = Path.Combine(Environment.CurrentDirectory, "..", "..", "..", "test");
            var testFiles = Directory.GetFiles(imagesFolder, "*", SearchOption.AllDirectories);
            var testImages = testFiles.Select(f => new ImageData()
            {
                ImagePath = f
            });
        }
    }
}
