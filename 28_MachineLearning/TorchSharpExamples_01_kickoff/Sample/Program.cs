﻿using TorchSharp;
using TorchSharp.Modules;
using static TorchSharp.torch.distributions;

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
            float[] f = ta.ToArray();

            //reshaping
            torch.arange(3.0f, 5.0f, step: 0.1f).reshape(4, 5).str(fltFormat: "0.00");

            //Basic Numerics
            //https://github.com/dotnet/TorchSharpExamples/blob/main/tutorials/CSharp/tutorial3.ipynb

            //arithm
            var a = torch.ones(3, 4);
            var b = torch.ones(3, 4);
            var c = torch.tensor(5);
            var arithmetic = a * c + b; //6x6x6
            Console.WriteLine(arithmetic.ToString(TensorStringStyle.Julia));
            Console.WriteLine(arithmetic.shape);

            //Random Numbers and Distributions
            //https://github.com/dotnet/TorchSharpExamples/blob/main/tutorials/CSharp/tutorial4.ipynb

            torch.rand(10).print();
            torch.randn(10).print();
            torch.randint(100, 10).print();
            torch.randperm(25).print();

            //seed number
            torch.random.manual_seed(4711);
            torch.rand(10).print();
            torch.random.manual_seed(17);
            torch.rand(10).print();
            torch.random.manual_seed(4711);
            torch.rand(10).print();

            //coin toss
            var bern = new Bernoulli(torch.tensor(0.5f));
            bern.sample().item<float>();

            //Categories
            var cat = new Categorical(torch.tensor(new float[] { 0.1f, 0.7f, 0.1f, 0.1f }));
            cat.sample(4);

            //Real-valued Distributions
            torch.Tensor foo(Distribution dist) { return dist.sample(4, 4); }

            var norm1 = Normal(torch.tensor(0.5f), torch.tensor(0.125f));
            var norm2 = Normal(torch.tensor(0.15f), torch.tensor(0.025f));

            foo(norm1).print();
            foo(norm2).print();

            //Generator
            torch.Generator gen1 = torch.random.manual_seed(17);
            torch.rand(2, 3, generator: gen1).print();
            torch.randn(2, 3, generator: gen1).print();

            var gen2 = new torch.Generator(189);
            var gen3 = new torch.Generator(189);

            torch.rand(2, 3, generator: gen2).print();
            torch.rand(2, 3, generator: gen3).print();
            torch.rand(2, 3, generator: gen2).print();
            torch.rand(2, 3, generator: gen3).print();

            norm1 = Normal(torch.tensor(0.5f), torch.tensor(0.125f), generator: gen2);
            norm1.sample(10);
        }
    }
}