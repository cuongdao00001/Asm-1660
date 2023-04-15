using Idea1.Models;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Idea1.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        private ApplicationUserManager _userManager;


        public UserController()
        {
        }

        public UserController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().Get<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        // GET: User/ListUser
        private ApplicationDbContext db = new ApplicationDbContext();
        [CustomAuthorize(Roles = "Admin,QA Manager")]

        public ActionResult Index()
        {
            var users = db.Users.ToList().Select(u => new User(u));
            return View(users);
        }


        // GET: User/Details/5
        [CustomAuthorize(Roles = "Admin,QA Manager")]

        public async Task<ActionResult> Details(string id)
        {
            var user = await UserManager.FindByIdAsync(id);
            return View(new User(user));
        }


        // GET: User/Edit
        // Setting to the Function of Edit in Role
        [CustomAuthorize(Roles = "Admin,QA Manager")]

        public async Task<ActionResult> Edit(string id)
        {
            var user = await UserManager.FindByIdAsync(id);
            return View(new User(user));
        }
        [HttpPost]
        public async Task<ActionResult> Edit(User model)
        {
            var role = model.ApplicationUser;
            await UserManager.UpdateAsync(role);
            return RedirectToAction("Index");
        }
        // GET: User/Delete
        [CustomAuthorize(Roles = "Admin,QA Manager")]

        public async Task<ActionResult> Delete(string id)
        {
            var user = await UserManager.FindByIdAsync(id);
            return View(new User(user));
        }
        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            var user = await UserManager.FindByIdAsync(id);
            await UserManager.DeleteAsync(user);

            return RedirectToAction("Index");
        }
    }
}