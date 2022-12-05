using System;

using chapter05.ML.Base;
using chapter05.ML.Objects;

using Microsoft.ML;
using Microsoft.ML.Data;

namespace chapter05.ML
{
    public class Trainer : BaseML
    {
        //1. The first change is the addition of a GetDataView helper method, which builds
        //the IDataView object from the columns previously defined in the FileData
        //class
        private IDataView GetDataView(string fileName)
        {
            return MlContext.Data.LoadFromTextFile(path: fileName,
                columns: new[]
                {
                    new TextLoader.Column(nameof(FileData.Label), DataKind.Single, 0),
                    new TextLoader.Column(nameof(FileData.IsBinary), DataKind.Single, 1),
                    new TextLoader.Column(nameof(FileData.IsMZHeader), DataKind.Single, 2),
                    new TextLoader.Column(nameof(FileData.IsPKHeader), DataKind.Single, 3)
                },
                hasHeader: false,
                separatorChar: ',');
        }

        public void Train(string trainingFileName, string testingFileName)
        {
            if (!System.IO.File.Exists(trainingFileName))
            {
                Console.WriteLine($"Failed to find training data file ({trainingFileName}");

                return;
            }

            if (!System.IO.File.Exists(testingFileName))
            {
                Console.WriteLine($"Failed to find test data file ({testingFileName}");

                return;
            }

            //2. We then build the data process pipeline, transforming the columns into a single
            //Features column
            IDataView trainingDataView = GetDataView(trainingFileName);

            Microsoft.ML.Transforms.ColumnConcatenatingEstimator dataProcessPipeline = MlContext.Transforms.Concatenate(
                FEATURES,
                nameof(FileData.IsBinary),
                nameof(FileData.IsMZHeader),
                nameof(FileData.IsPKHeader));

            //3. We can then create the k-means trainer with a cluster size of 3 and create the
            //model. The default value for the number of clusters is 5.
            Microsoft.ML.Trainers.KMeansTrainer trainer = MlContext.Clustering.Trainers.KMeans(featureColumnName: FEATURES, numberOfClusters: 3);
            EstimatorChain<ClusteringPredictionTransformer<Microsoft.ML.Trainers.KMeansModelParameters>> trainingPipeline = dataProcessPipeline.Append(trainer);
            TransformerChain<ClusteringPredictionTransformer<Microsoft.ML.Trainers.KMeansModelParameters>> trainedModel = trainingPipeline.Fit(trainingDataView);

            MlContext.Model.Save(trainedModel, trainingDataView.Schema, ModelPath);

            //4. Now we evaluate the model we just trained using the testing dataset
            IDataView testingDataView = GetDataView(testingFileName);

            IDataView testDataView = trainedModel.Transform(testingDataView);

            ClusteringMetrics modelMetrics = MlContext.Clustering.Evaluate(
                data: testDataView,
                labelColumnName: "Label",
                scoreColumnName: "Score",
                featureColumnName: FEATURES);

            //5. Finally, we output all of the classification metrics
            Console.WriteLine($"Average Distance: {modelMetrics.AverageDistance}");
            Console.WriteLine($"Davies Bould Index: {modelMetrics.DaviesBouldinIndex}");
            Console.WriteLine($"Normalized Mutual Information: {modelMetrics.NormalizedMutualInformation}");
        }
    }
}