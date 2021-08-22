using System;
using Tensorflow;
using Tensorflow.NumPy;
using static Tensorflow.Binding;

namespace Sample
{
    //https://tensorflownet.readthedocs.io/en/latest/LinearRegression.html
    class Program
    {
        static void Main(string[] args)
        {
            tf.compat.v1.disable_eager_execution();
            NumPyImpl nnn = new NumPyImpl();
            // Prepare training Data
            var train_X = np.array(3.3f, 4.4f, 5.5f, 6.71f, 6.93f, 4.168f, 9.779f, 6.182f, 7.59f, 2.167f, 7.042f, 10.791f, 5.313f, 7.997f, 5.654f, 9.27f, 3.1f);
            var train_Y = np.array(1.7f, 2.76f, 2.09f, 3.19f, 1.694f, 1.573f, 3.366f, 2.596f, 2.53f, 1.221f, 2.827f, 3.465f, 1.65f, 2.904f, 2.42f, 2.94f, 1.3f);
            var n_samples = train_X.shape[0];

            // tf Graph Input
            var X = tf.placeholder(tf.float32);
            var Y = tf.placeholder(tf.float32);

            // Set model weights
            var W = tf.Variable(np.random.rand(), name: "weight"); //exception
            var b = tf.Variable(np.random.rand(), name: "bias");

            // Construct a linear model
            var pred = tf.add(tf.multiply(X, W), b);

            // Mean squared error
            var cost = tf.reduce_sum(tf.pow(pred - Y, 2.0f)) / (2.0f * n_samples);

            var optimizer = tf.train.GradientDescentOptimizer(0.01f).minimize(cost);

            Console.WriteLine("Read key...");
            Console.ReadKey();
        }
    }
}