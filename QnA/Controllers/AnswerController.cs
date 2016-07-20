using Microsoft.AspNet.Identity;
using QnA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace QnA.Controllers
{
    public class AnswerController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Answers/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Answers answers = db.Answers.Find(id);
            if (answers == null || answers.UserID != User.Identity.GetUserId() || !User.IsInRole("Administrator"))
            {
                return HttpNotFound();
            }
            return View(answers);
        }

        // POST: Answers/Edit/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Content")] Answers answers)
        {
            if (ModelState.IsValid && answers.UserID == User.Identity.GetUserId() || User.IsInRole("Administrator"))
            {
                var x = db.Answers.Find(answers.ID);
                answers.QuestionID = x.QuestionID;
                answers.Date = x.Date;
                answers.Votes = x.Votes;
                answers.UserID = x.UserID;

                db.Entry(x).CurrentValues.SetValues(answers);
                db.SaveChanges();
                return RedirectToAction("Question", "Question", new { id = answers.QuestionID });

            }
            return View(answers);
        }

        // GET: Answers/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Answers answers = db.Answers.Find(id);
            if (answers == null || answers.UserID != User.Identity.GetUserId() || !User.IsInRole("Administrator"))
            {
                return HttpNotFound();
            }
            return View(answers);
        }

        // POST: Answers/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Answers answers = db.Answers.Find(id);
            if (answers == null || answers.UserID != User.Identity.GetUserId() || !User.IsInRole("Administrator"))
            {
                return HttpNotFound();
            }
            db.Answers.Remove(answers);
            db.SaveChanges();
            return RedirectToAction("Question", "Question", new { id = answers.QuestionID });
        }

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