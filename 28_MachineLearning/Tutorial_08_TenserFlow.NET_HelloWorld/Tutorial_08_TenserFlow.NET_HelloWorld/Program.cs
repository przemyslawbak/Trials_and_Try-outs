using System;
using static Tensorflow.Binding;

namespace Tutorial_08_TenserFlow.NET_HelloWorld
{
    //https://scisharp.github.io/tensorflow-net-docs/#/tutorials/HelloWorld?id=install-the-tensorflownet-sdk
    class Program
    {
        static void Main(string[] args)
        {
            var hello = tf.constant("Hello, TensorFlow!");
            Console.WriteLine(hello);
            Console.WriteLine("Press any key...");
            Console.ReadKey();
        }
    }
}
