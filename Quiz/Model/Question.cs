using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz.Model
{
    internal class Question
    {
        private string QuestionTitle;
        private string QuestionText;
        private string[] Answers;
        private int[] CorrectAnswers; 

        public Question(string questionTitle, string questionText, string[] answers, int[] correctAnswers)
        {
            QuestionTitle = questionTitle;
            QuestionText = questionText;
            Answers = answers;
            CorrectAnswers = correctAnswers;
        }
    }
}
