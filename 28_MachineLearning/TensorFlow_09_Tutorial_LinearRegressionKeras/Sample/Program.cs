using Sample.Utilities;
using static Tensorflow.KerasApi;
using static Tensorflow.Binding;
using static PandasNet.PandasApi;

//https://github.com/SciSharp/SciSharp-Stack-Examples/blob/master/src/TensorFlowNET.Examples/NeuralNetworks/FuelEfficiencyPrediction.cs

namespace Sample
{
    public class FuelEfficiencyPrediction : SciSharpExample, IExample
    {
        public ExampleConfig InitConfig()
        => Config = new ExampleConfig
        {
            Name = "Predict fuel efficiency",
            Enabled = true
        };

        public bool Run()
        {
            PrepareData();
            return true;
        }

        public override void PrepareData()
        {
            string url = $"http://archive.ics.uci.edu/ml/machine-learning-databases/auto-mpg/auto-mpg.data";
            var dataset = pd.read_csv(url,
                names: new[]
                {
                "MPG", "Cylinders", "Displacement", "Horsepower",
                "Weight", "Acceleration", "Model Year", "Origin"
                },
                sep: ' ',
                na_values: '?',
                comment: '\t',
                skipinitialspace: true);
            dataset = dataset.dropna();
            var xx = dataset.data;

            // The "Origin" column is categorical, not numeric.
            // So the next step is to one-hot encode the values in the column with pd.get_dummies.
            dataset["Origin"] = dataset["Origin"].map<string, string>((i) => i switch
            {
                "1" => "USA",
                "2" => "Europe",
                "3" => "Japan",
                _ => "N/A"
            });

            dataset = pd.get_dummies(dataset, columns: new[] { "Origin" }, prefix: "", prefix_sep: "");

            var train_dataset = dataset.sample(frac: 0.8f, random_state: 0);
            var test_dataset = dataset.drop(train_dataset.index.array<int>());

            var train_features = train_dataset.copy();
            var test_features = test_dataset.copy();

            var train_labels = train_features.pop("MPG");
            var test_labels = test_features.pop("MPG");

            // var df = train_dataset.describe().transpose()["mean", "std"];

            var normalizer = tf.keras.layers.Normalization(axis: -1);
            normalizer.adapt(train_features); //problem with dots/commas

            // Linear regression
            var horsepower = train_features["Horsepower"];

            var horsepower_normalizer = layers.Normalization(input_shape: 1, axis: null);
            horsepower_normalizer.adapt(horsepower);

            var horsepower_model = keras.Sequential(horsepower_normalizer,
                layers.Dense(units: 1));

            horsepower_model.summary();

            horsepower_model.compile(
                optimizer: tf.keras.optimizers.Adam(learning_rate: 0.1f),
                loss: tf.keras.losses.MeanAbsoluteError());

            var history = horsepower_model.fit(
                train_features["Horsepower"],
                train_labels,
                epochs: 100,
                // Suppress logging.
                verbose: 1,
                // Calculate validation results on 20% of the training data.
                validation_split: 0.2f);

            var results = horsepower_model.evaluate(
                test_features["Horsepower"],
                test_labels, verbose: 1);

            // Linear regression with multiple inputs
            var linear_model = keras.Sequential(normalizer,
                layers.Dense(units: 1));

            linear_model.compile(
                optimizer: tf.keras.optimizers.Adam(learning_rate: 0.1f),
                loss: tf.keras.losses.MeanAbsoluteError());

            history = linear_model.fit(
                train_features,
                train_labels,
                epochs: 100,
                verbose: 1,
                validation_split: 0.2f);

            linear_model.evaluate(
                test_features, test_labels, verbose: 1);

            // Regression with a deep neural network (DNN)
            var dnn_model = keras.Sequential(normalizer,
                layers.Dense(64, activation: "relu"),
                layers.Dense(64, activation: "relu"),
                layers.Dense(1));

            dnn_model.compile(
                optimizer: tf.keras.optimizers.Adam(learning_rate: 0.001f),
                loss: tf.keras.losses.MeanAbsoluteError());

            history = dnn_model.fit(
                train_features,
                train_labels,
                epochs: 100,
                verbose: 1,
                validation_split: 0.2f);

            dnn_model.evaluate(
                test_features, test_labels, verbose: 1);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            //
        }
    }
}