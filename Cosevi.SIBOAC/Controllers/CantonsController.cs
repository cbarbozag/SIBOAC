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
    public class CantonsController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: Cantons
        public ActionResult Index()
        {
            return View(db.CANTON.ToList());
        }

        // GET: Cantons/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Canton canton = db.CANTON.Find(id);
            if (canton == null)
            {
                return HttpNotFound();
            }
            return View(canton);
        }

        // GET: Cantons/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Cantons/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] Canton canton)
        {
            if (ModelState.IsValid)
            {
                db.CANTON.Add(canton);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(canton);
        }

        // GET: Cantons/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Canton canton = db.CANTON.Find(id);
            if (canton == null)
            {
                return HttpNotFound();
            }
            return View(canton);
        }

        // POST: Cantons/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] Canton canton)
        {
            if (ModelState.IsValid)
            {
                db.Entry(canton).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(canton);
        }

        // GET: Cantons/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Canton canton = db.CANTON.Find(id);
            if (canton == null)
            {
                return HttpNotFound();
            }
            return View(canton);
        }

        // POST: Cantons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Canton canton = db.CANTON.Find(id);
            db.CANTON.Remove(canton);
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
