using Microsoft.AspNet.Identity;
using QnA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QnA.Controllers
{
    public class QuestionController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Index/Question/5
        public ActionResult Question(int id)
        {
            QuestionsAnswersViewModel qa = new QuestionsAnswersViewModel();
            qa.Questions = db.Questions.Find(id);

            if (qa.Questions == null)
            {
                return HttpNotFound();
            }

            qa.Questions.Views += 1;
            db.SaveChanges();

            return View(qa);
        }

        // POST: Answers/Create
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Question(QuestionsAnswersViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.Answers.Date = DateTime.Now;
                model.Answers.Votes = 0;
                model.Answers.UserID = User.Identity.GetUserId();
                db.Answers.Add(model.Answers);
                db.SaveChanges();
                return Redirect(ControllerContext.HttpContext.Request.UrlReferrer.ToString());
            }

            model.Questions = db.Questions.Find(model.Answers.QuestionID);
            return View(model);
        }

        // Upvote a question
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upvote(int? id)
        {
            Questions questions = db.Questions.Find(id);
            QuestionsVotes votes = db.QuestionsVotes.Find(id, User.Identity.GetUserId());
            if (questions == null)
            {
                return HttpNotFound();
            }
            else {
                if (votes == null)
                {
                    var x = new QuestionsVotes();
                    x.QuestionID = (int)id;
                    x.UserID = User.Identity.GetUserId();
                    x.VotedPositive = true;
                    questions.QuestionsVotes.Add(x);
                    questions.Votes = questions.Votes + 1;
                }
                else {
                    // upvote a question
                    if (votes.VotedPositive == false && votes.VotedNegative == false)
                    {
                        votes.VotedPositive = true;
                        questions.Votes = questions.Votes + 1;
                    }
                    // upvote a question voted down
                    else if (votes.VotedPositive == false && votes.VotedNegative == true)
                    {
                        votes.VotedPositive = true;
                        votes.VotedNegative = false;
                        questions.Votes = questions.Votes + 2;
                    }
                    // eliminate a upvote given
                    else if (votes.VotedPositive == true && votes.VotedNegative == false)
                    {
                        votes.VotedPositive = false;
                        questions.Votes = questions.Votes - 1;
                    }
                }
                db.SaveChanges();
                return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
            }
        }

        // Downvote a question
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Downvote(int? id)
        {
            Questions questions = db.Questions.Find(id);
            QuestionsVotes votes = db.QuestionsVotes.Find(id, User.Identity.GetUserId());
            if (questions == null)
            {
                return HttpNotFound();
            }
            else {
                if (votes == null)
                {
                    var x = new QuestionsVotes();
                    x.QuestionID = (int)id;
                    x.UserID = User.Identity.GetUserId();
                    x.VotedNegative = true;
                    questions.QuestionsVotes.Add(x);
                    questions.Votes = questions.Votes - 1;
                }
                else {
                    // downvote a question
                    if (votes.VotedPositive == false && votes.VotedNegative == false)
                    {
                        votes.VotedNegative = true;
                        questions.Votes = questions.Votes - 1;
                    }
                    // downvote a question upvoted
                    else if (votes.VotedPositive == true && votes.VotedNegative == false)
                    {
                        votes.VotedNegative = true;
                        votes.VotedPositive = false;
                        questions.Votes = questions.Votes - 2;
                    }
                    // eliminate a downvote given
                    else if (votes.VotedPositive == false && votes.VotedNegative == true)
                    {
                        votes.VotedNegative = false;
                        questions.Votes = questions.Votes + 1;
                    }
                }
                db.SaveChanges();
                return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
            }
        }
    }
}