using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using QnA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QnA.Controllers
{
    public class AdministratorController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: AddAdmin
        [Authorize(Roles = "Administrator")]
        public ActionResult ManageAdmin()
        {
            UserRolesViewModel UserRoles = new UserRolesViewModel();
            UserRoles.Admins = db.Users.Where(x => x.Roles.Any(y => y.RoleId == "1")).Select(x => new SelectListItem { Value = x.Id, Text = x.UserName });
            UserRoles.Users = db.Users.Where(x => x.Roles.All(y => y.RoleId != "1")).Select(x => new SelectListItem { Value = x.Id, Text = x.UserName});
            return View(UserRoles);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddAdmin(string Users)
        {
            if (Users != null)
            {
                var _context = new ApplicationDbContext();
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));
                UserManager.AddToRole(Users, "Administrator");
            }
            
            return RedirectToAction("ManageAdmin");
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveAdmin(string Admins)
        {
            if (Admins != null)
            {
                var _context = new ApplicationDbContext();
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));
                UserManager.RemoveFromRole(Admins, "Administrator");
            }

            return RedirectToAction("ManageAdmin");
        }
    }
}