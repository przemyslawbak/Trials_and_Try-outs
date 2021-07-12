using System;
using System.Text;

using chapter09.lib.ML;

using chapter09.trainer.Enums;
using chapter09.trainer.Helpers;
using chapter09.trainer.Objects;

namespace chapter09.trainer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //1. First, we need to register the CodePages encoder instance to properly read the
            //Windows - 1252 encoding from the files as we did in the web application, as
            //follows
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            Console.Clear();

            var arguments = CommandLineParser.ParseArguments<ProgramArguments>(args);

            //2. We then can use a simplified and strongly typed switch case to handle our three
            //actions, as follows
            switch (arguments.Action)
            {
                case ProgramActions.FEATURE_EXTRACTOR:
                    new FileClassificationFeatureExtractor().Extract(arguments.TrainingFolderPath,
                        arguments.TestingFolderPath);
                    break;
                case ProgramActions.PREDICT:
                    var prediction = new FileClassificationPredictor().Predict(arguments.PredictionFileName);

                    Console.WriteLine($"File is {(prediction.IsMalicious ? "malicious" : "clean")} with a {prediction.Confidence:P2}% confidence");
                    break;
                case ProgramActions.TRAINING:
                    new FileClassificationTrainer().Train(arguments.TrainingFileName, arguments.TestingFileName);
                    break;
                default:
                    Console.WriteLine($"Unhandled action {arguments.Action}");
                    break;
            }
        }
    }
}