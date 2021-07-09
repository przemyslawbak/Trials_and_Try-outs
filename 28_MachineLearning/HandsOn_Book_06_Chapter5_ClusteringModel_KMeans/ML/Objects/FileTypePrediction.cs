using Microsoft.ML.Data;

namespace chapter05.ML.Objects
{
    //The FileTypePrediction class contains the properties mapped to our prediction output
    public class FileTypePrediction
    {
        [ColumnName("PredictedLabel")]
        public uint PredictedClusterId;

        [ColumnName("Score")]
        public float[] Distances;
    }
}