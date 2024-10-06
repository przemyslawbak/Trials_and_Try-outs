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

            // Normal distribution
            torch.randn(3, 4);

            // Uniform distribution between [0,1]
            torch.rand(3, 4);

            // Uniform distribution between [100,110]
            var uni = (torch.rand(3, 4) * 10 + 100);

            //tensors from array
            var arr = new float[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            var tArr = torch.from_array(arr);

            //Since torch.from_array doesn't copy the data, modifying the tensor or the array will affect the other:
            tArr.print();
            arr[0] = 100;
            tArr.print();
            tArr[1] = 200;
            tArr.print();

            //getting array from tensor
            var ten = torch.rand(2, 5);
            TorchSharp.Utils.TensorAccessor<float> ta = ten.data<float>();
            float[] a = ta.ToArray();

            //reshaping
            torch.arange(3.0f, 5.0f, step: 0.1f).reshape(4, 5).str(fltFormat: "0.00");


        }
    }
}