using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QnA.Models
{
    public class HomeViewModel
    {
        public IEnumerable<Questions> Questions { get; set; }
        public IEnumerable<Answers> Answers { get; set; }
    }
}