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
        // return to QuestionFilter so we can have pagination and filtering on Home/Index
        public ActionResult Index(string sort, int? page, string q, string param)
        {
            ViewBag.q = q;
            ViewBag.param = param;
            return QuestionFilter(sort, page, q, param);
        }

        // GET: Profile
        // information about the user and a list of their questions and answers
        public new ActionResult Profile()
        {
            QuestionsAnswersViewModel qa = new QuestionsAnswersViewModel();

            qa.QuestionsList = db.Questions.Include(m => m.User).Where(x => x.User.UserName == User.Identity.Name).OrderByDescending(i => i.Date).ToList();
            qa.AnswersList = db.Answers.Include(m => m.User).Where(x => x.User.UserName == User.Identity.Name).OrderByDescending(i => i.Date).ToList();

            if (qa.QuestionsList == null || qa.AnswersList == null)
            {
                return HttpNotFound();
            }

            db.SaveChanges();

            return View(qa);
        }

        public ActionResult NotFound()
        {
            return View();
        }

        // Post: Search
        // The search is redirected to the Index action because it's where it's gonna be viewed
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search(string param)
        {
            if (!String.IsNullOrWhiteSpace(param))
            {
                string q = "search";
                return RedirectToAction("Index", new { sort = "newer", page = 1, q = q, param = param });
            }
            else
            {
                TempData["SearchError"] = "The search cannot be empty.";
            }
            return Redirect(HttpContext.Request.UrlReferrer.AbsoluteUri);
        }

        // Filter questions based on parameters: sorting, pagination and filtering/search
        public ActionResult QuestionFilter(string sort, int? page, string q, string param)
        {
            // Session 
            if (sort == null && Session["CurrentSort"] == null)
            {
                sort = "newer";
            }
            else if (sort == null && Session["CurrentSort"] != null)
            {
                sort = Session["CurrentSort"].ToString();
            }
            Session["CurrentSort"] = sort;
            ViewBag.CurrentSort = sort;

            ViewBag.CurrentSearch = (q ?? "all");
            ViewBag.SearchParam = param;
            q = (q ?? "");

            var questions = db.Questions.Include(x => x.User);

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

            // search, or filter by the current user or a tag
            switch (q)
            {
                case "user":
                    questions = questions.Where(x => x.User.UserName == param);
                    break;
                case "search":
                    questions = questions.Where(x => x.Title.Contains(param) || x.Content.Contains(param));
                    break;
                case "tags":
                    try
                    {
                        int tagID = Int32.Parse(param);
                        ViewBag.SearchParam = db.Tags.Find(tagID).Name;
                        questions = questions.Where(x => x.Tags.Any(m => m.TagID == tagID));
                    }
                    catch
                    {
                        ViewBag.CurrentSearch = "all";
                    }
                    break;
                default:
                    break;
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(questions.ToPagedList(pageNumber, pageSize));
        }

        // GET: Home/CookiePolicy
        // Mandatory due to the european cookie law
        public ActionResult CookiePolicy()
        {
            return View();
        }

        // GET: Home/About
        public ActionResult About()
        {
            return View();
        }
    }
}