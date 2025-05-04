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
        public List<Answer> Answers { get; set; }

        public Question(string questionTitle, string questionText, List<Answer> answers)
        {
            QuestionTitle = questionTitle;
            QuestionText = questionText;
            Answers = answers;
        }

        
        public override string ToString()
        {
            return $"{QuestionTitle}, {QuestionText}, {Answers}";
        }
    }
}
