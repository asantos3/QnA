namespace QnA.Models
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    public class Home : DbContext
    {
        public List<Questions> Question { get; set; }
        public List<Answers> Answers { get; set; }
    }
}