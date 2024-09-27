using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using Taxi.Models;

namespace Sample
{
    internal class Program
    {
        private static readonly string _trainDataPath = Path.GetFullPath(@"..\..\..\Data\taxi-fare-train.csv");
        private static readonly string _testDataPath = Path.GetFullPath(@"..\..\..\Data\taxi-fare-test.csv");
        private static readonly string _modelFarePath = Path.GetFullPath(@"..\..\..\Data\SampleTaxi.Fare.zip");

        static void Main(string[] args)
        {
            var mlContext = new MLContext();

            //load from files
            IDataView dataViewTraining = mlContext.Data.LoadFromTextFile<TaxiTrip>(
                _trainDataPath, hasHeader: true, separatorChar: ',');
            IDataView dataViewTesting = mlContext.Data.LoadFromTextFile<TaxiTrip>(
                _testDataPath, hasHeader: true, separatorChar: ',');

            //remove outliers
            dataViewTraining = RemoveOutliers(mlContext, dataViewTraining);

            //create pipeline for data processing
            var fareDataProcessing = ComposeDataProcessingPipeline(mlContext); //FEATURES <- created here!!!!!

            //train and evaluate
            TrainEvaluateAndSaveModel(mlContext, dataViewTraining, dataViewTesting, fareDataProcessing, _modelFarePath);
        }

        private static IDataView RemoveOutliers(MLContext context, IDataView data)
        {
            var modifiedDataView = context.Data.FilterRowsByColumn(data,
                "FareAmount",
                lowerBound: 1,
                upperBound: 150);
            return modifiedDataView;
        }

        private static IEstimator<ITransformer> ComposeDataProcessingPipeline(MLContext context)
        {
            var pipeline = context.Transforms.CopyColumns("Label", "FareAmount")
                .Append(context.Transforms.Categorical.OneHotEncoding("VendorIdEncoded", "VendorId"))
                .Append(context.Transforms.Categorical.OneHotEncoding("RateCodeEncoded", "RateCode"))
                .Append(context.Transforms.Categorical.OneHotEncoding("PaymentTypeEncoded", "PaymentType"))
                .Append(context.Transforms.NormalizeMeanVariance("PassengerCount"))
                .Append(context.Transforms.NormalizeMeanVariance("TripTime"))
                .Append(context.Transforms.NormalizeMeanVariance("TripDistance"))
                .Append(context.Transforms.Concatenate("Features",
                    "VendorIdEncoded",
                    "RateCodeEncoded",
                    "PaymentTypeEncoded",
                    "PassengerCount",
                    "TripTime",
                    "TripDistance"));
            return pipeline;
        }

        private static void TrainEvaluateAndSaveModel(MLContext mlContext, IDataView dataViewTraining, IDataView dataViewTesting, IEstimator<ITransformer> fareDataProcessing, string modelFarePath)
        {
            //OPTION 1: CROSS VALIDATION OF SEVERAL MODELS

            //pick up algo WITH cross validation
            Console.WriteLine("Pick up algo...");
            var trainer1 = mlContext
                    .Regression
                    .Trainers
                    .OnlineGradientDescent("Label", "Features", lossFunction: new SquaredLoss());

            //use pipeline to process data
            ITransformer dataPrepTransformer = fareDataProcessing.Fit(dataViewTraining);
            IDataView transformedData = dataPrepTransformer.Transform(dataViewTraining);

            //training
            Console.WriteLine("Training models...");
            var results = mlContext.Regression.CrossValidate(transformedData, trainer1, numberOfFolds: 5);
            ITransformer[] models =
                results
                    .OrderByDescending(fold => fold.Metrics.RSquared)
                    .Select(fold => fold.Model)
                    .ToArray();
            var metrics = results
                .OrderByDescending(fold => fold.Metrics.RSquared)
                .Select(fold => fold.Metrics)
                .ToArray();

            //best model
            ITransformer bestModel = models[0];
            RegressionMetrics bestModelMetrics = metrics[0];

            //save best model
            mlContext.Model.Save(bestModel, dataViewTraining.Schema, _modelFarePath);
            Console.WriteLine("The model is saved to {0}\n\n", _modelFarePath);

            //OPTION 2: NO CROSS VALIDATION, JUST EVALUATE AT THE END

            /*var trainer2 = mlContext
                    .Regression
                    .Trainers
                    .Sdca("Label", "Features", lossFunction: new SquaredLoss());
            var trainingPipeline = fareDataProcessing.Append(trainer2);

            // Train the model fitting to the training dataset
            var trainedModel = trainingPipeline.Fit(dataViewTraining);

            // Evaluate the model and show accuracy stats
            IDataView predictions = trainedModel.Transform(dataViewTesting);

            var metrics2 = mlContext.Regression.Evaluate(predictions, labelColumnName: "Label", scoreColumnName: "Score");

            // Save the trained model to a .ZIP file
            mlContext.Model.Save(trainedModel, dataViewTraining.Schema, _modelFarePath);
            Console.WriteLine("The model is saved to {0}\n\n", _modelFarePath);*/
        }
    }
}