using Common;
using Microsoft.Extensions.Configuration;
using Microsoft.ML;
using System;
using System.IO;
using TrackClassifier.DataStructures;

namespace TrackClassifier
{
    class Program
    {
        private static string AppPath => Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]);

        private static string BaseDatasetsRelativePath = @"../../../../Data";
        private static string DataSetRelativePath = $"{BaseDatasetsRelativePath}/sfit-track-classifier-train.tsv";
        private static string DataSetLocation = GetAbsolutePath(DataSetRelativePath);

        private static string BaseModelsRelativePath = @"../../../../MLModels";
        private static string ModelRelativePath = $"{BaseModelsRelativePath}/TrackClassifierModel.zip";
        private static string ModelPath = GetAbsolutePath(ModelRelativePath);


        public enum MyTrainerStrategy : int { SdcaMultiClassTrainer = 1, OVAAveragedPerceptronTrainer = 2 };

        public static IConfiguration Configuration { get; set; }
        private static void Main(string[] args)
        {
            SetupAppConfiguration();

            //1. ChainedBuilderExtensions and Train the model
            BuildAndTrainModel(DataSetLocation, ModelPath, MyTrainerStrategy.OVAAveragedPerceptronTrainer);

            //2. Try/test to predict a label for a single hard-coded assessment scoring
            TestSingleLabelPrediction(ModelPath);

            ConsoleHelper.ConsolePressAnyKey();
        }

        public static void BuildAndTrainModel(string DataSetLocation, string ModelPath, MyTrainerStrategy selectedStrategy)
        {
            // Create MLContext to be shared across the model creation workflow objects 
            // Set a random seed for repeatable/deterministic results across multiple trainings.
            var mlContext = new MLContext(seed: 1);

            // STEP 1: Common data loading configuration
            var trainingDataView = mlContext.Data.LoadFromTextFile<AssessmentScoring>(DataSetLocation, hasHeader: true, separatorChar: '\t', allowSparse: false);

            // STEP 2: Common data process configuration with pipeline data transformations
            var dataProcessPipeline = mlContext.Transforms.Conversion.MapValueToKey(outputColumnName: "Label", inputColumnName: nameof(AssessmentScoring.Track))
                            .Append(mlContext.Transforms.Text.FeaturizeText(outputColumnName: "Q7aFeaturized", inputColumnName: nameof(AssessmentScoring.Q7a)))
                            .Append(mlContext.Transforms.Text.FeaturizeText(outputColumnName: "Q7bFeaturized", inputColumnName: nameof(AssessmentScoring.Q7b)))
                            .Append(mlContext.Transforms.Text.FeaturizeText(outputColumnName: "Q7cFeaturized", inputColumnName: nameof(AssessmentScoring.Q7c)))
                            .Append(mlContext.Transforms.Text.FeaturizeText(outputColumnName: "Q7dFeaturized", inputColumnName: nameof(AssessmentScoring.Q7d)))
                            .Append(mlContext.Transforms.Text.FeaturizeText(outputColumnName: "Q8aFeaturized", inputColumnName: nameof(AssessmentScoring.Q8a)))
                            .Append(mlContext.Transforms.Text.FeaturizeText(outputColumnName: "Q8bFeaturized", inputColumnName: nameof(AssessmentScoring.Q8b)))
                            .Append(mlContext.Transforms.Text.FeaturizeText(outputColumnName: "Q8cFeaturized", inputColumnName: nameof(AssessmentScoring.Q8c)))
                            .Append(mlContext.Transforms.Text.FeaturizeText(outputColumnName: "Q8dFeaturized", inputColumnName: nameof(AssessmentScoring.Q8d)))
                            .Append(mlContext.Transforms.Text.FeaturizeText(outputColumnName: "Q9aFeaturized", inputColumnName: nameof(AssessmentScoring.Q9a)))
                            .Append(mlContext.Transforms.Text.FeaturizeText(outputColumnName: "Q9bFeaturized", inputColumnName: nameof(AssessmentScoring.Q9b)))
                            .Append(mlContext.Transforms.Text.FeaturizeText(outputColumnName: "Q9cFeaturized", inputColumnName: nameof(AssessmentScoring.Q9c)))
                            .Append(mlContext.Transforms.Text.FeaturizeText(outputColumnName: "Q9dFeaturized", inputColumnName: nameof(AssessmentScoring.Q9d)))
                            .Append(mlContext.Transforms.Text.FeaturizeText(outputColumnName: "Q9eFeaturized", inputColumnName: nameof(AssessmentScoring.Q9e)))
                            .Append(mlContext.Transforms.Text.FeaturizeText(outputColumnName: "Q9fFeaturized", inputColumnName: nameof(AssessmentScoring.Q9f)))
                            .Append(mlContext.Transforms.Text.FeaturizeText(outputColumnName: "Q10aFeaturized", inputColumnName: nameof(AssessmentScoring.Q10a)))
                            .Append(mlContext.Transforms.Text.FeaturizeText(outputColumnName: "Q10bFeaturized", inputColumnName: nameof(AssessmentScoring.Q10b)))
                            .Append(mlContext.Transforms.Text.FeaturizeText(outputColumnName: "Q10cFeaturized", inputColumnName: nameof(AssessmentScoring.Q10c)))
                            .Append(mlContext.Transforms.Text.FeaturizeText(outputColumnName: "Q10dFeaturized", inputColumnName: nameof(AssessmentScoring.Q10d)))
                            .Append(mlContext.Transforms.Text.FeaturizeText(outputColumnName: "Q10eFeaturized", inputColumnName: nameof(AssessmentScoring.Q10e)))
                            .Append(mlContext.Transforms.Text.FeaturizeText(outputColumnName: "QxaFeaturized", inputColumnName: nameof(AssessmentScoring.Qxa)))
                            .Append(mlContext.Transforms.Text.FeaturizeText(outputColumnName: "QxbFeaturized", inputColumnName: nameof(AssessmentScoring.Qxb)))
                            .Append(mlContext.Transforms.Text.FeaturizeText(outputColumnName: "QxcFeaturized", inputColumnName: nameof(AssessmentScoring.Qxc)))
                            .Append(mlContext.Transforms.Text.FeaturizeText(outputColumnName: "QxdFeaturized", inputColumnName: nameof(AssessmentScoring.Qxd)))
                            .Append(mlContext.Transforms.Concatenate(outputColumnName: "Features"
                            , "Q7aFeaturized", "Q7bFeaturized", "Q7cFeaturized", "Q7dFeaturized"
                            , "Q8aFeaturized", "Q8bFeaturized", "Q8cFeaturized", "Q8dFeaturized"
                            , "Q9aFeaturized", "Q9bFeaturized", "Q9cFeaturized", "Q9dFeaturized", "Q9eFeaturized", "Q9fFeaturized"
                            , "Q10aFeaturized", "Q10bFeaturized", "Q10cFeaturized", "Q10dFeaturized", "Q10eFeaturized"
                            , "QxaFeaturized", "QxbFeaturized", "QxcFeaturized", "QxdFeaturized"))
                            .AppendCacheCheckpoint(mlContext);
            // Use in-memory cache for small/medium datasets to lower training time. 
            // Do NOT use it (remove .AppendCacheCheckpoint()) when handling very large datasets.

            // (OPTIONAL) Peek data (such as 2 records) in training DataView after applying the ProcessPipeline's transformations into "Features" 
            ConsoleHelper.PeekDataViewInConsole(mlContext, trainingDataView, dataProcessPipeline, 2);

            // STEP 3: Create the selected training algorithm/trainer
            IEstimator<ITransformer> trainer = null;
            switch (selectedStrategy)
            {
                case MyTrainerStrategy.SdcaMultiClassTrainer:
                    trainer = mlContext.MulticlassClassification.Trainers.SdcaMaximumEntropy("Label", "Features");
                    break;
                case MyTrainerStrategy.OVAAveragedPerceptronTrainer:
                    {
                        // Create a binary classification trainer.
                        var averagedPerceptronBinaryTrainer = mlContext.BinaryClassification.Trainers.AveragedPerceptron("Label", "Features", numberOfIterations: 10);
                        // Compose an OVA (One-Versus-All) trainer with the BinaryTrainer.
                        // In this strategy, a binary classification algorithm is used to train one classifier for each class, "
                        // which distinguishes that class from all other classes. Prediction is then performed by running these binary classifiers, "
                        // and choosing the prediction with the highest confidence score.
                        trainer = mlContext.MulticlassClassification.Trainers.OneVersusAll(averagedPerceptronBinaryTrainer);

                        break;
                    }
                default:
                    break;
            }

            //Set the trainer/algorithm and map label to value (original readable state)
            var trainingPipeline = dataProcessPipeline.Append(trainer)
                    .Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));

            // STEP 4: Cross-Validate with single dataset (since we don't have two datasets, one for training and for evaluate)
            // in order to evaluate and get the model's accuracy metrics

            Console.WriteLine("=============== Cross-validating to get model's accuracy metrics ===============");
            var crossValidationResults = mlContext.MulticlassClassification.CrossValidate(data: trainingDataView, estimator: trainingPipeline, numberOfFolds: 6, labelColumnName: "Label");

            ConsoleHelper.PrintMulticlassClassificationFoldsAverageMetrics(trainer.ToString(), crossValidationResults);

            // STEP 5: Train the model fitting to the DataSet
            Console.WriteLine("=============== Training the model ===============");
            var trainedModel = trainingPipeline.Fit(trainingDataView);

            // STEP 6: Save/persist the trained model to a .ZIP file
            Console.WriteLine("=============== Saving the model to a file ===============");
            mlContext.Model.Save(trainedModel, trainingDataView.Schema, ModelPath);

            ConsoleHelper.ConsoleWriteHeader("Training process finalized");
        }

        private static void TestSingleLabelPrediction(string modelFilePathName)
        {
            var labeler = new Classifier(modelPath: ModelPath);
            labeler.TestPredictionForSingleAssessmentScoring();
        }
                
        private static void SetupAppConfiguration()
        {
            var builder = new ConfigurationBuilder()
                                        .SetBasePath(Directory.GetCurrentDirectory())
                                        .AddJsonFile("appsettings.json");

            Configuration = builder.Build();
        }

        public static string GetAbsolutePath(string relativePath)
        {
            var _dataRoot = new FileInfo(typeof(Program).Assembly.Location);
            string assemblyFolderPath = _dataRoot.Directory.FullName;

            string fullPath = Path.Combine(assemblyFolderPath, relativePath);

            return fullPath;
        }
    }
}
