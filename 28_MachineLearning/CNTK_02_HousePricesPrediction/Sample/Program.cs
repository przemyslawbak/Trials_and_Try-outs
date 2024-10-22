using Microsoft.ML;
using Microsoft.ML.Data;
using CNTK;
using Sample.CNTKUtil;

namespace Sample
{
    //https://github.com/mdfarragher/DLR/tree/master/Regression/HousePricePrediction

    /// <summary>
    /// The HouseBlockData class holds one single housing block data record.
    /// </summary>
    public class HouseBlockData
    {
        [LoadColumn(0)] public float Longitude { get; set; }
        [LoadColumn(1)] public float Latitude { get; set; }
        [LoadColumn(2)] public float HousingMedianAge { get; set; }
        [LoadColumn(3)] public float TotalRooms { get; set; }
        [LoadColumn(4)] public float TotalBedrooms { get; set; }
        [LoadColumn(5)] public float Population { get; set; }
        [LoadColumn(6)] public float Households { get; set; }
        [LoadColumn(7)] public float MedianIncome { get; set; }
        [LoadColumn(8)] public float MedianHouseValue { get; set; }

        public float[] GetFeatures() => new float[] { Longitude, Latitude, HousingMedianAge, TotalRooms, TotalBedrooms, Population, Households, MedianIncome };

        public float GetLabel() => MedianHouseValue / 1000.0f;
    }

    /// <summary>
    /// The program class.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// The main application entry point.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        static void Main(string[] args)
        {
            //
        }
    }
}