using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrackClassifier.App.Models
{
    public class Track
    {

        public float Score { get; set; }

        public string Name { get; set; }

        public Track(float score, string name)
        {
            Score = score;
            Name = name;
        }

    }
}
