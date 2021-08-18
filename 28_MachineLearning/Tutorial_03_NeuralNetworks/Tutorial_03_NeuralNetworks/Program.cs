﻿using CsvHelper;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms;
using Microsoft.ML.Vision;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Tensorflow;
using XPlot.Plotly;

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
            var dataView = mlContext.Data.LoadFromEnumerable(images);
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
                FeatureColumnName = "Image",
                LabelColumnName = "LaelAsKey",
                Arch = ImageClassificationTrainer.Architecture.ResnetV250,
                Epoch = 5,
                BatchSize = 10,
                LearningRate = 0.01f,
                MetricsCallback = (metrics) => System.Console.WriteLine(metrics),
                ValidationSet = testSet
            };

            EstimatorChain<KeyToValueMappingTransformer> trainingPipeline = MLContext.MulticlassClassification.Trainers.ImageClassification(options)
                .Append(mlContext.Transforms.Conversion.MapKeyToValue(outputColumnName: "PredictedLabel", inputColumnName: "PredictedLabel"));
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
