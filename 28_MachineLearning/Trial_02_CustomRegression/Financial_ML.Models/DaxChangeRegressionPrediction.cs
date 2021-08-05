using Microsoft.ML.Data;

namespace Financial_ML.Models
{
    public class DaxChangeRegressionPrediction
    {
        [ColumnName("Score")]
        public float Score { get; set; }
    }
}
