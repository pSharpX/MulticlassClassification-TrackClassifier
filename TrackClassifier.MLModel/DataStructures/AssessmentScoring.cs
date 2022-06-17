using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace TrackClassifier.DataStructures
{
    public class AssessmentScoring
    {
        [LoadColumn(0)]
        public string Track;

        [LoadColumn(1)]
        public string Q7a;

        [LoadColumn(2)]
        public string Q7b;

        [LoadColumn(3)]
        public string Q7c;

        [LoadColumn(4)]
        public string Q7d;

        [LoadColumn(5)]
        public string Q8a;

        [LoadColumn(6)]
        public string Q8b;

        [LoadColumn(7)]
        public string Q8c;

        [LoadColumn(8)]
        public string Q8d;

        [LoadColumn(9)]
        public string Q9a;

        [LoadColumn(10)]
        public string Q9b;

        [LoadColumn(11)]
        public string Q9c;

        [LoadColumn(12)]
        public string Q9d;

        [LoadColumn(13)]
        public string Q9e;

        [LoadColumn(14)]
        public string Q9f;

        [LoadColumn(15)]
        public string Q10a;

        [LoadColumn(16)]
        public string Q10b;

        [LoadColumn(17)]
        public string Q10c;

        [LoadColumn(18)]
        public string Q10d;

        [LoadColumn(19)]
        public string Q10e;

        [LoadColumn(20)]
        public string Qxa;

        [LoadColumn(21)]
        public string Qxb;

        [LoadColumn(22)]
        public string Qxc;

        [LoadColumn(23)]
        public string Qxd;

    }
}
