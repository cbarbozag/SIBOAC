using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Cosevi.SIBOAC.Models;

namespace Cosevi.SIBOAC.Controllers
{
    public class RevisionsController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: Revisions
        public ActionResult Index()
        {
            return View(db.Revision.ToList());
        }

        // GET: Revisions/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Revision revision = db.Revision.Find(id);
            if (revision == null)
            {
                return HttpNotFound();
            }
            return View(revision);
        }

        // GET: Revisions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Revisions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descripcion")] Revision revision)
        {
            if (ModelState.IsValid)
            {
                db.Revision.Add(revision);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(revision);
        }

        // GET: Revisions/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Revision revision = db.Revision.Find(id);
            if (revision == null)
            {
                return HttpNotFound();
            }
            return View(revision);
        }

        // POST: Revisions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion")] Revision revision)
        {
            if (ModelState.IsValid)
            {
                db.Entry(revision).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(revision);
        }

        // GET: Revisions/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Revision revision = db.Revision.Find(id);
            if (revision == null)
            {
                return HttpNotFound();
            }
            return View(revision);
        }

        // POST: Revisions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Revision revision = db.Revision.Find(id);
            db.Revision.Remove(revision);
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
