namespace QnA.Models
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class QuestionsAnswersViewModel
    {
        public Questions Questions { get; set; }
        public Answers Answers { get; set; }
    }
}