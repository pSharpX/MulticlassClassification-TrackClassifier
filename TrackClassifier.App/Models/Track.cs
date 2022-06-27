using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrackClassifier.App.Models
{
    public class Track
    {

        public string Id { get; set; }

        public float Score { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public Track(float score, string name)
        {
            Score = score;
            Name = name;
        }

        public Track(string id, float score, string name, string description)
        {
            Id = id;
            Score = score;
            Name = name;
            Description = description;
        }
    }
}
