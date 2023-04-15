using Idea1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Idea1.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            var departments = db.Departments.ToList();
            var categories = db.Categories.ToList();
            var topics = db.Topics.ToList();
            var ideas = db.Ideas.ToList();
            return View(new Homepage { Departments = departments,Topics = topics, Ideas = ideas });
        }


    }
}