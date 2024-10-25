using SharpLearning.InputOutput.Csv;
using System;
using System.IO;
using System.Linq;

namespace Sample
{
    internal class Program
    {
        private static string dataPath = Path.GetFullPath(@"..\..\..\Data\winequality-white.csv");

        static void Main(string[] args)
        {
            // Setup the CsvParser
            var parser = new CsvParser(() => new StreamReader(dataPath));

            // the column name in the wine quality data set we want to model.
            var targetName = "quality";

            // read the "quality" column, this is the targets for our learner. 
            var targets = parser.EnumerateRows(targetName)
                .ToF64Vector();

            // read the feature matrix, all columns except "quality",
            // this is the observations for our learner.
            var observations = parser.EnumerateRows(c => c != targetName)
                .ToF64Matrix();


        }
    }
}