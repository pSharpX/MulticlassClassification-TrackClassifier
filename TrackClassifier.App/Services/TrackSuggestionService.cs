using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ML;
using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrackClassifier.App.DataStructures;
using TrackClassifier.App.Models;

namespace TrackClassifier.App.Services
{
    public class TrackSuggestionService : ITrackSuggestionService
    {
        private readonly ILogger<TrackSuggestionService> _logger;
        private readonly IQuestionIdentifierService _questionIdentifierService;
        private readonly PredictionEnginePool<AssessmentScoring, AssessmentScoringPrediction> _model;

        public TrackSuggestionService(ILogger<TrackSuggestionService> logger,
            IQuestionIdentifierService questionIdentifierService,
            PredictionEnginePool<AssessmentScoring, AssessmentScoringPrediction> model)
        {
            _logger = logger;
            _questionIdentifierService = questionIdentifierService;
            _model = model;
        }

        public TrackSuggestionResponse Recommend(AssessmentScoringRequest request)
        {
            _logger.LogInformation("BEGIN - Track Recommend execution");
            AssessmentScoring assessmentScoring = new AssessmentScoring();
            request.Questions.ForEach(question => _questionIdentifierService.Fill(assessmentScoring, question));
            AssessmentScoringPrediction prediction = _model.Predict(assessmentScoring);
            FullPrediction[] predictions = GetBestThreePredictions(prediction);
            TrackSuggestionResponse response = new TrackSuggestionResponse();
            response.Tracks = predictions.Select(prediction => new Track(prediction.Score, prediction.PredictedLabel)).ToList();
            _logger.LogInformation("END - Track Recommend execution");
            return response;
        }

        private FullPrediction[] GetBestThreePredictions(AssessmentScoringPrediction prediction)
        {
            float[] scores = prediction.Score;
            int size = scores.Length;
            int index0, index1, index2 = 0;

            VBuffer<ReadOnlyMemory<char>> slotNames = default;
            _model.GetPredictionEngine().OutputSchema[nameof(AssessmentScoringPrediction.Score)].GetSlotNames(ref slotNames);

            GetIndexesOfTopThreeScores(scores, size, out index0, out index1, out index2);

            FullPrediction[] _fullPredictions = new FullPrediction[]
                {
                    new FullPrediction(slotNames.GetItemOrDefault(index0).ToString(),scores[index0],index0),
                    new FullPrediction(slotNames.GetItemOrDefault(index1).ToString(),scores[index1],index1),
                    new FullPrediction(slotNames.GetItemOrDefault(index2).ToString(),scores[index2],index2)
                };

            return _fullPredictions;
        }

        private void GetIndexesOfTopThreeScores(float[] scores, int n, out int index0, out int index1, out int index2)
        {
            int i;
            float first, second, third;
            index0 = index1 = index2 = 0;
            if (n < 3)
            {
                Console.WriteLine("Invalid Input");
                return;
            }
            third = first = second = 000;
            for (i = 0; i < n; i++)
            {
                // If current element is  
                // smaller than first 
                if (scores[i] > first)
                {
                    third = second;
                    second = first;
                    first = scores[i];
                }
                // If arr[i] is in between first 
                // and second then update second 
                else if (scores[i] > second)
                {
                    third = second;
                    second = scores[i];
                }

                else if (scores[i] > third)
                    third = scores[i];
            }
            var scoresList = scores.ToList();
            index0 = scoresList.IndexOf(first);
            index1 = scoresList.IndexOf(second);
            index2 = scoresList.IndexOf(third);
        }

    }
}
