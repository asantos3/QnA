namespace QnA.Models
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    // view model used in the Question and Profile view so we have all of the questions and answers
    public class QuestionsAnswersViewModel
    {
        public QuestionsAnswersViewModel()
        {
            this.QuestionsList = new List<Questions>();
            this.AnswersList = new List<Answers>();
        }

        public List<Questions> QuestionsList { get; set; }
        public List<Answers> AnswersList { get; set; }
        public Questions Questions { get; set; }
        public Answers Answers { get; set; }
    }
}