using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace TrackClassifier.DataStructures
{
    public class AssessmentScoringPrediction
    {
        [ColumnName("PredictedLabel")]
        public string Track;

        public float[] Score;
    }
}
