using Microsoft.ML;
using Microsoft.ML.Data;
using CNTK;
using Sample.CNTKUtil;
using XPlot.Plotly;
using System;
using System.IO;
using System.Linq;

namespace Sample
{
    //https://github.com/mdfarragher/DLR/tree/master/BinaryClassification/LstmDemo

    /// <summary>
    /// The program class.
    /// </summary>
    internal class Program
    {
        private static readonly string _dataPath = Path.GetFullPath(@"..\..\..\..\Data\california_housing.csv");

        /// <summary>
        /// The main application entry point.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        [STAThread]
        static void Main(string[] args)
        {
            //
        }
    }
}