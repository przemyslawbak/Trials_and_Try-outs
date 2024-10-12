using TorchSharp;
using TorchSharp.Modules;
using static TorchSharp.torch;
using static TorchSharp.torch.distributions;
using static TorchSharp.torch.nn;

namespace Sample
{
    //https://github.com/dotnet/TorchSharp/tree/main/src/Examples
    //https://github.com/dotnet/TorchSharp/blob/main/src/Examples/TextClassification.cs

    internal class Program
    {
        private const long emsize = 200;

        private const long batch_size = 128;
        private const long eval_batch_size = 128;

        private const int epochs = 15;

        private readonly static string _dataLocation = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "AG_NEWS");

        static void Main(string[] args)
        {
            torch.random.manual_seed(1); //what for?

            var cwd = Environment.CurrentDirectory; //what for?

            var device = torch.cuda.is_available() ? torch.CUDA : torch.CPU; //CPU will be (nuget?)

            Console.WriteLine($"Running TextClassification on {device.type.ToString()}"); //prints CPU

            using (var reader = TorchText) //how should be data readed?
            {

            }
        }
    }
}