
using Tensorflow.Keras.Engine;
using Tensorflow.Keras.Layers;
using Tensorflow.NumPy;
using static Tensorflow.KerasApi;

//https://www.infoq.com/articles/building-neural-networks-tensorflow-net/

namespace Sample
{
    public class Fnn
    {
        Model model;
        NDArray x_train, y_train, x_test, y_test;

        public void PrepareData()
        {
            (x_train, y_train, x_test, y_test) = keras.datasets.mnist.load_data();
            x_train = x_train.reshape(new Tensorflow.Shape(60000, 784)) / 255f; //modified
            x_test = x_test.reshape(new Tensorflow.Shape(10000, 784)) / 255f; //modified
        }

        public void BuildModel()
        {
            var inputs = keras.Input(shape: 784);

            var layers = new LayersApi();

            var outputs = layers.Dense(64, activation: keras.activations.Relu).Apply(inputs);
            outputs = layers.Dense(10).Apply(outputs);

            model = (Model)keras.Model(inputs, outputs, name: "mnist_model"); //modified
            model.summary();

            model.compile(loss: keras.losses.SparseCategoricalCrossentropy(from_logits: true),
                optimizer: keras.optimizers.Adam(),
                metrics: new[] { "accuracy" });
        }

        public void Train()
        {
            model.fit(x_train, y_train, batch_size: 10, epochs: 2);
            model.evaluate(x_test, y_test);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Fnn fnn = new Fnn();
            fnn.PrepareData();
            fnn.BuildModel();
            fnn.Train();
        }
    }
}