using Synapse.ML.Lightgbm;
using Synapse.ML.Featurize;
using Microsoft.Spark.Sql;
using Microsoft.Spark.Sql.Types;

//https://devblogs.microsoft.com/dotnet/announcing-synapseml-for-dotnet/#getting-started-in-.net

namespace Sample
{
    internal class Program
    {
        private static string trainingDataPath = Path.GetFullPath(@"..\..\..\Data\train_data.csv");

        static void Main(string[] args)
        {
            //https://github.com/dotnet/spark/blob/main/docs/getting-started/windows-instructions.md

            // Create Spark session
            SparkSession spark = SparkSession //exception "System.Net.Internals.SocketExceptionFactory.ExtendedSocketException: 'No connection could be made because the target machine actively refused it. 127.0.0.1:5567'"
                .Builder()
                .AppName("LightGBMExample")
                .GetOrCreate();

            // Load Data
            DataFrame df = spark.Read()
                .Option("inferSchema", true)
                .Parquet("wasbs://publicwasb@mmlspark.blob.core.windows.net/AdultCensusIncome.parquet")
                .Limit(2000);


        }
    }
}