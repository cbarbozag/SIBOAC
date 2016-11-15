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
    public class IluminacionsController : Controller
    {
        private PC_HH_AndroidEntities db = new PC_HH_AndroidEntities();

        // GET: Iluminacions
        public ActionResult Index()
        {
            return View(db.Iluminacion.ToList());
        }

        // GET: Iluminacions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Iluminacion iluminacion = db.Iluminacion.Find(id);
            if (iluminacion == null)
            {
                return HttpNotFound();
            }
            return View(iluminacion);
        }

        // GET: Iluminacions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Iluminacions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] Iluminacion iluminacion)
        {
            if (ModelState.IsValid)
            {
                db.Iluminacion.Add(iluminacion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(iluminacion);
        }

        // GET: Iluminacions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Iluminacion iluminacion = db.Iluminacion.Find(id);
            if (iluminacion == null)
            {
                return HttpNotFound();
            }
            return View(iluminacion);
        }

        // POST: Iluminacions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descripcion,Estado,FechaDeInicio,FechaDeFin")] Iluminacion iluminacion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(iluminacion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(iluminacion);
        }

        // GET: Iluminacions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Iluminacion iluminacion = db.Iluminacion.Find(id);
            if (iluminacion == null)
            {
                return HttpNotFound();
            }
            return View(iluminacion);
        }

        // POST: Iluminacions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Iluminacion iluminacion = db.Iluminacion.Find(id);
            iluminacion.Estado = "I";
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
