using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrackClassifier.App.DataStructures;
using TrackClassifier.App.Models;

namespace TrackClassifier.App.Services
{
    public interface IQuestionIdentifierService
    {
        void Fill(AssessmentScoring assessmentScoring, Question question);
    }
}
