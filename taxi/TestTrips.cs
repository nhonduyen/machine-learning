namespace taxi
{
    public static class TestTrips
    {
        internal static readonly TaxiTrip Trip1 = new TaxiTrip
        {
            VendorId = "VTS",
            RateCode = "1",
            PassengerCount = 1,
            TripDistance = 10.33f,
            PaymentType = "CSH",
            FareAmount = 0 // predict it. actual 29.5
        };

        internal static readonly TaxiTrip Trip2 = new TaxiTrip
        {
            VendorId = "VTS",
            RateCode = "1",
            PassengerCount = 1,
            TripDistance = 1.54f,
            PaymentType = "CRD",
            FareAmount = 0 // predict it. actual 8.5
        };

        internal static readonly TaxiTrip Trip3 = new TaxiTrip
        {
            VendorId = "CMT",
            RateCode = "1",
            PassengerCount = 1,
            TripDistance = 0.34f,
            PaymentType = "CRD",
            FareAmount = 0 // predict it. actual 3
        };
    }
}