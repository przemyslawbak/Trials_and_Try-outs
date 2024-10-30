using Tensorflow;

namespace Sample.Utilities
{
    public interface IExample
    {
        ExampleConfig Config { get; set; }
        ExampleConfig InitConfig();
        bool Run();

        void BuildModel();

        /// <summary>
        /// Build dataflow graph, train and predict
        /// </summary>
        /// <returns></returns>
        void Train();
        string FreezeModel();
        void Test();

        void Predict();

        Graph ImportGraph();

        Graph BuildGraph();

        /// <summary>
        /// Prepare dataset
        /// </summary>
        void PrepareData();
    }
}
