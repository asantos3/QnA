﻿using System;
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
        [Authorize(Roles = "Administrator")]
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
            questions.Views += 1;
            db.SaveChanges();
            return View(questions);
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
            if (questions == null && questions.UserID != User.Identity.GetUserId() || !User.IsInRole("Administrator"))
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
            if (questions == null && questions.UserID != User.Identity.GetUserId() || !User.IsInRole("Administrator"))
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
            if (questions == null && questions.UserID != User.Identity.GetUserId() || !User.IsInRole("Administrator"))
            {
                return HttpNotFound();
            }
            db.Questions.Remove(questions);
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
