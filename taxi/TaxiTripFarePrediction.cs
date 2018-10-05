using Microsoft.ML.Runtime.Api;

namespace taxi
{
    public class TaxiTripFarePrediction
    {
        [ColumnName("Score")]
        public float FareAmount;
    }
}