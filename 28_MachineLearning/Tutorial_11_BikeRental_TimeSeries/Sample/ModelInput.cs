namespace HelloML.App
{
    internal class ModelInput
    {
        public DateTime RentalDate { get; set; } //The date of the observation.

        public float Year { get; set; } //The encoded year of the observation (0=2011, 1=2012).

        public float TotalRentals { get; set; } //The total number of bike rentals for that day.
    }
}
