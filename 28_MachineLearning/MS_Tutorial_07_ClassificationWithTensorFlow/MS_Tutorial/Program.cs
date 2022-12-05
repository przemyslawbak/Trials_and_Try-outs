using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms;

namespace MS_Tutorial
{
    class Program
    {
        public const int FeatureLength = 600;
        static readonly string _modelPath = Path.Combine(Environment.CurrentDirectory, "sentiment_model");

        static void Main(string[] args)
        {
            MLContext mlContext = new MLContext();

            IDataView lookupMap = mlContext.Data.LoadFromTextFile(Path.Combine(_modelPath, "imdb_word_index.csv"), columns: new[]
               {
                    new TextLoader.Column("Words", DataKind.String, 0),
                    new TextLoader.Column("Ids", DataKind.Int32, 1),
               }, separatorChar: ',');

            Action<VariableLength, FixedLength> ResizeFeaturesAction = (s, f) =>
            {
                var features = s.VariableLengthFeatures;
                Array.Resize(ref features, FeatureLength);
                f.Features = features;
            };

            //code to load the TensorFlow model
            TensorFlowModel tensorFlowModel = mlContext.Model.LoadTensorFlowModel(_modelPath);
            DataViewSchema schema = tensorFlowModel.GetModelSchema();
            Console.WriteLine(" =============== TensorFlow Model Schema =============== ");
            VectorDataViewType featuresType = (VectorDataViewType)schema["Features"].Type;
            Console.WriteLine($"Name: Features, Type: {featuresType.ItemType.RawType}, Size: ({featuresType.Dimensions[0]})");
            VectorDataViewType predictionType = (VectorDataViewType)schema["Prediction/Softmax"].Type;
            Console.WriteLine($"Name: Prediction/Softmax, Type: {predictionType.ItemType.RawType}, Size: ({predictionType.Dimensions[0]})");

            IEstimator<ITransformer> pipeline =
            // Split the text into individual words
            mlContext.Transforms.Text.TokenizeIntoWords("TokenizedWords", "ReviewText")
            // Map each word to an integer value. The array of integer makes up the input features.
                .Append(mlContext.Transforms.Conversion.MapValue("VariableLengthFeatures", lookupMap,
                    lookupMap.Schema["Words"], lookupMap.Schema["Ids"], "TokenizedWords"))
                // Resize variable length vector to fixed length vector.
                .Append(mlContext.Transforms.CustomMapping(ResizeFeaturesAction, "Resize"))
                // Passes the data to TensorFlow for scoring
                .Append(tensorFlowModel.ScoreTensorFlowModel("Prediction/Softmax", "Features"))
                // Retrieves the 'Prediction' from TensorFlow and copies to a column
                .Append(mlContext.Transforms.CopyColumns("Prediction", "Prediction/Softmax"));
                // Create an executable model from the estimator pipeline
                IDataView dataView = mlContext.Data.LoadFromEnumerable(new List<MovieReview>());
                ITransformer model = pipeline.Fit(dataView);
            PredictSentiment(mlContext, model);
        }

        public static void PredictSentiment(MLContext mlContext, ITransformer model)
        {
            PredictionEngine<MovieReview, MovieReviewSentimentPrediction> engine = mlContext.Model.CreatePredictionEngine<MovieReview, MovieReviewSentimentPrediction>(model);
            MovieReview review = new MovieReview()
            {
                ReviewText = "this film is really good"
            };
            MovieReviewSentimentPrediction sentimentPrediction = engine.Predict(review);
            Console.WriteLine("Number of classes: {0}", sentimentPrediction.Prediction.Length);
            Console.WriteLine("Is sentiment/review positive? {0}", sentimentPrediction.Prediction[1] > 0.5 ? "Yes." : "No.");
        }
    }
}
