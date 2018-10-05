// data download link https://github.com/dotnet/machinelearning/blob/master/test/data/wikipedia-detox-250-line-data.tsv
// test link: https://github.com/dotnet/machinelearning/blob/master/test/data/wikipedia-detox-250-line-test.tsv
// https://docs.microsoft.com/en-us/dotnet/machine-learning/tutorials/sentiment-analysis
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
using Microsoft.ML.Runtime.Api;

namespace sentimentdata
{
    class Program
    {

        public static string _dataPath = Path.Combine(Environment.CurrentDirectory, "Data", "wikipedia-detox-250-line-data.tsv");
        public static string _testDataPath = Path.Combine(Environment.CurrentDirectory, "Data", "wikipedia-detox-250-line-test.tsv");
        public static string _modelpath = Path.Combine(Environment.CurrentDirectory, "Data", "Model.zip");
        static async Task Main(string[] args)
        {

            Console.WriteLine(_dataPath);
            Console.WriteLine(_testDataPath);
            Console.WriteLine(_modelpath);

            var model = await Train();
            Evaluate(model);
            Predict(model);
        }

        public static async Task<PredictionModel<SentimentData, SentimentPrediction>> Train()
        {
            var pipeline = new LearningPipeline();
            pipeline.Add(new TextLoader(_dataPath).CreateFrom<SentimentData>());
            pipeline.Add(new TextFeaturizer("Features", "SentimentText"));
            pipeline.Add(new FastTreeBinaryClassifier() { NumLeaves = 50, NumTrees = 50, MinDocumentsInLeafs = 20 });
            var model = pipeline.Train<SentimentData, SentimentPrediction>();
            await model.WriteAsync(_modelpath);
            return model;
        }

        public static void Evaluate(PredictionModel<SentimentData, SentimentPrediction> model)
        {
            var testData = new TextLoader(_testDataPath).CreateFrom<SentimentData>();
            var evaluator = new BinaryClassificationEvaluator();
            var metrics = evaluator.Evaluate(model, testData);
            Console.WriteLine("PredictionModel quality metrics evaluation");
            Console.WriteLine($"Accuracy: {metrics.Accuracy:P2}");
            Console.WriteLine($"Auc: {metrics.Auc:P2}");
            Console.WriteLine($"F1Score: {metrics.F1Score:P2}");
        }

        public static void Predict(PredictionModel<SentimentData, SentimentPrediction> model)
        {
            var sentiments = new[]
            {
               new SentimentData
               {
                   SentimentText="Please refrain from adding nonsense to wikipedia"
               },
                new SentimentData
               {
                   SentimentText="He is the best and the article should say that."
               },
                new SentimentData
               {
                   SentimentText="Today was the worst day ever"
               },
                new SentimentData
               {
                   SentimentText="Please do not make personal attacks."
               },
                 new SentimentData
               {
                   SentimentText="you're funny"
               },
                  new SentimentData
               {
                   SentimentText="I will hunt you down and kill you"
               },
                  new SentimentData
               {
                   SentimentText="You make me feel sick"
               },
                new SentimentData
               {
                   SentimentText="you are retard, aren't you?"
               }
           };
            var predictions = model.Predict(sentiments);
            var sentimentsAndPredictions = sentiments.Zip(predictions, (sentiment, prediction) =>
            (sentiment, prediction));

            foreach (var item in sentimentsAndPredictions)
            {
                Console.WriteLine($"Sentiment: {item.sentiment.SentimentText} | Prediction: {(item.prediction.Sentiment ? "Negative" : "Positive")}");
            }
        }
    }
}
