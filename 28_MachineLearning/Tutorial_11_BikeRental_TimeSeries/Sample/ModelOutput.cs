namespace HelloML.App
{
    internal class ModelOutput
    {
        public float[] ForecastedRentals { get; set; } //The predicted values for the forecasted period.

        public float[] LowerBoundRentals { get; set; } //The predicted minimum values for the forecasted period.

        public float[] UpperBoundRentals { get; set; } //The predicted maximum values for the forecasted period.
    }
}
