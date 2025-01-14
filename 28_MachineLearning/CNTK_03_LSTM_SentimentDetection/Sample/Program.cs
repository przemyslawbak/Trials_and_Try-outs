using Sample.CNTKUtil;
using System.IO.Compression;

namespace Sample
{
    //https://github.com/mdfarragher/DLR/tree/master/BinaryClassification/LstmDemo

    /// <summary>
    /// The program class.
    /// </summary>
    internal class Program
    {
        private static readonly string _dataPath = Path.GetFullPath(@"..\..\..\..\Data\imdb_data.zip");

        /// <summary>
        /// The main application entry point.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        [STAThread]
        static void Main(string[] args)
        {
            // check the compute device
            Console.WriteLine("Checking compute device...");
            Console.WriteLine($"  Using: {NetUtil.CurrentDevice.AsString()}");

            // unpack archive
            if (!File.Exists("x_train_imdb.bin"))
            {
                ZipFile.ExtractToDirectory(_dataPath, ".");
            }

            // load training and test data
            Console.WriteLine("Loading data files...");
            var sequenceLength = 500;
            var training_data = DataUtil.LoadBinary<float>("x_train_imdb.bin", 25000, sequenceLength);
            var training_labels = DataUtil.LoadBinary<float>("y_train_imdb.bin", 25000);
            var testing_data = DataUtil.LoadBinary<float>("x_test_imdb.bin", 25000, sequenceLength);
            var testing_labels = DataUtil.LoadBinary<float>("y_test_imdb.bin", 25000);
        }
    }
}