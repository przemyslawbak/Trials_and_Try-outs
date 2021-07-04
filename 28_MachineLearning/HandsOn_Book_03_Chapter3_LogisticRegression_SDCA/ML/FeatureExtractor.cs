using System;
using System.IO;
using System.Text;

using chapter03_logistic_regression.Common;
using chapter03_logistic_regression.ML.Base;

namespace chapter03_logistic_regression.ML
{
    //Once extraction is complete, the classification and strings data is written out to the sampledata file
    public class FeatureExtractor : BaseML
    {
        public void Extract(string folderPath)
        {
            string[] files = Directory.GetFiles(folderPath);

            using (StreamWriter streamWriter = new StreamWriter(Path.Combine(AppContext.BaseDirectory, $"../../../Data/{Constants.SAMPLE_DATA}")))
            {
                foreach (string file in files)
                {
                    var strings = GetStrings(File.ReadAllBytes(file));

                    streamWriter.WriteLine($"{file.ToLower().Contains("malicious")}\t{strings}");
                }
            }

            Console.WriteLine($"Extracted {files.Length} to {Constants.SAMPLE_DATA}");
        }
    }
}