﻿using Microsoft.AspNet.Identity;
using QnA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
                return RedirectToAction(model.Answers.QuestionID + "");
            }

            model.Questions = db.Questions.Find(model.Answers.QuestionID);
            return View(model);
        }

        // GET: Questions/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Questions/Create
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Title,Content,Date,Votes,Views,UserID")] Questions questions)
        {
            if (ModelState.IsValid)
            {
                var x = new QuestionsTags();
                x.Tag = new Tags();
                x.Tag.Name = "Teste"; // test value
                x.Question = questions;
                questions.Tags.Add(x);

                questions.Date = DateTime.Now;
                questions.Votes = 0;
                questions.Views = 0;
                questions.UserID = User.Identity.GetUserId();
                db.Questions.Add(questions);
                db.SaveChanges();
                return RedirectToAction(questions.ID + "");
            }
            return View(questions);
        }

        // GET: Questions/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Questions questions = db.Questions.Find(id);
            if (questions == null || questions.UserID != User.Identity.GetUserId() || !User.IsInRole("Administrator"))
            {
                return HttpNotFound();
            }
            return View(questions);
        }

        // POST: Questions/Edit/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Title,Content")] Questions questions)
        {
            if (ModelState.IsValid && questions.UserID == User.Identity.GetUserId() || User.IsInRole("Administrator"))
            {
                var x = db.Questions.Find(questions.ID);
                questions.Date = x.Date;
                questions.Votes = x.Votes;
                questions.Views = x.Views;
                questions.UserID = x.UserID;

                db.Entry(x).CurrentValues.SetValues(questions);
                db.SaveChanges();
                return RedirectToAction(questions.ID + "");
            }
            return View(questions);
        }

        // GET: Questions/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Questions questions = db.Questions.Find(id);
            if (questions == null || questions.UserID != User.Identity.GetUserId() || !User.IsInRole("Administrator"))
            {
                return HttpNotFound();
            }
            return View(questions);
        }

        // POST: Questions/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Questions questions = db.Questions.Find(id);
            if (questions == null || questions.UserID != User.Identity.GetUserId() || !User.IsInRole("Administrator"))
            {
                return HttpNotFound();
            }
            db.Questions.Remove(questions);
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
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