using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz.Model
{
    internal class QuizName
    {
        public string Name { get; set; }
        public List<Question> Questions { get; set; }

        public QuizName(string name)
        {
            Name = name;
            Questions = new List<Question>();
        }
        public void AddQuestion(Question question)
        {
            Questions.Add(question);
        }
    }
}
