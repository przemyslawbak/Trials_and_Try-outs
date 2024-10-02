using Microsoft.ML.Data;

namespace Taxi.Models
{
    public class TaxiFarePrediction
    {
        [ColumnName("Score")]
        public float FareAmount;
    }
}
