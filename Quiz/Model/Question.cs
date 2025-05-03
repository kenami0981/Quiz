using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz.Model
{
    internal class Question
    {
        public string QuestionTitle { get; set; }
        public string QuestionText { get; set; }
        public string[] Answers { get; set; }
        public bool[] CorrectAnswers { get; set; }

        public Question(string questionTitle, string questionText, string[] answers, bool[] correctAnswers)
        {
            QuestionTitle = questionTitle;
            QuestionText = questionText;
            Answers = answers;
            CorrectAnswers = correctAnswers;
        }

        
        public override string ToString()
        {
            return $"{QuestionTitle}, {QuestionText}, {Answers}, {CorrectAnswers}";
        }
    }
}
