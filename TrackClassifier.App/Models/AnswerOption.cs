using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrackClassifier.App.Models
{
    public class AnswerOption
    {

        public string Code { get; set; }

        public string Name { get; set; }

        public int Score { get; set; }

        public bool Selected { get; set; }

        public AnswerOption()
        {
            this.Code = string.Empty;
            this.Selected = true;
        }
    }
}
