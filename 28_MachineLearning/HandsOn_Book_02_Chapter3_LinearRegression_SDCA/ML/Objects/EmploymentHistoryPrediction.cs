using Microsoft.ML.Data;

namespace chapter03.ML.Objects
{
    //The EmploymentHistoryPrediction class contains only the prediction value of how
    //many months the employee is projected to be at his or her job in the DurationInMonths
    //property
    public class EmploymentHistoryPrediction
    {
        [ColumnName("Score")]
        public float DurationInMonths;
    }
}