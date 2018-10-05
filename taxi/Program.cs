// https://docs.microsoft.com/en-us/dotnet/machine-learning/tutorials/taxi-fare
// data https://github.com/dotnet/machinelearning/blob/master/test/data/taxi-fare-train.csv
// test https://raw.githubusercontent.com/dotnet/machinelearning/master/test/data/taxi-fare-test.csv
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ML.Legacy;
using Microsoft.ML.Legacy.Models;
using Microsoft.ML.Legacy.Data;
using Microsoft.ML.Legacy.Transforms;
using Microsoft.ML.Legacy.Trainers;


namespace taxi
{
    class Program
    {
        public static string _dataPath = Path.Combine(Environment.CurrentDirectory, "Data", "taxi-fare-train.csv");
        public static string _testDataPath = Path.Combine(Environment.CurrentDirectory, "Data", "taxi-fare-test.csv");
        public static string _modelpath = Path.Combine(Environment.CurrentDirectory, "Data", "Model.zip");
        static async Task Main(string[] args)
        {

            var model = await Train();
            Evaluate(model);
            var prediction = model.Predict(TestTrips.Trip1);
            Console.WriteLine("Predicted fare: {0}, actual fare 29.5", prediction.FareAmount);
            var prediction2 = model.Predict(TestTrips.Trip2);
            Console.WriteLine("Predicted fare: {0}, actual fare 8.5", prediction2.FareAmount);
            var prediction3 = model.Predict(TestTrips.Trip3);
            Console.WriteLine("Predicted fare: {0}, actual fare 3", prediction3.FareAmount);
        }

        public static async Task<PredictionModel<TaxiTrip, TaxiTripFarePrediction>> Train()
        {
            var pipeline = new LearningPipeline
           {
               new TextLoader(_dataPath).CreateFrom<TaxiTrip>(useHeader: true, separator: ','),
               new ColumnCopier(("FareAmount", "Label")),
               new CategoricalHashOneHotVectorizer(
                   "VendorId",
                   "RateCode",
                   "PaymentType"),
                new ColumnConcatenator(
                    "Features",
                    "VendorId",
                    "RateCode",
                    "PassengerCount",
                    "TripDistance",
                    "PaymentType"),
                new FastTreeRegressor()
           };
            var model = pipeline.Train<TaxiTrip, TaxiTripFarePrediction>();
            await model.WriteAsync(_modelpath);
            return model;
        }

        public static void Evaluate(PredictionModel<TaxiTrip, TaxiTripFarePrediction> model)
        {
            var testData = new TextLoader(_testDataPath).CreateFrom<TaxiTrip>(useHeader: true, separator: ',');
            var evaluator = new RegressionEvaluator();
            var metrics = evaluator.Evaluate(model, testData);
            Console.WriteLine("PredictionModel quality metrics evaluation");
            Console.WriteLine($"Rms: {metrics.Rms}");
            Console.WriteLine($"RSquared: {metrics.RSquared}");
        }


    }
}
