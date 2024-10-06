using TorchSharp;

namespace Sample
{
    //https://github.com/dotnet/TorchSharpExamples/tree/main/tutorials
    internal class Program
    {
        static void Main(string[] args)
        {
            //Formatting

            torch.ones(2, 3, 3);

            torch.TensorStringStyle = torch.numpy; //Numpy styles

            torch.TensorStringStyle = torch.csharp; //Julia styles
            torch.rand(2, 3, 3);
        }
    }
}