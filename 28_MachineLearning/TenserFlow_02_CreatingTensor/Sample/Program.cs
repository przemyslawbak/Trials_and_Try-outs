using System;
using Tensorflow;
using Tensorflow.NumPy;
using static Tensorflow.Binding;

namespace Sample
{
    //https://tensorflownet.readthedocs.io/en/latest/HelloWorld.html
    class Program
    {
        static void Main(string[] args)
        {
            // Create a tensor holds a scalar value
            var t1 = new Tensor(3);
            // Init from a string
            var t2 = new Tensor("Hello! TensorFlow.NET");
            // Tensor holds a ndarray
            var nd = new NDArray(new int[] { 3, 1, 1, 2 });
            var t3 = new Tensor(nd);
            Console.WriteLine($"t1: {t1}, t2: {t2}, t3: {t3}");

            Console.ReadKey();
        }
    }
}