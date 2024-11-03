using System;
using System.IO;
using System.Linq;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms;
using PLplot;

//https://github.com/mdfarragher/DSC/tree/master/TimeSeries/SalesSpikes

namespace Sample
{
    /// <summary>
    /// The SalesRecord class contains one shampoo sales record.
    /// </summary>
    public class SalesRecord
    {
        [LoadColumn(0)] public string Month;
        [LoadColumn(1)] public float Sales;
    }

    /// <summary>
    /// The SalesPrediction class contains one shampoo sales prediction.
    /// </summary>
    public class SalesPrediction
    {
        //vector to hold alert,score,p-value values
        [VectorType(3)] public double[] Prediction { get; set; }
    }

    internal class Program
    {
        // filenames for training and test data
        private static string dataPath = Path.GetFullPath(@"..\..\..\Data\sales.csv");

        static void Main(string[] args)
        {
            // create the machine learning context
            var context = new MLContext();

            // load the data file
            Console.WriteLine("Loading data...");
            var dataView = context.Data.LoadFromTextFile<SalesRecord>(path: dataPath, hasHeader: true, separatorChar: ',');

            // get an array of data points
            var sales = context.Data.CreateEnumerable<SalesRecord>(dataView, reuseRowObject: false).ToArray();

            // plot the data
            var pl = new PLStream(); //bug fix, not working :( https://github.com/surban/PLplotNet/issues/2#issuecomment-436068021
            pl.sdev("pngcairo");                // png rendering
            pl.sfnam("data.png");               // output filename
            pl.spal0("cmap0_alternate.pal");    // alternate color palette
            pl.init();
            pl.env(
                0, 36,                          // x-axis range
                0, 800,                         // y-axis range
                AxesScale.Independent,          // scale x and y independently
                AxisBox.BoxTicksLabelsAxes);    // draw box, ticks, and num ticks
            pl.lab(
                "Date",                         // x-axis label
                "Sales",                        // y-axis label
                "Shampoo sales over time");     // plot title
            pl.line(
                (from x in Enumerable.Range(0, sales.Count()) select (double)x).ToArray(),
                (from p in sales select (double)p.Sales).ToArray()
            );
            pl.eop();

            // build a training pipeline for detecting spikes
            var pipeline = context.Transforms.DetectIidSpike(
                outputColumnName: nameof(SalesPrediction.Prediction),
                inputColumnName: nameof(SalesRecord.Sales),
                confidence: 95,
                pvalueHistoryLength: sales.Count() / 4); // 25% of x-range

            // train the model
            Console.WriteLine("Detecting spikes...");
            var model = pipeline.Fit(dataView);

            // predict spikes in the data
            var transformed = model.Transform(dataView);
            var predictions = context.Data.CreateEnumerable<SalesPrediction>(transformed, reuseRowObject: false).ToArray();

            // find the spikes in the data
            var spikes = (from i in Enumerable.Range(0, predictions.Count())
                          where predictions[i].Prediction[0] == 1
                          select (Day: i, Sales: sales[i].Sales));

            // plot the spikes
            pl.col0(2);     // blue color
            pl.schr(3, 3);  // scale characters
            pl.string2(
                (from s in spikes select (double)s.Day).ToArray(),
                (from s in spikes select (double)s.Sales + 40).ToArray(),
                "↓");
            pl.eop();
        }
    }
}