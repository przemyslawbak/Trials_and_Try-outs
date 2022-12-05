using System;
using Tensorflow;
using Tensorflow.NumPy;
using static Tensorflow.Binding;

namespace Sample
{
    //https://tensorflownet.readthedocs.io/en/latest/Constant.html
    //In TensorFlow, a constant is a special Tensor that cannot be modified while the graph is running. 
    class Program
    {
        static void Main(string[] args)
        {
            //Initialize a scalar constant:
            var c1 = tf.constant(3); // int
            var c2 = tf.constant(1.0f); // float
            var c3 = tf.constant(2.0); // double
            var c4 = tf.constant("Big Tree"); // string

            //Initialize a constant through ndarray:
            // dtype=int, shape=(2, 3)
            var nd = np.array(new int[,]
            {
                {1, 2, 3},
                {4, 5, 6}
            });
            var tensor = tf.constant(nd);

            Console.ReadKey();
        }
    }
}