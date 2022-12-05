using System;

using chapter03_logistic_regression.ML;

namespace chapter03_logistic_regression
{
    class Program
    {
        static void Main(string[] args)
        {
            //1. To begin, we modify the help text to indicate the new extract option that takes a
            //path to the training folder
            if (args.Length != 2)
            {
                Console.WriteLine($"Invalid arguments passed in, exiting.{Environment.NewLine}{Environment.NewLine}Usage:{Environment.NewLine}" +
                                  $"predict <path to input file>{Environment.NewLine}" +
                                  $"or {Environment.NewLine}" +
                                  $"train <path to training data file>{Environment.NewLine}" + 
                                  $"or {Environment.NewLine}" +
                                  $"extract <path to folder>{Environment.NewLine}");

                return;
            }

            //2. In addition, we also need to modify the main switch/case to support the extract
            //argument
            switch (args[0])
            {
                case "extract":
                    new FeatureExtractor().Extract(args[1]);
                    break;
                case "predict":
                    new Predictor().Predict(args[1]);
                    break;
                case "train":
                    new Trainer().Train(args[1]);
                    break;
                default:
                    Console.WriteLine($"{args[0]} is an invalid option");
                    break;
            }
        }
    }
}