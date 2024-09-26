using Microsoft.ML.Data;

namespace HelloML.Model
{
    internal class ModelInput
    {
        [LoadColumn(0)]
        public string Month { get; set; }
        [LoadColumn(1)]
        public float Sales { get; set; }
    }
}
