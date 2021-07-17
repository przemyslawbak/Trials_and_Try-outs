using Microsoft.ML.Data;

namespace MS_Tutorial
{
    /// <summary>
    /// GitHubIssue is the input dataset class and has the following String fields:
    ///the first column ID(GitHub Issue ID)
    ///the second column Area(the prediction for training)
    ///the third column Title(GitHub issue title) is the first feature used for predicting the Area
    ///the fourth column Description is the second feature used for predicting the Area

    /// </summary>
    public class GitHubIssue
    {
        [LoadColumn(0)]
        public string ID { get; set; }
        [LoadColumn(1)]
        public string Area { get; set; }
        [LoadColumn(2)]
        public string Title { get; set; }
        [LoadColumn(3)]
        public string Description { get; set; }
    }

    //IssuePrediction is the class used for prediction after the model has been trained.
    public class IssuePrediction
    {
        [ColumnName("PredictedLabel")]
        public string Area;
    }
}
