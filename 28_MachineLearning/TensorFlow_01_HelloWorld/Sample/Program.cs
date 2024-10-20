using System;
using static Tensorflow.Binding;

namespace Sample
{
    //https://tensorflownet.readthedocs.io/en/latest/HelloWorld.html
    class Program
    {
        static void Main(string[] args)
        {
            Tensorflow.Tensor hello = tf.constant("Hello, TensorFlow!");
            Console.WriteLine(hello);
            Console.ReadKey();
        }
    }
}