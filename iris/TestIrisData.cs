namespace iris
{
    public static class TestIrisData
    {
        internal static readonly IrisData Setota = new IrisData
        {
            SepalLength = 5.1f,
            SepalWidth = 3.5f,
            PetalLength = 1.4f,
            PetalWidth = 0.2f
        };

        internal static readonly IrisData Versicolor = new IrisData
        {
            SepalLength = 7.0f,
            SepalWidth = 3.2f,
            PetalLength = 4.7f,
            PetalWidth = 1.4f
        };

         internal static readonly IrisData Virginica = new IrisData
        {
            SepalLength = 7.4f,
            SepalWidth = 2.8f,
            PetalLength = 6.1f,
            PetalWidth = 1.9f
        };
    }
}