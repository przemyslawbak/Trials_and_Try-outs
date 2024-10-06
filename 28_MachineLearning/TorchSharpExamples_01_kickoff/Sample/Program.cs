using TorchSharp;
using TorchSharp.Modules;

namespace Sample
{
    //https://github.com/dotnet/TorchSharpExamples/tree/main/tutorials
    internal class Program
    {
        static void Main(string[] args)
        {
            //Formatting
            //https://github.com/dotnet/TorchSharpExamples/blob/main/tutorials/CSharp/tutorial1.ipynb

            torch.ones(2, 3, 3);

            torch.TensorStringStyle = torch.numpy; //Numpy styles

            torch.TensorStringStyle = torch.csharp; //csharp styles
            torch.rand(2, 3, 3);

            //Tensors
            //https://github.com/dotnet/TorchSharpExamples/blob/main/tutorials/CSharp/tutorial2.ipynb

            //3x4 matrix
            var t = torch.ones(3, 4);

            //printing tensor
            Console.WriteLine(t.ToString(TensorStringStyle.Julia)); //https://github.com/dotnet/TorchSharp/wiki/Tensor-String-Formatting

            var o = torch.ones(2, 4, 4);
            Console.WriteLine(o.ToString(TensorStringStyle.Julia));

            //fill up with empty values
            var e = torch.empty(4, 4);

            //fill up with pre-defined values
            var p = torch.full(4, 4, 3.14f);
            p.ToString(TensorStringStyle.Julia);

            //other way to print
            t.print();

            //other than float32 data type
            torch.zeros(4, 15, dtype: torch.int32);

            //access matrix item (array)
            Console.Write(t[0, 0]);
        }
    }
}