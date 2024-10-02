namespace Rental.Models
{
    public class RentalPrediction
    {
        public float[] ForecastedRentals
        {
            get; set;
        }
        public float[] LowerBoundRentals
        {
            get; set;
        }
        public float[] UpperBoundRentals
        {
            get; set;
        }
    }
}
