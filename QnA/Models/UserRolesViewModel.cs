namespace QnA.Models
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Web.Mvc;

    // view model used in the ManageAdmins views so we can retrieve all normal
    // regitered users and admins
    public class UserRolesViewModel : DbContext
    {
        public UserRolesViewModel()
        {
            this.Admins = new HashSet<SelectListItem>();
            this.Users = new HashSet<SelectListItem>();
        }

        public IEnumerable<SelectListItem> Admins { get; set; }
        public IEnumerable<SelectListItem> Users { get; set; }
    }
}