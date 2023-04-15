using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Idea1.Models;

namespace Idea1.Controllers
{
    public class StastusController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Stastus
        public ActionResult Index()
        {
            return View(db.Stastus.ToList());
        }

        // GET: Stastus/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stastus stastus = db.Stastus.Find(id);
            if (stastus == null)
            {
                return HttpNotFound();
            }
            return View(stastus);
        }

        // GET: Stastus/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Stastus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Description,Text,DateCreated,UserId")] Stastus stastus)
        {
            if (ModelState.IsValid)
            {
                db.Stastus.Add(stastus);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(stastus);
        }

        // GET: Stastus/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stastus stastus = db.Stastus.Find(id);
            if (stastus == null)
            {
                return HttpNotFound();
            }
            return View(stastus);
        }

        // POST: Stastus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Description,Text,DateCreated,UserId")] Stastus stastus)
        {
            if (ModelState.IsValid)
            {
                db.Entry(stastus).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(stastus);
        }

        // GET: Stastus/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stastus stastus = db.Stastus.Find(id);
            if (stastus == null)
            {
                return HttpNotFound();
            }
            return View(stastus);
        }

        // POST: Stastus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Stastus stastus = db.Stastus.Find(id);
            db.Stastus.Remove(stastus);
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
