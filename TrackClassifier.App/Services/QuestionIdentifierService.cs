using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using TrackClassifier.App.DataStructures;
using TrackClassifier.App.Models;

namespace TrackClassifier.App.Services
{
    public class QuestionIdentifierService : IQuestionIdentifierService
    {

        private readonly IDictionary<string, Action<AssessmentScoring, Question>> _identifiers;

        public QuestionIdentifierService()
        {
            this._identifiers = new Dictionary<string, Action<AssessmentScoring, Question>>
            {
                { QuestionCatalog.Question7, Question7Identifier },
                { QuestionCatalog.Question8, Question8Identifier },
                { QuestionCatalog.Question9, Question9Identifier },
                { QuestionCatalog.Question10, Question10Identifier },
            }.ToImmutableDictionary();
        }

        public void Fill(AssessmentScoring assessmentScoring, Question question)
        {
            if (_identifiers.ContainsKey(question.CustomId))
            {
                _identifiers[question.CustomId](assessmentScoring, question);
            }
        }

        private void Question7Identifier(AssessmentScoring assessmentScoring, Question question)
        {
            question.Options.ForEach(option => 
            {
                if (option.Code.ToLower().Contains("a"))
                {
                    assessmentScoring.Q7a = option.Selected ? "1": "0";
                } 
                else if (option.Code.ToLower().Contains("b"))
                {
                    assessmentScoring.Q7b = option.Selected ? "1" : "0";
                }
                else if (option.Code.ToLower().Contains("c"))
                {
                    assessmentScoring.Q7c = option.Selected ? "1" : "0";
                }
                else if (option.Code.ToLower().Contains("d"))
                {
                    assessmentScoring.Q7d = option.Selected ? "1" : "0";
                }
            });
        }

        private void Question8Identifier(AssessmentScoring assessmentScoring, Question question)
        {
            question.Options.ForEach(option =>
            {
                if (option.Code.ToLower().Contains("a"))
                {
                    assessmentScoring.Q8a = option.Selected ? "1" : "0";
                }
                else if (option.Code.ToLower().Contains("b"))
                {
                    assessmentScoring.Q8b = option.Selected ? "1" : "0";
                }
                else if (option.Code.ToLower().Contains("c"))
                {
                    assessmentScoring.Q8c = option.Selected ? "1" : "0";
                }
                else if (option.Code.ToLower().Contains("d"))
                {
                    assessmentScoring.Q8d = option.Selected ? "1" : "0";
                }
            });
        }

        private void Question9Identifier(AssessmentScoring assessmentScoring, Question question)
        {
            question.Options.ForEach(option =>
            {
                if (option.Code.ToLower().Contains("a"))
                {
                    assessmentScoring.Q9a = option.Selected ? "1" : "0";
                }
                else if (option.Code.ToLower().Contains("b"))
                {
                    assessmentScoring.Q9b = option.Selected ? "1" : "0";
                }
                else if (option.Code.ToLower().Contains("c"))
                {
                    assessmentScoring.Q9c = option.Selected ? "1" : "0";
                }
                else if (option.Code.ToLower().Contains("d"))
                {
                    assessmentScoring.Q9d = option.Selected ? "1" : "0";
                }
                else if (option.Code.ToLower().Contains("e"))
                {
                    assessmentScoring.Q9e = option.Selected ? "1" : "0";
                }
                else if (option.Code.ToLower().Contains("f"))
                {
                    assessmentScoring.Q9f = option.Selected ? "1" : "0";
                }
            });
        }

        private void Question10Identifier(AssessmentScoring assessmentScoring, Question question)
        {
            question.Options.ForEach(option =>
            {
                if (option.Code.ToLower().Contains("a"))
                {
                    assessmentScoring.Q10a = option.Selected ? "1" : "0";
                }
                else if (option.Code.ToLower().Contains("b"))
                {
                    assessmentScoring.Q10b = option.Selected ? "1" : "0";
                }
                else if (option.Code.ToLower().Contains("c"))
                {
                    assessmentScoring.Q10c = option.Selected ? "1" : "0";
                }
                else if (option.Code.ToLower().Contains("d"))
                {
                    assessmentScoring.Q10d = option.Selected ? "1" : "0";
                }
                else if (option.Code.ToLower().Contains("e"))
                {
                    assessmentScoring.Q10e = option.Selected ? "1" : "0";
                }
            });
        }

    }
}
