using System;
using System.IO;
using System.Linq;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms;

namespace Sample
{
    /// <summary>
    /// The Passenger class represents one passenger on the Titanic.
    /// </summary>
    public class Passenger
    {
        public bool Label;
        public float Pclass;
        public string Name;
        public string Sex;
        public string RawAge;
        public float SibSp;
        public float Parch;
        public string Ticket;
        public float Fare;
        public string Cabin;
        public string Embarked;
    }

    /// <summary>
    /// The PassengerPrediction class represents one model prediction. 
    /// </summary>
    public class PassengerPrediction
    {
        [ColumnName("PredictedLabel")] public bool Prediction;
        public float Probability;
        public float Score;
    }

    /// <summary>
    /// The RawAge class is a helper class for a column transformation.
    /// </summary>
    public class FromAge
    {
        public string RawAge;
    }

    /// <summary>
    /// The ProcessedAge class is a helper class for a column transformation.
    /// </summary>
    public class ToAge
    {
        public string Age;
    }

    internal class Program
    {
        // filenames for training and test data
        private static string trainingDataPath = Path.GetFullPath(@"..\..\..\Data\train_data.csv");
        private static string testDataPath = Path.GetFullPath(@"..\..\..\Data\test_data.csv");

        static void Main(string[] args)
        {
            
        }
    }
}