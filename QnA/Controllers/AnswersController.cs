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
    public class AnswersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Answers
        public ActionResult Index()
        {
            var answers = db.Answers.Include(a => a.Question).Include(a => a.User);
            return View(answers.ToList());
        }

        // GET: Answers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Answers answers = db.Answers.Find(id);
            if (answers == null)
            {
                return HttpNotFound();
            }
            return View(answers);
        }

        // GET: Answers/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Answers/Create
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "QuestionID,Date,UserID,Content,Votes")] Answers answers)
        {
            if (ModelState.IsValid)
            {
                answers.QuestionID = answers.QuestionID;
                answers.Date = DateTime.Now;
                answers.Votes = 0;
                answers.UserID = User.Identity.GetUserId();
                db.Answers.Add(answers);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(answers);
        }

        // GET: Answers/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Answers answers = db.Answers.Find(id);
            if (answers == null)
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
            if (ModelState.IsValid)
            {
                var x = db.Answers.Where(u => u.ID == answers.ID).First();
                answers.QuestionID = x.QuestionID;
                answers.Date = x.Date;
                answers.Votes = x.Votes;
                answers.UserID = x.UserID;

                db.Entry(x).CurrentValues.SetValues(answers);
                db.SaveChanges();
                return RedirectToAction("Index");
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
            if (answers == null)
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
            db.Answers.Remove(answers);
            db.SaveChanges();
            return RedirectToAction("Index");
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
