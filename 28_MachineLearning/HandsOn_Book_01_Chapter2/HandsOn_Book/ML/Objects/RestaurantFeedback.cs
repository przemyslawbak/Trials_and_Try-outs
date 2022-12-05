using Microsoft.ML.Data;

namespace HandsOn_Book.ML.Objects
{
    public class RestaurantFeedback
    {
        [LoadColumn(0)]
        public bool Label { get; set; }

        [LoadColumn(1)]
        public string Text { get; set; }
    }
}
