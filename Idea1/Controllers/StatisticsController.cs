using Idea1.Filter;
using Idea1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace Idea1.Controllers
{
    public class StatisticsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Statistics
        [MyAuthenTilter]

        public ActionResult Index()
        {
            return View();
        }
        public List<object> GetStatistics()
        {
            List<object> data = new List<object>();

            // Take to list of department
            List<string> departments = db.Departments.Select(d => d.Name).ToList();
            data.Add(departments);

            List<int> ideaCounts = new List<int>();
            foreach (var department in db.Departments)
            {
                int count = db.Ideas.Count();
                ideaCounts.Add(count);
            }

            data.Add(ideaCounts);
            return data;

        }

        public ActionResult IdeaByDepartment()
        {
            var ideas = db.Ideas.ToList();

            var data = new List<object>();

            var labels = new List<string>();
            var values = new List<int>();

            foreach (var idea in ideas)
            {
                var count = db.Users.Count(u => u.Id == idea.UserId);
                labels.Add(idea.Title);
                values.Add(count);
            }

            data.Add(labels);
            data.Add(values);

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult IdeaCountByDepartment()
        {
            var departments = db.Departments.ToList();

            var data = new List<object>();

            var labels = new List<string>();
            var values = new List<int>();

            foreach (var department in departments)
            {
                var count = db.Users.Count(u => u.DepartmentId == department.DepartmentId);
                labels.Add(department.Name);
                values.Add(count);
            }

            data.Add(labels);
            data.Add(values);

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UserCountByDepartment()
        {
            var departments = db.Departments.ToList();

            var data = new List<object>();

            var labels = new List<string>();
            var values = new List<int>();

            foreach (var department in departments)
            {
                var count = db.Users.Count(u => u.DepartmentId == department.DepartmentId);
                labels.Add(department.Name);
                values.Add(count);
            }

            data.Add(labels);
            data.Add(values);

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetIdeaPie()
        {
            List<Idea> ideas = db.Ideas.ToList();
            var ideaChart = new Chart(width: 600, height: 400)
                .AddTitle("Idea Chart")
                .AddSeries(chartType: "bar",
                    xValue: ideas,
                    xField: "Title",
                    yValues: ideas,
                    yFields: "Views")
                .Write();
            return Json(new { IdeaChart = ideaChart });
        }
        public JsonResult GetIdeaChart()
        {
            List<Idea> ideas = db.Ideas.ToList();
            var ideaChart = new Chart(width: 600, height: 400)
                .AddTitle("Idea Chart")
                .AddSeries(chartType: "bar",
                    xValue: ideas,
                    xField: "Title",
                    yValues: ideas,
                    yFields: "Views")
                .Write();
            return Json(new { IdeaChart = ideaChart });
        }

        public ActionResult IdeaCountByCategory()
        {
            var categories = db.Categories.ToList();
            var data = new List<object>();

            var labels = new List<string>();
            var values = new List<int>();

            foreach (var category in categories)
            {
                var count = db.Ideas.Count(u => u.CategoryId == category.CategoryId);
                labels.Add(category.Name);
                values.Add(count);
            }

            data.Add(labels);
            data.Add(values);

            return Json(data, JsonRequestBehavior.AllowGet);
        }

    }
}
