using Microsoft.ML.Data;

namespace MS_Tutorial
{
    /// <summary>
    /// Class to hold the fixed length feature vector. Used to define the
    /// column names used as output from the custom mapping action,
    /// </summary>
    public class FixedLength
    {
        public const int FeatureLength = 600;

        /// <summary>
        /// This is a fixed length vector designated by VectorType attribute.
        /// </summary>
        [VectorType(FeatureLength)]
        public int[] Features { get; set; }
    }
}
