using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using QnA.Models;
using PagedList;
using Microsoft.AspNet.Identity;
using System.Net;
using System;

namespace QnA.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Index
        public ActionResult Index(string sort, int? page)
        {
            return QuestionList(sort, page, null, null);
        }

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

        // GET: Questions
        public ActionResult QuestionList(string sort, int? page, string searchType, string searchParam)
        {
            if (sort == null && Session["CurrentSort"] == null) {
                sort = "newer";
            } else if (sort == null && Session["CurrentSort"] != null) {
                sort = Session["CurrentSort"].ToString();
            }
            Session["CurrentSort"] = sort;
            ViewBag.CurrentSort = sort;
            searchType = (searchType ?? "");
            var questions = db.Questions.Include(q => q.User);

            switch (sort)
            {
                case "newer":
                    questions = questions.OrderByDescending(i => i.Date);
                    break;
                case "older":
                    questions = questions.OrderBy(i => i.Date);
                    break;
                case "moreVotes":
                    questions = questions.OrderByDescending(i => i.Votes);
                    break;
                case "lessVotes":
                    questions = questions.OrderBy(i => i.Votes);
                    break;
                case "moreViews":
                    questions = questions.OrderByDescending(i => i.Views);
                    break;
                case "lessViews":
                    questions = questions.OrderBy(i => i.Views);
                    break;
                default:
                    questions = questions.OrderByDescending(i => i.Date);
                    break;
            }

            switch (searchType)
            {
                case "user":
                    questions = questions.Where(q => q.User.Email == searchParam);
                    break;
                case "search":
                    questions = questions.Where(q => q.Title == searchParam && q.Content == searchParam);
                    break;
                default:
                    break;
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(questions.ToPagedList(pageNumber, pageSize));
        }
    }
}