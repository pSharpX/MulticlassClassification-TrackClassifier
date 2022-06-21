using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrackClassifier.App.Models;

namespace TrackClassifier.App.Services
{
    public interface ITrackSuggestionService
    {

        TrackSuggestionResponse Recommend(AssessmentScoringRequest request);

    }
}
