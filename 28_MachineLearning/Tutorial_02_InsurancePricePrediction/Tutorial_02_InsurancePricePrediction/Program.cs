using Microsoft.ML;
using Microsoft.ML.AutoML;
using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tutorial_02_InsurancePricePrediction
{
    //https://www.red-gate.com/simple-talk/cloud/data-science/insurance-price-prediction-using-machine-learning-ml-net/
    class Program
    {
        private static string TRAIN_DATA_FILEPATH = @"C:\Chandra\1\Articles\Redgate\Practice\Ins-Price-Prediction\insurance.csv";
        private static string MODEL_FILEPATH = @"C:\Chandra\1\Articles\Redgate\Practice\Ins-Price-Prediction\MLModel.zip";
        static void Main(string[] args)
        {
            //Create ML Context and set a random seed for repeatable/deterministic results across multiple trainings.
            MLContext mlContext = new MLContext(seed: 1);

            //Load Data
            IDataView trainingDataView = mlContext.Data.LoadFromTextFile<ModelInput>(
                                            path: TRAIN_DATA_FILEPATH,
                                            hasHeader: true,
                                            separatorChar: ',',
                                            allowQuoting: true,
                                            allowSparse: false);

            /**********************************************************************
             * Run regression experiment on input data to find the best algorithm *
             **********************************************************************/

            //Set the experiement time in sec
            var settings = new RegressionExperimentSettings
            {
                MaxExperimentTimeInSeconds = 10
            };

            var experiment = mlContext.Auto().CreateRegressionExperiment(settings);

            Console.WriteLine($"*************************************************************************************************************");
            Console.WriteLine($"*                 Trainer Name           RSquared           MeanAbsoluteError");
            Console.WriteLine($"*------------------------------------------------------------------------------------------------------------");

            var progress = new Progress<RunDetail<RegressionMetrics>>(p =>
            {
                if (p.ValidationMetrics != null)
                {
                    Console.Write($"Current result : {p.TrainerName}");
                    Console.Write($"      {p.ValidationMetrics.RSquared}");
                    Console.Write($"      {p.ValidationMetrics.MeanAbsoluteError}");
                    Console.WriteLine();
                }
            });

            var result = experiment.Execute(trainingDataView, labelColumnName: "charges", progressHandler: progress);
            Console.WriteLine($"*************************************************************************************************************");

            Console.WriteLine("Best run:");
            Console.WriteLine($"Trainer name - {result.BestRun.TrainerName}");
            Console.WriteLine($"RSquared - {result.BestRun.ValidationMetrics.RSquared}");
            Console.WriteLine($"MAE - {result.BestRun.ValidationMetrics.MeanAbsoluteError}");

            Console.ReadLine();

            // Data process configuration with pipeline data transformations 
            var dataProcessPipeline = mlContext.Transforms.Conversion.ConvertType(new[] { new InputOutputColumnPair("smoker", "smoker") })
                                      .Append(mlContext.Transforms.Categorical.OneHotEncoding(new[] { new InputOutputColumnPair("sex", "sex"),
                                                                                                      new InputOutputColumnPair("region", "region") }))
                                      .Append(mlContext.Transforms.Concatenate("Features", new[] { "smoker", "sex", "region", "age", "bmi", "children" }));

            // Set the training algorithm, then create and config the modelBuilder
            var trainer = mlContext.Regression.Trainers.LightGbm(labelColumnName: "charges", featureColumnName: "Features");

            //Attach Trainer to data process pipeline
            var trainingPipeline = dataProcessPipeline.Append(trainer);

            //Train the model fitting to the DataSet
            //The pipeline is trained on the dataset that has been loaded and transformed.
            ITransformer model = trainingPipeline.Fit(trainingDataView);

            //Cross-Validate with single dataset 
            var crossValidationResults = mlContext.Regression.CrossValidate(trainingDataView, trainingPipeline,
                                                                            numberOfFolds: 5, labelColumnName: "charges");
            PrintRegressionFoldsAverageMetrics(crossValidationResults);

            // Save the trained model to a .ZIP file
            mlContext.Model.Save(model, trainingDataView.Schema, MODEL_FILEPATH);

            ModelInput sampleData = new ModelInput()
            {
                Age = 19F,
                Sex = @"female",
                Bmi = 27.9F,
                Children = 0F,
                Smoker = true,
                Region = @"southwest",
            };

            //Load the trained model from .Zip file
            ITransformer mlModel = mlContext.Model.Load(MODEL_FILEPATH, out var modelInputSchema);

            // Create prediction engine related to the loaded trained model
            var predEngine = mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(mlModel);

            //Predict the result from prediction engine
            ModelOutput pResult = predEngine.Predict(sampleData);

            Console.WriteLine("Using model to make single prediction -- Comparing actual Price with predicted Price from sample data...\n\n");
            Console.WriteLine($"Age: {sampleData.Age}");
            Console.WriteLine($"Sex: {sampleData.Sex}");
            Console.WriteLine($"Bmi: {sampleData.Bmi}");
            Console.WriteLine($"Children: {sampleData.Children}");
            Console.WriteLine($"Smoker: {sampleData.Smoker}");
            Console.WriteLine($"Region: {sampleData.Region}");
            Console.WriteLine($"\n\nPredicted Price: {pResult.Score}\n\n");
            Console.WriteLine("=============== End of process, hit any key to finish ===============");
            Console.ReadKey();


        }

        public static void PrintRegressionFoldsAverageMetrics(IEnumerable<TrainCatalogBase.CrossValidationResult<RegressionMetrics>> crossValidationResults)
        {
            var L1 = crossValidationResults.Select(r => r.Metrics.MeanAbsoluteError);
            var L2 = crossValidationResults.Select(r => r.Metrics.MeanSquaredError);
            var RMS = crossValidationResults.Select(r => r.Metrics.RootMeanSquaredError);
            var lossFunction = crossValidationResults.Select(r => r.Metrics.LossFunction);
            var R2 = crossValidationResults.Select(r => r.Metrics.RSquared);

            Console.WriteLine($"*************************************************************************************************************");
            Console.WriteLine($"*       Metrics for Regression model      ");
            Console.WriteLine($"*------------------------------------------------------------------------------------------------------------");
            Console.WriteLine($"*       Average L1 Loss:       {L1.Average():0.###} ");
            Console.WriteLine($"*       Average L2 Loss:       {L2.Average():0.###}  ");
            Console.WriteLine($"*       Average RMS:           {RMS.Average():0.###}  ");
            Console.WriteLine($"*       Average Loss Function: {lossFunction.Average():0.###}  ");
            Console.WriteLine($"*       Average R-squared:     {R2.Average():0.###}  ");
            Console.WriteLine($"*************************************************************************************************************");
        }
    }

    public class ModelInput
    {
        [ColumnName("age"), LoadColumn(0)]
        public float Age { get; set; }

        [ColumnName("sex"), LoadColumn(1)]
        public string Sex { get; set; }

        [ColumnName("bmi"), LoadColumn(2)]
        public float Bmi { get; set; }

        [ColumnName("children"), LoadColumn(3)]
        public float Children { get; set; }

        [ColumnName("smoker"), LoadColumn(4)]
        public bool Smoker { get; set; }

        [ColumnName("region"), LoadColumn(5)]
        public string Region { get; set; }

        [ColumnName("charges"), LoadColumn(6)]
        public float Charges { get; set; }
    }

    public class ModelOutput
    {
        public float Score { get; set; }
    }
}
