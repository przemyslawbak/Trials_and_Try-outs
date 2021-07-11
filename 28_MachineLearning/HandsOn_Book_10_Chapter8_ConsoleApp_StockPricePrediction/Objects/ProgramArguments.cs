using chapter08.Enums;

namespace chapter08.Objects
{
    //This new class, as referred to earlier in this section, provides the one-to-one mapping of
    //arguments to properties used throughout the application
    public class ProgramArguments
    {
        //1. First, we define the properties that map directly to the command-line arguments
        public ProgramActions Action { get; set; }

        public string TrainingFileName { get; set; }

        public string TestingFileName { get; set; }

        public string PredictionFileName { get; set; }

        public string ModelFileName { get; set; }

        //2. Lastly, we populate default values for the properties
        public ProgramArguments()
        {
            ModelFileName = "chapter8.mdl";

            PredictionFileName = @"..\..\..\Data\predict.csv";

            TrainingFileName = @"..\..\..\Data\sampledata.csv";

            TestingFileName = @"..\..\..\Data\testdata.csv";
        }
    }
}