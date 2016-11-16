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
    public class DelegacionsController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: Delegacions
        public ActionResult Index()
        {
            return View(db.DELEGACION.ToList());
        }

        // GET: Delegacions/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Delegacion delegacion = db.DELEGACION.Find(id);
            if (delegacion == null)
            {
                return HttpNotFound();
            }
            return View(delegacion);
        }

        // GET: Delegacions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Delegacions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] Delegacion delegacion)
        {
            if (ModelState.IsValid)
            {
                db.DELEGACION.Add(delegacion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(delegacion);
        }

        // GET: Delegacions/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Delegacion delegacion = db.DELEGACION.Find(id);
            if (delegacion == null)
            {
                return HttpNotFound();
            }
            return View(delegacion);
        }

        // POST: Delegacions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] Delegacion delegacion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(delegacion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(delegacion);
        }

        // GET: Delegacions/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Delegacion delegacion = db.DELEGACION.Find(id);
            if (delegacion == null)
            {
                return HttpNotFound();
            }
            return View(delegacion);
        }

        // POST: Delegacions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Delegacion delegacion = db.DELEGACION.Find(id);
            db.DELEGACION.Remove(delegacion);
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
