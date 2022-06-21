using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrackClassifier.App.Models
{
    public class Question
    {
        public string CustomId { get; set; }

        public string Name { get; set; }

        public string OptionType { get; set; }

        public string Category { get; set; }

        public List<AnswerOption> Options { get; set; }

    }
}
