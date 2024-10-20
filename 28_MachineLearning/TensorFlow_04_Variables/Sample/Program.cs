using System;
using Tensorflow;
using static Tensorflow.Binding;

namespace Sample
{
    //https://tensorflownet.readthedocs.io/en/latest/Variable.html
    //The variables in TensorFlow are mainly used to represent variable parameter values in the machine learning model
    class Program
    {
        static void Main(string[] args)
        {
            ResourceVariable x = new ResourceVariable();
            x = tf.Variable(10, name: "x"); //null exception
            using (var session = tf.Session())
            {
                session.run(x.initializer);
                var result = session.run(x);
                Console.WriteLine(result.ToString());

                Console.ReadKey();
            }
        }
    }
}