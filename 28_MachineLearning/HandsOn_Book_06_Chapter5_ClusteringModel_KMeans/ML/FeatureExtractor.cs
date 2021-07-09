using System;
using System.IO;

using chapter05.Common;
using chapter05.ML.Base;
using chapter05.ML.Objects;

namespace chapter05.ML
{
    public class FeatureExtractor : BaseML
    {
        //1. First, we generalize the extraction to take the folder path and the output file. As
        //noted earlier, we also pass in the filename, providing the Labeling to occur
        //cleanly inside the FileData class
        private void ExtractFolder(string folderPath, string outputFile)
        {
            if (!Directory.Exists(folderPath))
            {
                Console.WriteLine($"{folderPath} does not exist");

                return;
            }

            string[] files = Directory.GetFiles(folderPath);

            using (var streamWriter =
                new StreamWriter(Path.Combine(AppContext.BaseDirectory, $"../../../Data/{outputFile}")))
            {
                foreach (var file in files)
                {
                    var extractedData = new FileData(File.ReadAllBytes(file), file);

                    streamWriter.WriteLine(extractedData.ToString());
                }
            }

            Console.WriteLine($"Extracted {files.Length} to {outputFile}");
        }

        //2. Lastly, we take the two parameters from the command line (called from the
        //Program class) and simply call the preceding method a second time
        public void Extract(string trainingPath, string testPath)
        {
            ExtractFolder(trainingPath, Constants.SAMPLE_DATA);
            ExtractFolder(testPath, Constants.TEST_DATA);
        }
    }
}