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
            //https://devblogs.microsoft.com/dotnet/announcing-synapseml-for-dotnet/#a-.net-example-using-lightgbmclassifier-in-synapseml
            //https://microsoft.github.io/SynapseML/docs/Reference/Dotnet%20Setup/#1-install-net
        }
    }
}