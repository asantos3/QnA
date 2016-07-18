using Microsoft.AspNet.Identity;
using QnA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QnA.Controllers
{
    public class AnswerController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // Upvote an answer
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upvote(int? id)
        {
            Answers answers = db.Answers.Find(id);
            AnswersVotes votes = db.AnswersVotes.Find(id, User.Identity.GetUserId());
            if (answers == null)
            {
                return HttpNotFound();
            }
            else {
                if (votes == null)
                {
                    var x = new AnswersVotes();
                    x.AnswerID = (int)id;
                    x.UserID = User.Identity.GetUserId();
                    x.VotedPositive = true;
                    answers.AnswersVotes.Add(x);
                    answers.Votes = answers.Votes + 1;
                }
                else {
                    // upvote an answer
                    if (votes.VotedPositive == false && votes.VotedNegative == false)
                    {
                        votes.VotedPositive = true;
                        answers.Votes = answers.Votes + 1;
                    }
                    // upvote an answer voted down
                    else if (votes.VotedPositive == false && votes.VotedNegative == true)
                    {
                        votes.VotedPositive = true;
                        votes.VotedNegative = false;
                        answers.Votes = answers.Votes + 2;
                    }
                    // eliminate a upvote given
                    else if (votes.VotedPositive == true && votes.VotedNegative == false)
                    {
                        votes.VotedPositive = false;
                        answers.Votes = answers.Votes - 1;
                    }
                }
                db.SaveChanges();
                return Redirect(ControllerContext.HttpContext.Request.UrlReferrer.ToString());
            }
        }

        // Downvote an answer
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Downvote(int? id)
        {
            Answers answers = db.Answers.Find(id);
            AnswersVotes votes = db.AnswersVotes.Find(id, User.Identity.GetUserId());
            if (answers == null)
            {
                return HttpNotFound();
            }
            else {
                if (votes == null)
                {
                    var x = new AnswersVotes();
                    x.AnswerID = (int)id;
                    x.UserID = User.Identity.GetUserId();
                    x.VotedNegative = true;
                    answers.AnswersVotes.Add(x);
                    answers.Votes = answers.Votes - 1;
                }
                else {
                    // downvote as answer
                    if (votes.VotedPositive == false && votes.VotedNegative == false)
                    {
                        votes.VotedNegative = true;
                        answers.Votes = answers.Votes - 1;
                    }
                    // downvote an answer upvoted
                    else if (votes.VotedPositive == true && votes.VotedNegative == false)
                    {
                        votes.VotedNegative = true;
                        votes.VotedPositive = false;
                        answers.Votes = answers.Votes - 2;
                    }
                    // eliminate a downvote given
                    else if (votes.VotedPositive == false && votes.VotedNegative == true)
                    {
                        votes.VotedNegative = false;
                        answers.Votes = answers.Votes + 1;
                    }
                }
                db.SaveChanges();
                return Redirect(ControllerContext.HttpContext.Request.UrlReferrer.ToString());
            }
        }
    }
}