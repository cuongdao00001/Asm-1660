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
    public class RoleController : Controller
    {
        // GET: Role
        private ApplicationRoleManger _rolerManager;
        private ApplicationUserManager _userManager;
        public RoleController()
        {
        }

        public RoleController(ApplicationRoleManger roleManger, ApplicationUserManager userManager)
        {
            RoleManager = roleManger;
            UserManager = userManager;
        }

        public ApplicationRoleManger RoleManager
        {
            get
            {
                return _rolerManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManger>();
            }
            private set
            {
                _rolerManager = value;
            }
        }
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        private ApplicationDbContext db = new ApplicationDbContext();
        // // GET: /RoleList
        [CustomAuthorize(Roles = "Admin,QA Manager")]

        public ActionResult Index()
        {
            var roles = db.Roles.ToList().Select(r => new Role()
            {
                RoleID = r.Id,
                RoleName = r.Name
            });
            return View(roles);
        }



        //// GET:  /RoleList/Create
        [CustomAuthorize(Roles = "Admin,QA Manager")]

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Create(Role model)
        {
            var role = new ApplicationRole() { Name = model.RoleName };
            await RoleManager.CreateAsync(role);
            return RedirectToAction("Index");
        }


        // Setting to the Function of Edit in Role
        [CustomAuthorize(Roles = "Admin,QA Manager")]

        public async Task<ActionResult> Edit(string id)
        {
            var role = await RoleManager.FindByIdAsync(id);
            return View(new Role(role));
        }
        [HttpPost]
        public async Task<ActionResult> Edit(Role model)
        {
            var role = new ApplicationRole() { Id = model.RoleID, Name = model.RoleName };
            await RoleManager.UpdateAsync(role);
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Details(string id)
        {
            var role = await RoleManager.FindByIdAsync(id);
            return View(new Role(role));
        }


        // Setting to the Function of Delete in Role
        [CustomAuthorize(Roles = "Admin,QA Manager")]

        public async Task<ActionResult> Delete(string id)
        {
            var role = await RoleManager.FindByIdAsync(id);
            return View(new Role(role));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            var role = await RoleManager.FindByIdAsync(id);
            await RoleManager.DeleteAsync(role);
            return RedirectToAction("Index");
        }
    }
}