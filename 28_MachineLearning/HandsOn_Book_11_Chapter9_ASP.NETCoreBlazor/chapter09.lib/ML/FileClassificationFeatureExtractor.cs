using System;
using System.IO;

using chapter09.lib.Common;
using chapter09.lib.Data;
using chapter09.lib.Helpers;

namespace chapter09.lib.ML
{
    public class FileClassificationFeatureExtractor
    {
        //1. First, our ExtractFolder method takes in the folder path and the output file
        //that will contain our feature extraction, as shown in the following code block
        private void ExtractFolder(string folderPath, string outputFile)
        {
            if (!Directory.Exists(folderPath))
            {
                Console.WriteLine($"{folderPath} does not exist");

                return;
            }

            string[] files = Directory.GetFiles(folderPath);

            using (var streamWriter =
                new StreamWriter(Path.Combine(AppContext.BaseDirectory, $"../../../../{outputFile}")))
            {
                foreach (var file in files)
                {
                    var extractedData = new FileClassificationResponseItem(File.ReadAllBytes(file)).ToFileData();

                    extractedData.Label = !file.Contains("clean");

                    streamWriter.WriteLine(extractedData.ToString());
                }
            }

            Console.WriteLine($"Extracted {files.Length} to {outputFile}");
        }

        //2. Next, we use the Extract method to call both the training and test extraction, as
        //follows
        public void Extract(string trainingPath, string testPath)
        {
            ExtractFolder(trainingPath, Constants.SAMPLE_DATA);
            ExtractFolder(testPath, Constants.TEST_DATA);
        }
    }
}