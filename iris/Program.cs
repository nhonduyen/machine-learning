// https://docs.microsoft.com/en-us/dotnet/machine-learning/tutorials/iris-clustering
// data https://github.com/dotnet/machinelearning/blob/master/test/data/iris.data
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ML.Legacy;
using Microsoft.ML.Legacy.Data;
using Microsoft.ML.Legacy.Transforms;
using Microsoft.ML.Legacy.Trainers;

namespace iris
{
    class Program
    {
        public static string _dataPath = Path.Combine(Environment.CurrentDirectory, "Data", "iris.data");
        public static string _modelpath = Path.Combine(Environment.CurrentDirectory, "Data", "Model.zip");
        static async Task Main(string[] args)
        {

            var model = await Train();

            var prediction = model.Predict(TestIrisData.Setota);
            var prediction2 = model.Predict(TestIrisData.Versicolor);
            var prediction3 = model.Predict(TestIrisData.Virginica);
            Console.WriteLine($"Cluster: {prediction.PredictedClusterId}");
            Console.WriteLine($"Distance {string.Join(" ", prediction.Distances)}");
            Console.WriteLine($"Cluster: {prediction2.PredictedClusterId}");
            Console.WriteLine($"Distance {string.Join(" ", prediction2.Distances)}");
            Console.WriteLine($"Cluster: {prediction3.PredictedClusterId}");
            Console.WriteLine($"Distance {string.Join(" ", prediction3.Distances)}");
        }

        public static async Task<PredictionModel<IrisData, ClusterPrediction>> Train()
        {
            var pipeline = new LearningPipeline();
            pipeline.Add(new TextLoader(_dataPath).CreateFrom<IrisData>(separator: ','));
            pipeline.Add(new ColumnConcatenator(
                    "Features",
                    "SepalLength",
                    "SepalWidth",
                    "PetalLength",
                    "PetalWidth"));
            pipeline.Add(new KMeansPlusPlusClusterer() { K = 3 });

            var model = pipeline.Train<IrisData, ClusterPrediction>();
            await model.WriteAsync(_modelpath);
            return model;
        }
    }
}
