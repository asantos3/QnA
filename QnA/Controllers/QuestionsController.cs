using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QnA.Models;
using Microsoft.AspNet.Identity;

namespace QnA.Controllers
{
    public class QuestionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Questions
        public ActionResult Index()
        {
            var questions = db.Questions.Include(q => q.User);
            return View(questions.ToList());
        }

        // GET: Questions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Questions questions = db.Questions.Find(id);
            if (questions == null)
            {
                return HttpNotFound();
            }
            return View(questions);
        }

        // POST: Questions/Details/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Details([Bind(Include = "QuestionID,Date,UserID,Content,Votes")] Answers answers)
        {
            if (ModelState.IsValid)
            {
                answers.Date = DateTime.Now;
                answers.Votes = 0;
                answers.UserID = User.Identity.GetUserId();
                db.Answers.Add(answers);
                db.SaveChanges();
                return RedirectToAction("Details");
            }
            return View(answers);
        }

        [Authorize]
        // GET: Questions/Create
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
                return RedirectToAction("Index");
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
            if (questions == null)
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
            if (ModelState.IsValid)
            {
                var x = db.Questions.Where(u => u.ID == questions.ID).First();
                questions.Date = x.Date;
                questions.Votes = x.Votes;
                questions.Views = x.Views;
                questions.UserID = x.UserID;

                db.Entry(x).CurrentValues.SetValues(questions);
                db.SaveChanges();
                return RedirectToAction("Index");
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
            if (questions == null)
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
            db.Questions.Remove(questions);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upvote(int id)
        {
            Questions questions = db.Questions.Find(id);
            if (questions == null)
            {
                return HttpNotFound();
            } else {
                questions.Votes = questions.Votes + 1;
                db.SaveChanges();
                return RedirectToAction("Details/" + id);
            }
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Downvote(int id)
        {
            Questions questions = db.Questions.Find(id);
            if (questions == null)
            {
                return HttpNotFound();
            }
            else {
                questions.Votes = questions.Votes - 1;
                db.SaveChanges();
                return RedirectToAction("Details/"+id);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
