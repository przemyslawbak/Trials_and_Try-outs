using Microsoft.ML.Data;

namespace MS_Tutorial
{
    /// <summary>
    /// Class to contain the output values from the transformation.
    /// </summary>
    public class MovieReviewSentimentPrediction
    {
        [VectorType(2)]
        public float[] Prediction { get; set; }
    }
}
