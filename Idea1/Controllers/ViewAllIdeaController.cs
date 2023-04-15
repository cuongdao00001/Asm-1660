using Idea1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Net;
using Microsoft.AspNet.Identity;
using System.IO;

namespace Idea1.Controllers
{
    public class ViewAllIdeaController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ViewAllIdea
        public ActionResult Index()
        {
            List<Idea> ideas = db.Ideas.ToList();
            return View(ideas);
        }

        // GET: ViewAllIdea/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.TopicId = id;
            var topicId = Convert.ToInt32(id);
            var topics = db.Topics.Where(t => t.TopicId == topicId).ToList();
            var selectList = new SelectList(topics, "TopicId", "Title");
            ViewBag.TopicId = selectList;
            var categories = db.Categories.ToList();

            ViewBag.CategoryId = new SelectList(categories, "CategoryId", "Name");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Idea idea = db.Ideas.Find(id);

            if (idea == null)
            {
                return HttpNotFound();
            }

            if (!User.IsInRole("Admin") && idea.UserId != User.Identity.GetUserId())
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name", idea.CategoryId);
            ViewBag.TopicId = new SelectList(db.Topics, "TopicId", "Title", idea.TopicId);
            return View(idea);
        }

        // POST: ViewAllIdea/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Idea idea)
        {
            if (ModelState.IsValid)
            {
                db.Entry(idea).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(idea);
        }

        // GET: ViewAllIdea/Delete/5
        public ActionResult Delete(int? id)
        {
            ViewBag.TopicId = id;
            var topicId = Convert.ToInt32(id);
            var topics = db.Topics.Where(t => t.TopicId == topicId).ToList();
            var selectList = new SelectList(topics, "TopicId", "Title");
            ViewBag.TopicId = selectList;
            var categories = db.Categories.ToList();

            ViewBag.CategoryId = new SelectList(categories, "CategoryId", "Name");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Idea idea = db.Ideas.Find(id);

            if (idea == null)
            {
                return HttpNotFound();
            }

            if (!User.IsInRole("Admin") && idea.UserId != User.Identity.GetUserId())
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Name", idea.CategoryId);
            ViewBag.TopicId = new SelectList(db.Topics, "TopicId", "Title", idea.TopicId);
            return View(idea);
        }

        // POST: ViewAllIdea/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Idea idea = db.Ideas.Find(id);
            db.Ideas.Remove(idea);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public FileResult Download(string fileName)
        {
            var fileVirtualPath = "~/Uploads/" + fileName;
            return File(fileVirtualPath, "application/force-download", fileName);
        }
    }
}